(function () {
  'use strict';

  angular
    .module('bidding')
    .factory('modalFactory', ModalFactory);

  ModalFactory.$inject = ['$uibModal'];

  function ModalFactory($uibModal) {
    return {
      open: openModal
    };

    function openModal(model, options, attrs) {
      var defaults = {
        backdrop: 'static',
        resolve: {
          model: function () {
            return angular.copy(model);
          },
          attrs: function () {
            return attrs;
          }
        }
      };

      var promise = $uibModal.open(angular.merge({}, defaults, options));

      promise.save = saveModel;

      function saveModel(result, resolvedResult) {
        if (angular.isObject(result) && !angular.isArray(result)) {
          shallowCopy(result, model);
        } else {
          angular.copy(result, model);
        }

        return promise.close(resolvedResult || model);

        function shallowCopy(src, dst) {
          dst = dst || {};

          angular.forEach(dst, function (value, key) {
            if (!{}.hasOwnProperty.call(src, key) && key.charAt(0) !== '$') {
              delete dst[key];
            }
          });

          _.forEach(src, function (v, k) {
            if (k.charAt(0) !== '$') {
              if (angular.isObject(v) && !angular.isArray(v) && !(v instanceof Date) && !(v instanceof moment)) {
                dst[k] = shallowClearAndCopy(v, dst[k]);
              } else {
                dst[k] = v;
              }
            }
          });

          return dst;
        }
      }

      return promise;
    }
  }
})();
