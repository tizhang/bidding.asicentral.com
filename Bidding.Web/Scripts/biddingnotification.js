// A simple templating method for replacing placeholders enclosed in curly braces.
if (!String.prototype.supplant) {
	String.prototype.supplant = function (o) {
		return this.replace(/{([^{}]*)}/g,
			function (a, b) {
				var r = o[b];
				return typeof r === 'string' || typeof r === 'number' ? r : a;
			}
		);
	};
}

$(function () {

	var biddingApi = $.connection.biddingApi; // the generated client-side hub proxy

	function init() {
		//biddingApi.server.getAllStocks().done(function (stocks) {
		//	$stockTableBody.empty();
		//	$.each(stocks, function () {
		//		var stock = formatStock(this);
		//		$stockTableBody.append(rowTemplate.supplant(stock));
		//	});
		//});
	}

	// Add a client-side hub method that the server will call
	biddingApi.client.notify = function (message) {
		console.log(message);
		//var displayStock = formatStock(stock),
		//	$row = $(rowTemplate.supplant(displayStock));

		//$stockTableBody.find('tr[data-symbol=' + stock.Symbol + ']')
		//	.replaceWith($row);
	}

	biddingApi.client.updateBiddingItem = function (itemId) {
		console.log('update ' + itemId);
		//BiddingItem.get(itemId).then(
		//	function (item) {
		//		alert(item);
		//		//angular.element(document.getElementById('yourControllerElementID')).scope().get();
		//		//$scope.$broadcast('itemChanged', item);
		//	},
		//	function (err) {
		//		console.log(err);
		//	});
	}

	// Start the connection
	$.connection.hub.start().done(init);

});