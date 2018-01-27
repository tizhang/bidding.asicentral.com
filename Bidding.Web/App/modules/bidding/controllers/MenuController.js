(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('MenuController', MenuController);

  MenuController.$inject = ['$scope', '$state', 'Notification', '$cookies', 'modalFactory', 'modalOptions', 'BiddingItem', 'NotificationAck', 'heartBeat'];

  function MenuController($scope, $state, Notification, $cookies, modalFactory, modalOptions, BiddingItem, NotificationAck, heartBeat) {
    var vm = this;
    vm.currentTab = null;
    vm.tabs = [
      { Code: 'LiveNow', Text: 'Live Now', State: 'bidding.live', UpdatesCount: 0 },
      { Code: 'MyItems', Text: 'My Items', State: 'bidding.myitems', UpdatesCount: 0 },
      { Code: 'MyHistory', Text: 'My History', State: 'bidding.history', UpdatesCount: 0 },
      { Code: 'Watch', Text: 'Watch', State: 'watch', UpdatesCount: 0 }
    ];
    vm.AlertList = [];
    vm.todoList = [];

    vm.gotoTab = gotoTab;
    vm.noteAck = noteAck;;
    vm.viewItem = viewItem;

    init();

    function init() {
      var tab = findTabByState($state.current.name);
      vm.currentTab = tab ? angular.copy(tab) : angular.copy(vm.tabs[0]);
      vm.UserId = $cookies.get('UserID');
      //loadNotifications();
      heartBeat.init(5000);
      heartBeat.register('notification', loadNotifications, handleNotification);

      function findTabByState(state) {
        for (var i = 0; i < vm.tabs.length; i++) {
          if (vm.tabs[i].State == state)
            return vm.tabs[i];
        }
        return null;
      }
    }

    function loadNotifications(callback) {
      Notification.getByUserId({ userid: vm.UserId })
        .then(
        function (resp) {
          vm.todoList = getNewer(vm.AlertList, resp);
          vm.AlertList = resp;
          callback(true);
        },
        function (err) {
          console.log(err);
          callback(false);
        });
    }

    function handleNotification() {
      for (var i = 0; i < vm.todoList.length; i++) {
        BiddingItem.get(vm.todoList[i].BiddingItemId).then(
          function (item) {
            $rootScope.$broadcast('itemChanged', item);
          },
          function (err) {
            console.log(err);
          });

      }
    }

    function gotoTab(tab) {
      if (tab && tab.State) {
        vm.currentTab = angular.copy(tab);
        $state.go(tab.State);
      }
    }

    function viewItem(item) {
      BiddingItem.get(item.BiddingItemId).then(
        function (model) {
          modalFactory.open(model, modalOptions.viewItem);
        },
        function (err) {
          console.log(err)
        });
    }

    function noteAck() {
      var time = lastestNoteTime();
      var ack = new NotificationAck({ UserId: vm.UserId, LastAccessDate: time });
      NotificationAck.dismissFrom(ack).then(
        function () {
          loadNotifications();
        },
        function (err) {
          console.log(err)
        });
    }

    function lastestNoteTime() {
      if (vm.AlertList.length > 0) {
        var result = Date.parse(vm.AlertList[0].CreateDate);
        for (var i = 0; i < vm.AlertList; i++) {
          var current = Date.parse(vm.AlertList[i].CreateDate);
          if (result < current) {
            result = curent;
          }
        }
        return new Date(result);
      }
      return new Date();
    }

    function getNewer(origin, updated) {
      var result = [];
      if (origin.length < 1)
        return updated;
      var LastTime = Date.parse(origin[0].EventTime);
      return updated.filter(function (a) { return Date.parse(a.EventTime) > LastTime });
    }
    
  }
})();
