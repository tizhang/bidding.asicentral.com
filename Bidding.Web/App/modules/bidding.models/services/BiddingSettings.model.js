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
        ShowOwner: false,
        ShowCurrentPrice: false,
        Increment: 1.00,
        StartPrice: 0.00,
        StartDate: null,
        EndDate: null,
        BidtimePerUser: null,
        AcceptMinimumPrice: 0.00
      },
      init: init,
      instance: {
        beforeRequestCleanup: beforeRequestCleanup
      }
    });

    return model;

    function init(instance) {

    }

    function beforeRequestCleanup() {
      if (this.StartDate)
        this.StartDate = new Date(Date.parse(this.StartDate));

      if (this.EndDate)
        this.EndDate = new Date(Date.parse(this.EndDate));
    }
  }
})();
