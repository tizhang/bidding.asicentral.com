(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('MenuController', MenuController);

  MenuController.$inject = ['$scope','$state'];

  function MenuController($scope, $state) {
    var vm = this;
    vm.currentTab = null;
    vm.tabs = [
      { Code: 'LiveNow', Text: 'Live Now',State:'bidding.live' , UpdatesCount: 0},
      { Code: 'MyBids', Text: 'My Bids',State:'bidding.bids', UpdatesCount: 0},
      { Code: 'MyItems', Text: 'My Items', State: 'bidding.items', UpdatesCount: 0},
      { Code: 'MyHistory', Text: 'My History', State: 'bidding.history', UpdatesCount: 0},
      { Code: 'Watch', Text: 'Watch', State: 'watch', UpdatesCount: 0}
    ];

    vm.gotoTab = gotoTab();

    init();

    function init() {
      vm.currentTab = angular.copy(vm.tabs[0]);
    }
    function gotoTab(tab) {
      if (tab && tab.State) {
        vm.currentTab = angular.copy(tab);
        $state.go(tab.State);
      }
    }
  }
})();
