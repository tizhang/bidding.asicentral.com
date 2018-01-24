﻿(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('BiddingMyItemsController', BiddingMyItemsController);

  BiddingMyItemsController.$inject = ['$scope', '$state', '$filter', '$q', 'ngTableParams', 'BiddingItem'];

  function BiddingMyItemsController($scope, $state, $filter, $q, ngTableParams, BiddingItem) {
    var vm = this;

    vm.addItem = addItem;
    vm.editItem = editItem;
    vm.deleteItem = deleteItem;
    vm.sendEmail = sendEmail;
    vm.viewItem = viewItem;

    init();

    function init() {
      vm.tableMyItems = new ngTableParams(
        //{ page: 1, count: 10, filter: $scope.filter_in_grid.filter, sorting: $scope.filter_in_grid.sorting },
        {
          page: 1,
          count: 10
        },
        {
          total: 0,
          getData: getMyItems
        }
      );

      vm.tableMyItems.reload();
    };

    function getMyItems($defer, params) {
      vm.myItems = [
        { BiddingItemId: 1, Name: 'Pen', Description: 'Blue Pen', ImageUrl: '', BidTimePerUser: 10, Price: 10, BiddingType: 'High Win', MinIncrement: 1, StartDate: '1/23/2018', EndDate: '2/10/2018', MinIncrement: 1, Status: 'ACTV'},
        { BiddingItemId: 2, Name: 'Mug', Description: 'Red Mug', ImageUrl: '', BidTimePerUser: 10, Price: 15, BiddingType: 'Low Win', MinIncrement: 2, StartDate: '2/26/2018', EndDate: '2/28/2018', MinIncrement: 1, Status: 'DRAF'},
        { BiddingItemId: 3, Name: 'T-Shirt', Description: 'White T-shirt', ImageUrl: '', BidTimePerUser: 10, Price: 10, BiddingType: 'High Win', MinIncrement: 3, StartDate: '1/28/2018', EndDate: '1/31/2018', MinIncrement: 1, Status: 'STAG'}
      ];

      var data = params.sorting() ? $filter('orderBy')(vm.myItems, params.orderBy()) : vm.myItems;
      data = params.filter() ? $filter('filter')(data, params.filter()) : data;
      params.total(data.length);
      vm.totalCount = data.length;
      //vm.SelectedRows = data.slice((params.page() - 1) * params.count(), params.page() * params.count());
      $defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));

    }

    function addItem() {

    }

    function editItem(item) {
      // edit mode
      $state.go('bidding.details', { id: item.BiddingItemId });
    }

    function deleteItem(item) {
      var msgConfirm = '';

      if (item.Status == 'DRAF') {
        msgConfirm = 'Are you sure you want to delete this item?';
      };
      BootstrapDialog.confirmdirty(msgConfirm, function (result) {
        if (result) {
          var bid = new BiddingItem(item);
          bid.$delete({ id: item.BiddingItemId },
            function (response) {
            },
            function (error) {
            });

          //BiddingItem.delete(item.BiddingItemId)
          //  .then(
          //  function (res) {
          //    var result = res;
          //  },
          //  function (err) {
          //    console.log(err);
          //  });

        }
        else {
          return;
        }
      });
    }

    function sendEmail(item) {
     
    }

    function viewItem(item) {
      // view mode
      $state.go('bidding.details', { id: item.BiddingItemId });
    }
    
  }
})();
