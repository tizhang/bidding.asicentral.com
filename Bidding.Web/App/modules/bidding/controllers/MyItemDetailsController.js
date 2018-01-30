(function () {
  'use strict';

  angular
    .module('bidding')
    .controller('MyItemDetailsController', MyItemDetailsController);

  MyItemDetailsController.$inject = ['$scope', 'BiddingItem', 'model', '$uibModalInstance', 'mode', '$cookies','$filter'];

  function MyItemDetailsController($scope, BiddingItem, model, $uibModalInstance, mode, $cookies, $filter) {
    var vm = this;

    vm.applyBidGroup = false;
    vm.enableBidTimes = false;
    vm.groups = [['WESP'], ['SESP'], ['ADMG']];
    vm.mode = 'view';
    vm.model = model;
    vm.tabs = ['detail'];
    vm.tab = 'detail';

    vm.bid = bid;
    vm.cancel = cancel;
    vm.close = close;
    vm.email = email;
    vm.save = save;
    vm.stage = stage;
    vm.watch = watch;

	init();
	initChart();
    function init() {
      if (mode == 'add') {
        vm.model = new BiddingItem();
        vm.model.Setting.StartDate = new Date();
        vm.model.Setting.EndDate = new Date();
        vm.model.Owner = { Id: $cookies.get('UserID') };
      }
      if (vm.model.Status == 'DRAF')
        vm.mode = mode;
      vm.model = new BiddingItem(vm.model);
      vm.applyBidGroup = (vm.model.Setting.Groups && vm.model.Setting.Groups.length) ? true : false;
      vm.enableBidTimes = vm.model.Setting.BidTimePerUser > 0 ? true : false;
      if (vm.model.Status != 'DRAF' && vm.model.Status != 'STAG' && vm.model.History && vm.model.History.length) {
        vm.tabs.push('history');
        vm.tabs.push('statistics');
	  }
	  	
    }

	function initChart() {

		$scope.labels = ['started (' + $filter('date')(vm.model.Setting.StartDate, 'MM/dd/yy@h:mma') + ')'];
		var prices = [Number(vm.model.Setting.StartPrice)];
		angular.forEach(vm.model.History, function (value, key) {
			$scope.labels.push(value.Bidder.Name + ' (' + $filter('date')(value.ActionTime, 'MM/dd/yy@h:mma')+')');
			prices.push(Number(value.Price));
		});
		$scope.series = ['Bidding History'];

		$scope.data = [
			prices
		];
		$scope.onClick = function (points, evt) {
			console.log(points, evt);
		};
		$scope.datasetOverride = [{ yAxisID: 'y-axis-1' }];
		$scope.options = {
			scales: {
				yAxes: [
					{
						id: 'y-axis-1',
						type: 'linear',
						display: true,
						position: 'left'
					}
				]
			}
		};
	}

    function bid(model) {
      console.log('bid');
      console.log(model);
    }

    function watch(model) {
      console.log('watch');
      console.log(model);
    }

    function email(string) {
      alert(string);
    }

    function cancel() {
      $uibModalInstance.dismiss('cancel');
    }

    function close() {
      $uibModalInstance.close(vm.model);
    }

    function stage() {
      vm.model.Status = 'STAG';
      save();
    }

    function save() {
      vm.model.$save().then(
        function (resp) {
          vm.model = resp;
          if (vm.mode == 'add') {
            vm.mode == 'edit';
          } else if (vm.mode == 'edit' && vm.model.Status != 'DRAF') {
            vm.mode = 'view'
          }
          $uibModalInstance.close(vm.model);
        }).catch(
        function (err) {
          $uibModalInstance.dismiss('error');
        });

    }
  }
})();
