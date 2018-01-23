(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('LiveController', LiveController);

  LiveController.$inject = ['$scope', '$state', '$filter', '$q', 'ngTableParams'];

  function LiveController($scope, $state, $filter, $q, ngTableParams) {
    var vm = this;

    vm.addWatch = addWatch;
    vm.submitBid = submitBid;

    init();

    function init() {
      vm.tableAllLive = new ngTableParams(
        //{ page: 1, count: 10, filter: $scope.filter_in_grid.filter, sorting: $scope.filter_in_grid.sorting },
        {
          page: 1,
          count: 10
        },
        {
          total: 0,
          getData: getAllLive
        }
      );

      vm.tableAllLive.reload();
    };

    function getAllLive($defer, params) {
      vm.liveItems = [
        { BiddingItemId: 1, Name: 'Pen', Description: 'Blue Pen', ImageUrl: '', BidTimes: 3, Price: 3, MinNextPrice: 4, TimeLeft: '1:10:20', Expiration: '1/26/2018', OwnerId: 1, OwnerEmail: 'tzhang@asicentral.com', Status: 'Live'},
        { BiddingItemId: 2, Name: 'Mug', Description: 'Red Mug', ImageUrl: '', BidTimes: 3, Price: 5, MinNextPrice: 6, TimeLeft: '2:00:00', Expiration: '1/31/2018', OwnerId: 1, OwnerEmail: 'tzhang@asicentral.com', Status: 'Live'},
        { BiddingItemId: 3, Name: 'T-Shirt', Description: 'White T-shirt', ImageUrl: '', BidTimes: 3, Price: 10, MinNextPrice: 11, TimeLeft: '3:00:00', Expiration: '1/31/2018', OwnerId: 1, OwnerEmail: 'tzhang@asicentral.com', Status: 'Live'}
      ];

      var data = params.sorting() ? $filter('orderBy')(vm.liveItems, params.orderBy()) : vm.liveItems;
      data = params.filter() ? $filter('filter')(data, params.filter()) : data;
      params.total(data.length);
      vm.totalCount = data.length;
      //vm.SelectedRows = data.slice((params.page() - 1) * params.count(), params.page() * params.count());
      $defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));

    }

    function addWatch() {

    }

    function submitBid(bidItem) {
      // add popup window to enter price

      var action = new BiddingAction(bitItem);
      action.$save().then(function (response) { },
        function (error) { })
    }

  }
})();
