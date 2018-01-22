(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('httpInterceptor', Interceptor);

  Interceptor.$inject = ['$q', '$injector'];

  function Interceptor($q, $injector) {
    return ({
      responseError: responseError
    });

    function responseError(res) {
      switch (res.status) {
        case 404:
          $injector.get('$state').transitionTo('404');
          break;
        default:
          return $q.reject(res);
      }
    }
  }
})();
