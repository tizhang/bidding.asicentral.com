(function () {
  'use strict';

  angular
    .module('bidding.models')
    .factory('BiddingSettings', BiddingSettings);

  BiddingSettings.$inject = ['$simpleModelFactory'];

  function BiddingSettings($simpleModelFactory) {
    var model = $simpleModelFactory({
      pk: 'Id',
      defualts: {
        ItemId: null,
        BidderGroups: [],
        CanSeeOwner: false,
        CanSeeCurrentPrice: false,
        Increment: 1.00,
        StartPrice: 0.00,
        StartDate: null,
        EndDate: null,
        BidtimePerUser: null,
        AcceptMinimumPrice: 0.00
      },
      init: init
    });

    return model;

    function init(instance) {

    }
  }
})();
