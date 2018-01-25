(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('NotificationAck', NotificationAck);

  NotificationAck.$inject = ['$modelFactory', 'httpInterceptor'];

  function NotificationAck($modelFactory, httpInterceptor) {
    var model = $modelFactory('api/notificationAck', {
      pk: 'Id',
      map: {
      },
      defaults: {
        UserId:null,
        LastAccessDate:null
      },
      actions: {
        'base': {
          interceptor: httpInterceptor
        },
        'dismissFrom': {
          method: 'POST',
          url: '/{UserId}'
        }
      },
      instance: {
      },
      init: init
    });

    return model;

    function init(instance) {
    }
  }
})();
