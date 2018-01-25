(function () {
  'use strict';

  angular
    .module('bidding')
    .directive('watchBtn', watchBtn);

  function watchBtn() {
    return {
      restrict: 'A',
      scope: {
        model: '=watchBtn'
      },
      controller: watchBtnController,
      controllerAs: 'vm',
      bindToController: true,
      replace: true,
      templateUrl: 'App/modules/bidding/partials/directives/watchbtn.html'
    };
  }

  watchBtnController.$inject = ['$rootScope', '$scope','myWatchList','$cookies'];

  function watchBtnController($rootScope, $scope, myWatchList,$cookies) {
    var vm = this;
    vm.watching = null;
    vm.watch = watch;
    vm.unwatch = unwatch;
    init();

    function init() {
      myWatchList.iAm($cookies.get('UserID'), function () {
        vm.watching = myWatchList.isWatching(vm.model.Id);
      });
    }

    function watch() {
      myWatchList.watch(vm.model.Id, function (watching) {
        vm.watching = watching;
      });
    }

    function unwatch() {
      myWatchList.unwatch(vm.model.Id, function (watching) {
        vm.watching = watching;
      });
    }
  }
})();
