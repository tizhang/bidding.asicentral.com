(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('BiddingHistoryController', BiddingHistoryController);

  BiddingHistoryController.$inject = ['$scope', '$state', '$filter', '$q', 'ngTableParams', 'BiddingItem', '$cookies'];

  function BiddingHistoryController($scope, $state, $filter, $q, ngTableParams, BiddingItem, $cookies) {
    var vm = this;
    vm.historyListById = historyListById;

    init();

    function init() {
      vm.UserId = $cookies.get('UserID');

      vm.tableMyHistory = new ngTableParams(
        //{ page: 1, count: 10, filter: $scope.filter_in_grid.filter, sorting: $scope.filter_in_grid.sorting },
        {
          page: 1,
          count: 10
        },
        {
          total: 0,
          getData: getMyHistory
        }
      );

      vm.tableMyHistory.reload();
    };

    function getMyHistory($defer, params) {
      //vm.history = [
      //  { BiddingItemId: 1, Name: 'Pen', Description: 'Blue Pen', ImageUrl: '', BidTimes: 3, Price: 3, MinNextPrice: 4, TimeLeft: '1:10:20', Expiration: '1/26/2018', OwnerId: 1, OwnerEmail: 'tzhang@asicentral.com', Status: 'STAG'},
      //  { BiddingItemId: 2, Name: 'Mug', Description: 'Red Mug', ImageUrl: '', BidTimes: 3, Price: 5, MinNextPrice: 6, TimeLeft: '2:00:00', Expiration: '1/31/2018', OwnerId: 1, OwnerEmail: 'tzhang@asicentral.com', Status: 'STAG'},
      //  { BiddingItemId: 3, Name: 'T-Shirt', Description: 'White T-shirt', ImageUrl: '', BidTimes: 3, Price: 10, MinNextPrice: 11, TimeLeft: '3:00:00', Expiration: '1/31/2018', OwnerId: 1, OwnerEmail: 'tzhang@asicentral.com', Status: 'STAG'}
      //];

      //var data = params.sorting() ? $filter('orderBy')(vm.history, params.orderBy()) : vm.history;
      //data = params.filter() ? $filter('filter')(data, params.filter()) : data;
      //params.total(data.length);
      //vm.totalCount = data.length;
      //$defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));


      BiddingItem.getByGroup({ group: '', bidderId: vm.UserId, includeSettings: true, includeHistory: true })
        .then(
        function (resp) {
          //vm.history = resp;

          // only STAG/ACTV/SUCC/FAIL
          vm.history = $filter('filter')(resp, function (status) {
          return(resp.status != 'DRAF');
          });

          angular.forEach(vm.history, function (item) {
            item.BiddingType = item.Setting.MinIncrement > 0 ? 'High Win' : 'Low Win';
            var histList = $filter('filter')(item.History, function (rec) {
              return (rec.Bidder.Id == vm.UserId)
            });

            var bidderHistory = $filter('orderBy')(histList, 'Price');
            if (bidderHistory.length > 0) {
              bidderHistory.reverse();
              item.MyLastBid = bidderHistory[0].Price;
            }
            else {
              item.MyLastBid = null;
            }
            });


          var data = params.sorting() ? $filter('orderBy')(vm.history, params.orderBy()) : vm.history;
          data = params.filter() ? $filter('filter')(data, params.filter()) : data;
          params.total(data.length);
          vm.totalCount = data.length;
          //vm.SelectedRows = data.slice((params.page() - 1) * params.count(), params.page() * params.count());
          $defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
        },
        function (err) {
          console.log(err);
        });

    }

    function historyListById(itemId) {
      //$state.go('bidding.history.list', { id: itemId });
    }

    function submitBid(bidItem) {
      // add popup window to enter price

      var action = new BiddingAction(bitItem);
      action.$save().then(function (response) { },
        function (error) { })
    }

  }
})();
