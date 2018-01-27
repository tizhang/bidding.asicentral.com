(function () {
  'use strict';

  angular
    .module('bidding')
    .service('heartBeat', heartBeat);

  heartBeat.$inject = ['Notification'];

  function heartBeat(Notification) {
    var registeredList = {};
    var keys = [];
    var busy = false;
    var interval = 20000;
    var work=null;
    return {
      init: init,
      register: register,
      unregister: unregister,
      isBusy: isBusy
    };


    function init(time) {
      if (time > 0) {
        if (work != null)
          clearInterval(work);
        interval = time;
        work=setInterval(heartbeat, interval);
      };
    }

    function register(key, event, handler) {
      registeredList[key] = { event: event, handler: handler };
      keys.push(key);
    }

    function unregister(event) {
      delete registeredList[key];
      keys = array.filter(function (a) { return a != key });
    }

    function isBusy() {
      return busy;
    }

    function heartbeat() {
      if (!busy) {
        busy = true;
        for (var i = 0; i < keys.length; i++) {
          var key = keys[i];
          registeredList[key].event(function (result) {
            if (result)
              registeredList[key].handler();
          });
        }
        busy = false;
      }
    }
  }
})();
