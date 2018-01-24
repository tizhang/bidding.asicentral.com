(function () {
  'use strict';

  angular
    .module('bidding')
    .directive('biddingItemCard', BiddingItemCard);

  function BiddingItemCard() {
    return {
      restrict: 'E',
      scope: {
        model: '=ngModel',
        bidHandler: '&',
        watchHandler: '&',
        clickHandler: '&',
      },
      controller: BiddingItemCardController,
      controllerAs: 'vm',
      bindToController: true,
      replace: true,
      templateUrl: 'App/modules/bidding/partials/directives/item-card.html'
    };
  }

  BiddingItemCardController.$inject = ['$rootScope', '$state', '$scope'];

  function BiddingItemCardController($rootScope, $state, $scope) {
    var vm = this;
    // vm.model
  }
})();
