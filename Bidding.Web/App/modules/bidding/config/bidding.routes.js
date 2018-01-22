(function () {
  'use strict';

  angular
    .module('bidding')
    .config(routeConfig);

  routeConfig.$inject = ['$stateProvider'];

  function routeConfig($stateProvider) {
    $stateProvider
      .state('bidding', {
        url: '/',
        templateUrl: 'App/modules/bidding/partials/home.html',
        controller: 'HomeController',
        controllerAs: 'vm',
        params: {
        },
        resolve: {
        }
      });
  }
})();
