(function () {
  'use strict';

  angular
    .module('bidding')
    .config(routeConfig);

  routeConfig.$inject = ['$stateProvider'];

  function routeConfig($stateProvider) {
    $stateProvider
      .state('bidding', {
        abstract: true,
        views: {
          'menu': {
            templateUrl: 'App/modules/bidding/partials/menu.html',
            controller: 'MenuController',
            controllerAs: 'vm'
          }
        }
      })
      .state('bidding.live', {
        url: '/',
        views: {
          'content': {
            templateUrl: 'App/modules/bidding/partials/live.html',
            controller: 'LiveController',
            controllerAs: 'vm',
          }
        }
      })
      .state('bidding.myitems', {
        url: '/myitems',
        views: {
          'content': {
            templateUrl: 'App/modules/bidding/partials/biddingMyItems.html',
            controller: 'BiddingMyItemsController',
            controllerAs: 'vm',
          }
        },
        //resolve: {
        //  resolvedItem: getBiddingItem
        //}
      })
      .state('bidding.history', {
        url: '/history',
        views: {
          'content': {
            templateUrl: 'App/modules/bidding/partials/biddingHistory.html',
            controller: 'BiddingHistoryController',
            controllerAs: 'vm',
          }
        }
      });


      //getBiddingItem.$inject = ['$stateParams', '$state', 'BiddingItem'];

      //function getBiddingItem($stateParams, $state, BiddingItem,) {
      //  return BiddingItem.get($stateParams.id).catch(function (err) { console.log(err); });
      //}
  }
})();
