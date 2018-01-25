(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('MyItemDetailsController', MyItemDetailsController);

  MyItemDetailsController.$inject = ['$scope', 'BiddingItem', 'model', '$uibModalInstance', 'mode', '$cookies'];

  function MyItemDetailsController($scope, BiddingItem, model, $uibModalInstance, mode,$cookies) {
    var vm = this;
    
    vm.applyBidGroup = false;
    vm.enableBidTimes = false;
    vm.groups = [['WESP'], ['SESP'], ['ADMT']];
    vm.mode = 'view';
    vm.model = model;
    vm.tabs = ['detail'];
    vm.tab = 'detail';

    vm.bid = bid;
    vm.cancel = cancel;
    vm.close = close;
    vm.email = email;
    vm.save = save;
    vm.stage = stage;
    vm.watch = watch;

    init();
    function init() {
      if (mode == 'add') {
        vm.model = new BiddingItem();
        vm.model.Setting.StartDate = new Date();
        vm.model.Setting.EndDate = new Date();
        vm.model.Owner = { Id: $cookies.get('UserID') };
      }
      if (vm.model.Status == 'DRAF')
        vm.mode = mode;
      vm.model = new BiddingItem(vm.model);
      vm.applyBidGroup = vm.model.Setting.Groups && vm.model.Setting.Groups.length;
      vm.enableBidTimes = vm.model.Setting.BidTimePerUser > 0;
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

    function email(string) {
      alert(string);
    }

    function cancel() {
      $uibModalInstance.dismiss('cancel');
    }

    function close() {
      $uibModalInstance.close(vm.model);
    }

    function stage() {
      vm.model.Status = 'STAG';
      save();
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
