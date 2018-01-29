(function () {
  'use strict';

  angular
    .module('bidding')
    .directive('countdownTo', countdownTo);

  function countdownTo() {
    return {
      restrict: 'A',
      scope: {
        originalTime: '=countdownTo',
        timeoutCallback: '='
      },
      controller: CountdownController,
      controllerAs: 'vm',
      bindToController: true,
      replace: true,
      templateUrl: 'App/modules/bidding/partials/directives/countdown.html'
    };
  }

  CountdownController.$inject = ['$rootScope', '$scope', '$interval'];

  function CountdownController($rootScope, $scope, $interval) {
    var vm = this;
    // vm.time
    vm.time = new Date(Date.parse(vm.originalTime));
    vm.countdown = '';
    vm.getCountDown = getCountDown;
    vm.pause = null;
    init();

    function init() {
      $scope.$watch('vm.originalTime', function (newv, oldv) {
        if (newv !== oldv) {
          vm.time = new Date(Date.parse(newv));
          //if (newv instanceof Date) {
          //  vm.time = newv;
          //} else {
          //}
        }
      });
      
      vm.pause = $interval(function () {
        vm.countdown = getCountDown();
      }, 1000);
    }
    function getCountDown() {
      if (vm.time) {
        var millionseconds = vm.time.getTime() - new Date().getTime();
      }
      if (millionseconds <= 0 && (vm.timeoutCallback && {}.toString.call(vm.timeoutCallback) === '[object Function]'))
        vm.timeoutCallback();
      return ms2dhms(millionseconds);
    }

    function ms2dhms(ms) {
      var totalSeconds = Math.floor(ms / 1000);
      var d = Math.floor(totalSeconds / 86400);
      var ds = totalSeconds % 86400;
      var h = Math.floor(ds / 3600);
      var dh = ds % 3600;
      var m = Math.floor(dh / 60);
      var s = dh % 60;
      var rd = d > 0 ? d + 'd' : '';
      var rh = h > 0 ? (h > 9 ? h + 'h' : '0' + h + 'h') : '';
      var rm = m > 0 ? (m > 9 ? m + 'm' : '0' + m + 'm') : '';
      var rs = s > 0 ? (s > 9 ? s + 's' : '0' + s + 's') : '00s';
      var r = rd + rh + rm + rs;
      return r === '00s' ? '' : r;
    }
  }
})();
