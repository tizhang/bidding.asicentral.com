(function () {
  'use strict';

  angular
    .module('bidding')
    .service('myWatchList', myWatchList);

  myWatchList.$inject = ['Watcher'];

  function myWatchList(Watcher) {
    var myWatchlist = [];
    var userId = 0;
    var loaded = false;
    return {
      iAm: iAm,
      isWatching: isWatching,
      watch: watch,
      unwatch: unwatch,
      list: myWatchlist
    };
    function iAm(userid, callback) {
      if (!userid)
        return;
      if (userId != userid) {
        userId = userid;
        Watcher.getByUser({ userid: userId })
          .then(
          function (resp) {
            myWatchlist = resp;
            loaded = true;
            if (callback)
              callback();
          },
          function () {
            console.log("Error in myWatchList");
          });
      } else if (loaded && callback){
          callback();
      }
    }
    function isWatching(itemId) {
      return myWatchlist && myWatchlist.length && myWatchlist.includes(itemId);
    }

    function watch(itemId, callback) {
      if (!isWatching(itemId)) {
        var watcher = new Watcher({ BiddingItemId: itemId, UserId: userId, IsActive: true });
        watcher.$save()
          .then(
          function () {
            if (!myWatchlist || !myWatchlist.length)
              myWatchlist = [itemId];
            else
              myWatchlist.push(itemId);
            if (callback)
              callback(true);
          }).catch(function () {
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
      var index = myWatchlist && myWatchlist.length ? myWatchlist.indexOf(itemId) : -1;
      if (index >= 0) {
        var watcher = new Watcher({ BiddingItemId: itemId, UserId: userId, IsActive: false });
        watcher.$save()
          .then(
          function () {
            myWatchlist.splice(index, 1);
            if (callback)
              callback(false);
          }).catch(
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
