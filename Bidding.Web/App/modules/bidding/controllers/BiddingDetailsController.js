(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('BiddingDetailsController', BiddingDetailsController);

  BiddingDetailsController.$inject = ['$scope', '$state', 'BiddingItem'];

  function BiddingDetailsController($scope, $state, BiddingItem) {
    var vm = this;
    vm.item = null;
    BiddingItem.get(2)
      .then(
      function (res) {
        vm.item = res;
      },
      function (err) {
        console.log(err);
      });
  }
})();
