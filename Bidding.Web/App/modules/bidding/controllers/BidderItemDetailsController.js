(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('BidderItemDetailsController', BidderItemDetailsController);

  BidderItemDetailsController.$inject = ['$scope', 'BiddingItem', 'model', '$uibModalInstance', 'mode', '$cookies', '$filter'];

  function BidderItemDetailsController($scope, BiddingItem, model, $uibModalInstance, mode, $cookies, $filter) {
    var vm = this;

    vm.model = model;
    vm.myProducts = [];

    vm.addNewProduct = addNewProduct;
    vm.bid = bid;
    vm.cancel = cancel;
    vm.close = close;
    vm.email = email;
    vm.save = save;
    vm.selected = null;
    vm.selectProduct = selectProduct;
    vm.stage = stage;
    vm.watch = watch;

    init();

    function init() {
      vm.model = new BiddingItem(vm.model);
      if (vm.model.Name === '1000 shirts') {
        vm.myProducts = [
          { Id: 1, src: 'http://matchem.com/wp-content/uploads/2015/03/tumblr_m4m270iUfv1r6dybk-e1338300619778.jpeg' },
          { Id: 2, src: 'http://imshopping.rediff.com/imgshop/300-400/shopping/pixs/3807/b/blackshirt__stylish-party-wear-black-shirt-for-men._stylish-party-wear-black-shirt-for-men.jpg' }
        ];
        selectProduct(vm.myProducts[0]);
      }
    }

    function addNewProduct() {
      alert('To be implemented.');
    }

    function selectProduct(prod) {

      vm.selected = prod.Id;
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
          $uibModalInstance.close(vm.model);
        }).catch(
        function (err) {
          $uibModalInstance.dismiss('error');
        });

    }
  }
})();
