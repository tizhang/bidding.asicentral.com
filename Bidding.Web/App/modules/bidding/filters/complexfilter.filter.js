
(function () {
  'use strict';

  angular
    .module('bidding')
    .filter('complexfilter', function () {
      return function (items, filter) {
        var result = [];
        angular.forEach(items, function (item, index) {
          var matched = true;
          matched = matched && (!item.Setting || !item.Setting.Group || !filter.Setting || item.Setting.Group == filter.Setting.Group);
          matched = matched && (!item.custom || !filter.custom || filter.custom.bidded === undefined || filter.custom.bidded == item.custom.bidded);
          matched = matched && (!item.custom || !filter.custom || filter.custom.watched === undefined || filter.custom.watched == item.custom.watched);
          if (matched)
            result.push(item);
        });
        return result;
      };
    });
})();
