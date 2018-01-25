(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('BiddingUser', User);

  User.$inject = ['$simpleModelFactory'];

  function User($simpleModelFactory) {
    var model = $simpleModelFactory({
      pk: 'Id',
      defualts: {
        Name: '',
        Email: '',
        Groups: []
      }
    });

    return model;
  }
})();
