(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('BiddingDetailsController', BiddingDetailsController);

  BiddingDetailsController.$inject = ['$scope', '$state', 'BiddingItem'];

  function BiddingDetailsController($scope, $state, BiddingItem) {
    var vm = this;
    vm.item = null;
    vm.bid = bid;
    vm.watch = watch;

    init();
    function init() {
      BiddingItem.get(2)
        .then(
        function (res) {
          vm.item = res;
        },
        function (err) {
          console.log(err);
        });
    }

    function bid(model) {
      console.log('bid');
      console.log(model);
    }
    

    function watch(model) {
      console.log('watch');
      console.log(model);
    }
  }
})();
