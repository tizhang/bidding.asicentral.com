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
          res.Status = 'ACTV';
          if (!res.Settings.EndDate) {
            var d = new Date();
            d.setUTCDate(d.getUTCDate() + 1);
            d.setUTCHours(d.getUTCHours() + 2);
            d.setUTCMinutes(d.getUTCMinutes() + 3);
            d.setUTCSeconds(d.getUTCSeconds() + 4);
            res.Settings.EndDate = d;
          }
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
