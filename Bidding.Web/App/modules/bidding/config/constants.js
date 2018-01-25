﻿(function () {
  'use strict';

  angular
    .module('bidding')
    .constant('modalOptions', {
      addItem: {
        templateUrl: 'App/modules/bidding/partials/MyBiddingItem.html',
        controller: 'BiddingDetailsController',
        controllerAs: 'vm',
        windowClass: 'modal-wide',
        resolve: {
          mode: function () { return 'add'; }
        }
      },
      editItem: {
        templateUrl: 'App/modules/bidding/partials/MyBiddingItem.html',
        controller: 'BiddingDetailsController',
        controllerAs: 'vm',
        windowClass: 'modal-wide',
        resolve: {
          mode: function () { return 'edit'; }
        }
      },
      viewMyItem: {
        templateUrl: 'App/modules/bidding/partials/MyBiddingItem.html',
        controller: 'BiddingDetailsController',
        controllerAs: 'vm',
        windowClass: 'modal-wide',
        resolve: {
          mode: function () { return 'view'; }
        }
      }
    });

})();