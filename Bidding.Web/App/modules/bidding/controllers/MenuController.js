(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('MenuController', MenuController);

  MenuController.$inject = ['$scope', '$state', 'Notification', '$cookies'];

  function MenuController($scope, $state, Notification, $cookies) {
    var vm = this;
    vm.currentTab = null;
    vm.tabs = [
      { Code: 'LiveNow', Text: 'Live Now',State:'bidding.live' , UpdatesCount: 0},
      { Code: 'MyItems', Text: 'My Items', State: 'bidding.myitems', UpdatesCount: 0},
      { Code: 'MyHistory', Text: 'My History', State: 'bidding.history', UpdatesCount: 0},
      { Code: 'Watch', Text: 'Watch', State: 'watch', UpdatesCount: 0}
    ];
    vm.AlertList = [];

    vm.gotoTab = gotoTab;
    vm.viewItem = viewItem;
    
    init();

    function init() {
      vm.currentTab = angular.copy(vm.tabs[0]);
      vm.UserId = $cookies.get('UserID');

      //vm.AlertList = [
      //  { Id: 1, BiddingItemId: 1, ImageUrl: '', Message: 'bidder tbidding1 submitted new bid price 2 for pen', CreateDate: '2018-01-24T09:43:43.313', EventTime: '2018-01-24T09:43:43.313'},
      //  { Id: 2, BiddingItemId: 1, ImageUrl: '', Message: 'bidder tbidding2 submitted new bid price 16 for pen', CreateDate: '2018-01-24T09:43:43.327', EventTime: '2018-01-24T09:43:43.327' },
      //  { Id: 5, BiddingItemId: 1, ImageUrl: '', Message: 'bidder yfang submitted new bid price 2 for lanyard', CreateDate: '2018-01-24T09:43:43.343', EventTime: '2018-01-24T09:43:43.343' }
      //];

      Notification.getByUserId({ userid:vm.UserId })
        .then(
        function (resp) {
          vm.AlertList = resp;
        },
        function (err) {
          console.log(err);
        });
    }
    function gotoTab(tab) {
      if (tab && tab.State) {
        vm.currentTab = angular.copy(tab);
        $state.go(tab.State);
      }
    }

    function viewItem(item) {
      $state.go('bidding.details', { id: item.BiddingItemId });
    }

    //vm.test = function () {
    //  $state.go('bidding.details', { id: 2 });
    //}
  }
})();
