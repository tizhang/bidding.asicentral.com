(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('BiddingAction', BiddingAction);

  BiddingAction.$inject = ['$modelFactory', 'httpInterceptor', 'BiddingUser'];

  function BiddingAction($modelFactory, httpInterceptor, BiddingUser) {
    var model = $modelFactory('api/action', {
      pk: 'Id',
      map: {
      },
      defaults: {
        ItemId: null,
        Bidder: null,
        BidderId: null,
        Price: null,
        ActionTimeUTC: null,
        Status: null
      },
      actions: {
        'base': {
          interceptor: httpInterceptor
        },
        'getByItem': {
          method: 'GET',
          url: '/item',
          wrap: false
        },
        'getByUser': {
          method: 'GET',
          url: '/user',
          wrap: false
        }
      },
      instance: {
        prepareBidderById: function (userid) {
          if (userid) {
            this.BidderId = userid;
            this.Bidder = new BiddingUser({ Id: userid });
          }
        }
      }
    });

    return model;
  }
})();
