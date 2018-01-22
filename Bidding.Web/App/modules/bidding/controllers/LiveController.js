(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('LiveController', LiveController);

  LiveController.$inject = ['$scope','$state'];

  function LiveController($scope, $state) {
    var vm = this;
  }
})();
