(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('LiveController', LiveController);

  LiveController.$inject = ['$scope', '$state', '$filter', 'BiddingItem', 'modalFactory', 'modalOptions', 'myWatchList', '$cookies'];

  function LiveController($scope, $state, $filter, BiddingItem, modalFactory, modalOptions, myWatchList, $cookies) {
    var vm = this;
    vm.activeItems = null;
    vm.filterBy = {
      '#activeGallery': {}, '#stagedGallery': {}
    };
    vm.filterOptions = { '#activeGallery': [], '#stagedGallery': [] };
    vm.maxIndex = { '#activeGallery': 0, '#stagedGallery': 0 };
    vm.showFromIndex = { '#activeGallery': 0, '#stagedGallery': 0 };
    vm.sortBy = { '#activeGallery': '-CreateDate', '#stagedGallery': '-CreateDate' };
    vm.sortOptions = { '#activeGallery': [], '#stagedGallery': [] };
    vm.stagedItems = null;
    //vm.watchedIds = myWatchList; // TODO load
    vm.biddedIds = []; // TODO load

    vm.bid = bid;
    vm.galleryDragMove = galleryDragMove;
    vm.galleryMove = galleryMove;
    vm.view = view;
    vm.watch = watch;

    vm.test = function () {
      console.log(vm.filterBy['#activeGallery']);
      console.log(vm.activeItems);
    };

    init();

    function init() {

      var groups = $cookies.get('AccessibleGroups');
      BiddingItem.getByGroup({ groups: groups, includeSettings: true, includeHistory: true })
        .then(
        function (resp) {
          myWatchList.iAm($cookies.get('UserID'), function () {
            vm.activeItems = $filter('filter')(resp, { Status: "ACTV" });
            vm.maxIndex['#activeGallery'] = vm.activeItems.length ? vm.activeItems.length - 1 : 0;
            generateSortFilterOptions('#activeGallery', vm.activeItems);
            vm.stagedItems = $filter('filter')(resp, { Status: "STAG" });
            vm.maxIndex['#stagedGallery'] = vm.stagedItems.length ? vm.stagedItems.length - 1 : 0;
            generateSortFilterOptions('#stagedGallery', vm.stagedItems);
          });
        },
        function (err) {
          console.log(err);
        });
    }

    function bid(model) {
      console.log('bid');
      console.log(model);
    }


    function watch(model) {
      console.log('watch');
      console.log(model);
    }

    function galleryMove(id, toRight) {
      var str = angular.element(id).css('left');
      var valueStr = str.substring(0, str.length - 2).trim();
      var left = parseInt(valueStr);
      if (toRight) {
        left -= 220;
        vm.showFromIndex[id]++;
      } else {
        left += 220;
        vm.showFromIndex[id]--;
      }
      angular.element(id).css('left', left + 'px');
    }

    function galleryDragMove(id, distance, time) {
      //var distance = dragResult.x;
      //var id = '#' + dragResult.id;
      //var time = dragResult.t;
      if (Math.abs(distance) < 50)
        return;
      var str = angular.element(id).css('left');
      var valueStr = str.substring(0, str.length - 2).trim();
      var left = parseInt(valueStr);
      var step = Math.floor(distance * time / 50000);
      left += 220 * step;
      if (vm.showFromIndex[id] - step <= 0) {
        left = 0;
        vm.showFromIndex[id] = 0;
      } else if (vm.showFromIndex[id] - step >= vm.maxIndex[id]) {
        left = (- vm.maxIndex[id]) * 220;
        vm.showFromIndex[id] = vm.maxIndex[id];
      } else {
        vm.showFromIndex[id] -= step;
      }
      angular.element(id).css('left', left + 'px');
    }

    function generateSortFilterOptions(id, items) {
      var Groups = [{ name: 'All' }];
      vm.filterOptions[id].Types = [{ name: 'All' }, { name: 'Bidded', code: { bidded: true } }, { name: 'Watching', code: { watched: true } }];
      vm.sortOptions[id] = [
        { name: 'Popularity', code: '+BidTimes' },
        { name: 'Latest', code: '-CreateDate' },
        { name: 'Start Date', code: '+Setting.StartDate' },
        { name: 'End Date', code: '+Setting.EndDate' },
        { name: 'Price', code: '+Price' }];
      angular.forEach(items, function (value, index) {
        if (value.Setting && value.Setting.Group) {
          Groups.push({ name: value.Setting.Group, code: { Group: value.Setting.Group } });
        }
        items[index].custom = {};
        items[index].custom.watched = myWatchList.isWatching(value.Id);
        items[index].custom.bidded = vm.biddedIds.includes(value.Id);

      });
      vm.filterOptions[id].Groups = groupUnique(Groups);
    }
    function groupUnique(array) {
      var a = angular.copy(array);
      for (var i = 0; i < a.length; ++i) {
        for (var j = i + 1; j < a.length; ++j) {
          if (a[i].name === a[j].name)
            a.splice(j--, 1);
        }
      }
      return a;
    };

    function view(model) {
      modalFactory.open(model, modalOptions.viewItem);
    }

    $scope.$on('itemChanged', function (evnet, data) {
      if (!findUpdate(vm.activeItems, data))
        findUpdate(vm.stagedItems, data);
      //vm.activeItems = $filter('filter')(resp, { Status: "ACTV" });
      //vm.maxIndex['#activeGallery'] = vm.activeItems.length ? vm.activeItems.length - 1 : 0;
      //generateSortFilterOptions('#activeGallery', vm.activeItems);
      //vm.stagedItems = $filter('filter')(resp, { Status: "STAG" });
      //vm.maxIndex['#stagedGallery'] = vm.stagedItems.length ? vm.stagedItems.length - 1 : 0;
      //generateSortFilterOptions('#stagedGallery', vm.stagedItems);
    });

    function findUpdate(array, item) {
      for (var i = 0; array && i < array.length; i++) {
        if (array[i].Id == item.Id) {
          array[i] = item;
          return true;
        }
      }
      return false;
    }
  }
})();
