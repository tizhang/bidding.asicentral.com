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
      });
  }
})();
