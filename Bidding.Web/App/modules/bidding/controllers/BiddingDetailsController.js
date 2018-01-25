(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('BiddingDetailsController', BiddingDetailsController);

  BiddingDetailsController.$inject = ['$scope', 'BiddingItem', 'model', '$uibModalInstance', 'mode'];

  function BiddingDetailsController($scope, BiddingItem, model, $uibModalInstance, mode) {
    var vm = this;

    vm.applyBidGroup = false;
    vm.enableBidTimes = false;
    vm.groups = [['WESP'], ['SESP'], ['ADMT']];
    vm.mode = 'view';
    vm.model = null;
    vm.tabs = ['detail'];
    vm.tab = 'detail';

    vm.bid = bid;
    vm.cancel = cancel;
    vm.close = close;
    vm.save = save;
    vm.watch = watch;

    init();
    function init() {
      if (mode == 'add')
        vm.model = new BiddingItem();
      if (vm.model.Status == 'DRAF')
        vm.mode = mode;
      vm.applyBidGroup = vm.model.Setting.Groups && vm.model.Setting.Groups.length;
      vm.enableBidTimes = vm.model.Setting.BidTimePerUser;
      if (vm.model.Status != 'DRAF' && vm.model.Status != 'STAG' && vm.model.History && vm.model.History.length) {
        vm.tabs.push('history');
      }
    }

    function bid(model) {
      console.log('bid');
      console.log(model);
    }

    function watch(model) {
      console.log('watch');
      console.log(model);
    }

    function cancel() {
      $uibModalInstance.dismiss('cancel');
    }

    function close() {
      $uibModalInstance.close(vm.model);
    }

    function save() {
      vm.model.$save().then(
        function (resp) {
          vm.model = resp;
          if (vm.mode == 'add') {
            vm.mode == 'edit';
          } else if (vm.mode == 'edit' && vm.model.Status != 'DRAF') {
            vm.mode = 'view'
          }
        },
        function (err) {
          $uibModalInstance.dismiss('error');
        });

    }
  }
})();
