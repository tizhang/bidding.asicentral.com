(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('Watcher', Watcher);

  Watcher.$inject = ['$modelFactory', 'httpInterceptor'];

  function Watcher($modelFactory, httpInterceptor) {
    var model = $modelFactory('api/watch', {
      pk: 'Id',
      map: {
      },
      defaults: {
        BiddingItemId: null,
        UserId: null,
        IsActive: false
      },
      actions: {
        'base': {
          interceptor: httpInterceptor
        },
        'getByUser': {
          method: 'GET',
          url: '{userid}',
          wrap: false,
          afterRequest: function (response) {
            var result = [];
            angular.forEach(response, function (value, index) {
              if (value.IsActive === true || value.IsActive === 'true')
                result.push(value.BiddingItemId);
            });
            return result;
          }
        }
      },
      instance: {
      }
    });

    return model;
  }
})();
