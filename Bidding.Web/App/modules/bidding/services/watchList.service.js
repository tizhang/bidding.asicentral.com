(function () {
  'use strict';

  angular
    .module('bidding')
    .service('myWatchList', myWatchList);

  myWatchList.$inject = ['Watcher'];

  function myWatchList(Watcher) {
    var myWatchlist = null;
    var userId = 0;
    return {
      iAm: iAm,
      isWatching: isWatching,
      watch: watch,
      unwatch: unwatch,
      list: myWatchlist
    };
    function iAm(userid,callback) {
      if (userid && userId != userid) {
        userId = userid;
        Watcher.getByUser({ userid: userId })
          .then(
          function (resp) {
            myWatchlist = resp;
            if(callback)
              callback();
          },
          function () {
            console.log("Error in myWatchList");
          });
      }
    }
    function isWatching(itemId) {
      return myWatchlist.includes(itemId);
    }

    function watch(itemId,callback) {
      if (!myWatchlist.includes(itemId)) {
        var watcher = new Watcher({ BiddingItemId: itemId, UserId: userId, IsActive: true });
        watcher.$save()
          .then(
          function () {
            myWatchlist.push(itemId);
            if (callback)
              callback(true);
          },
          function () {
            console.log("Error in myWatchList");
            if (callback)
              callback(false);
          });
      } else {
        if (callback)
          callback(true);
      }
    }

    function unwatch(itemId, callback) {
      var index = myWatchlist.indexOf(itemId);
      if (index >= 0) {
        var watcher = new Watcher({ BiddingItemId: itemId, UserId: userId, IsActive: false });
        watcher.$save()
          .then(
          function () {
            myWatchlist.splice(index, 1);
            if (callback)
              callback(false);
          },
          function () {
            console.log("Error in myWatchList");
            if (callback)
              callback(true);
          });
      } else {
        if (callback)
          callback(false);
      }
    }
  }
})();
