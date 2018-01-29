(function () {
  'use strict';

  angular
    .module('bidding')
    .directive('bidBtn', bidBtn);

  function bidBtn() {
    return {
      restrict: 'A',
      scope: {
        model: '=bidBtn'
      },
      controller: bidBtnController,
      controllerAs: 'vm',
      bindToController: true,
      replace: true,
      templateUrl: 'App/modules/bidding/partials/directives/bidbtn.html'
    };
  }

  bidBtnController.$inject = ['$rootScope', '$scope', 'BiddingAction', '$cookies'];

  function bidBtnController($rootScope, $scope, BiddingAction, $cookies) {
    var vm = this;
    vm.editing = false;
    vm.price = vm.model.Price + vm.model.Setting.MinIncrement

    vm.cancel = cancel;
    vm.edit = edit;
    vm.submit = submit;
    init();

    function init() {
      vm.action = new BiddingAction({ ItemId: vm.model.Id, Price: vm.model.Price });
      vm.action.prepareBidderById($cookies.get('UserID'));
    }

    function cancel() {
      vm.price = vm.model.Price + vm.model.Setting.MinIncrement;
      vm.editing = false;
    }

    function edit() {
      vm.editing = true;
    }

    function submit() {
      if (vm.model.Setting.CanSeeCurrentPrice) {
        if (vm.model.Setting.MinIncrement > 0 && vm.price < vm.model.Price + vm.model.Setting.MinIncrement) {
          alert('Price should not be less than' + (vm.model.Price + vm.model.Setting.MinIncrement));
          return;
        }
        if (vm.model.Setting.MinIncrement < 0 && vm.price > vm.model.Price + vm.model.Setting.MinIncrement) {
          alert('Price should not be more than' + (vm.model.Price + vm.model.Setting.MinIncrement));
          return;
        }
      }
      vm.action.Price = vm.price;
      vm.action.$save().then(
        function (resp) {
          vm.model.Price = resp.Price;
          init();
          vm.editing = false;
        },
        function (err) {
          alert(err);
          vm.editing = false;
        });
    }
  }
})();
