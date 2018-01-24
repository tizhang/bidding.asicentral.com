(function () {
  'use strict';
  angular.module('bidding')
    .directive('dragDropCallback', dragDropCallback);

  function dragDropCallback() {
    return {
      restrict: 'A',
      scope: {
        dragDropCallback: '&'
      },
      link: function (scope, element) {
        // element.attr("draggable", "true");
        var startX;
        var startY;
        var timer = 0;
        element.on('mousedown', function (event) {
          event.preventDefault();
          startX = event.pageX;
          startY = event.pageY;
          timer = (new Date()).getTime();
        });
        element.on('mouseup', function (event) {
          event.preventDefault();
          if (scope.dragDropCallback && {}.toString.call(scope.dragDropCallback) === '[object Function]')
            scope.dragDropCallback({ x: event.pageX - startX, y: event.pageY - startY, t: (new Date()).getTime() - timer });
        });
      }

    };
  }
})();
