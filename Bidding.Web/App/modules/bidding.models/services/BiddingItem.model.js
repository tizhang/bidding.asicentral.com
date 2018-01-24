(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('BiddingItem', BiddingItem);

  BiddingItem.$inject = ['$modelFactory', 'httpInterceptor', 'BiddingSettings', 'BiddingAction', 'BiddingUser'];

  function BiddingItem($modelFactory, httpInterceptor, BiddingSettings, BiddingAction, User) {
    var model = $modelFactory('api/bidding', {
      pk: 'Id',
      map: {
        Setting: BiddingSettings,
        History: BiddingAction.List,
        Owner: User
      },
      defaults: {
        Name: '',
        Description: '',
        ImageUrl: '',
        BidTimes: 0,
        Price: 0.00,
        Status: 'Draft',
        CreateDate: null,
      },
      actions: {
        'base': {
          interceptor: httpInterceptor,
          beforeRequest: function () {
            beforeRequestCleanup(this.data);
          },
        },
        'getByGroup': {
          method: 'GET',
          url: '/',
          wrap: false
        }
      },
      instance: {
      },
      init: init
    });

    return model;

    function init(instance) {
      instance.Setting = new BiddingSettings(instance.Setting);
    }

    function beforeRequestCleanup(data) {
      delete data.custom;
    }
  }
})();
