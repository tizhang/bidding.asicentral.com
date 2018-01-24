(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('BiddingAction', BiddingAction);

  BiddingAction.$inject = ['$modelFactory', 'httpInterceptor'];

  function BiddingAction($modelFactory, httpInterceptor) {
    var model = $modelFactory('api/action', {
      pk: 'Id',
      map: {
      },
      defaults: {
        ItemId: null,
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
      }
    });

    return model;
  }
})();
