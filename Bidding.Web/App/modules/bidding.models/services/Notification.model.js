(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('Notification', Notification);

  Notification.$inject = ['$modelFactory', 'httpInterceptor'];

  function Notification($modelFactory, httpInterceptor) {
    var model = $modelFactory('api/notification', {
      pk: 'Id',
      map: {
      },
      defaults: {
        BiddingItemId: null,
        Message: '',
        CreateDate: null,
        EventTime: '',
        ImageUrl: 'Images/question_mark.png'
      },
      actions: {
        'base': {
          interceptor: httpInterceptor
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
