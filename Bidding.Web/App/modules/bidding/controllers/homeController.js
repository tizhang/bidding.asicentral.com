(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('HomeController', HomeController);

  HomeController.$inject = ['$scope'];

  function HomeController($scope) {
    var vm = this;
    vm.user = { Name: 'world' };
  }
})();
