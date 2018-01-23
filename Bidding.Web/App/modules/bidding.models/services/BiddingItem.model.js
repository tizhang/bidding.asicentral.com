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
        Settings: BiddingSettings,
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
          interceptor: httpInterceptor
        },
        'getByGroup': {
          method: 'GET',
          url: '/group'
        }
      },
      instance: {
      },
      init: init
    });

    return model;

    function init(instance) {
      instance.Settings = new BiddingSettings(instance.Settings);
    }
  }
})();
