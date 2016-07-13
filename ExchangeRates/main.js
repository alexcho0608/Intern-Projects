function WebSocketTest(){

    if ("WebSocket" in window)
    {
       var ws = new WebSocket("ws://127.0.0.1:8181/");
		
       ws.onopen = function()
       {
          ws.send("Message to send");
       };
		
       ws.onmessage = function (evt) 
       {
          updateExchRate(evt.data);
       };
		
       ws.onclose = function()
       { 
       };
    }
 }
var viewModel = function(){
		this.rows = ko.observableArray();
		this.addRow = function(r) {
			this.rows.push(r);
		}
		this.arr = ko.observableArray();
		this.counter = -1;
		this.idToHistoryList = [];
		this.testArr = ko.observableArray();
		this.hotPage = ko.observable("");
		this.tableClass = ko.observable("table table-bordered");
		this.history = ko.observableArray();
		this.historyHtml = ko.observableArray();
		this.addHistory = function(h) {
			// if(this.arr[h.key] == undefined) {
			// 	this.arr[h.key] = false;
			// }
			if(this.history[h.key] == undefined) {
				this.counter++;
				this.idToHistoryList[h.key] = this.counter;
				this.historyHtml.push("");
				this.history[h.key] =[];
				this.arr.push(false);
				this.history[h.key].push(h.value);

			} else if(this.history[h.key].length < 5) {
				this.history[h.key].push(h.value);
			} else {
				this.history[h.key].shift();
				this.history[h.key].push(h.value);
			}

			this.historyHtml[this.idToHistoryList[h.key]] = "";
			var temp = '<div class="row"><div class="col-lg-4">' + h.key.substring(0,3) + '</div><div class="col-lg-4">' + h.key.substring(3,6) + '</div><div class="col-lg-4"><button data-bind="click: $root.onclick.bind($data,\''+ h.key +'\')">+</button></div></div>';
			for(let el of this.history[h.key]) {
				temp += '<div class="row" data-bind="visible: $root.arr()[\''+ h.key +'\']"><div class="col-lg-4">' + el.rate + '</div><div class="col-lg-4">' + el.date + '</div></div>';
			}

			this.historyHtml.splice(this.idToHistoryList[h.key],1,temp);
		}
		var self = this;

		this.onclick = function(key) {
			self.arr().splice(self.idToHistoryList[key],1,true);
			self.
		}
	}

 var model = new viewModel();
 function updateExchRate(stringJson){
 	var data = JSON.parse(stringJson);
 	var key = data.From + data.To;
 	var value = data.Rate;
 	var sessionItemRate = sessionStorage.getItem(key);
	var upOrDown = "neutral";

 	if (sessionItemRate) {

 		if (sessionItemRate < value) {
				upOrDown = "green";
 		}
 		else{
 			upOrDown = "red";
 		}

 		sessionStorage[key] = value;
 	} else{
 		sessionStorage.setItem(key,value);
 		upOrDown = "neutral";
 	}

	var $from = data.From;
	var $to = data.To;
	var $rate = new Number(data.Rate).toFixed(4);
	var rawDate = new Date(data.Date);
	var day = rawDate.getDay();
	var month = rawDate.getMonth();
	var hours = rawDate.getHours();
	var minutes = rawDate.getMinutes();
	var seconds = rawDate.getSeconds();
	var year = rawDate.getFullYear();

	var $date =year+'-'+ month + '-' + day + '	' + hours + ':' + minutes + ':' + seconds;
	
	var row = '<tr><td>' + $from + '</td><td>' + $to + '</td><td>' + $rate + '</td><td>' + $date + '</td></tr>';
	var hot = "";
	for( var item in sessionStorage) {
		hot += '<tr><td>' + item.substring(0,3) + '</td><td>' + item.substring(3,6) + '</td><td>' + sessionStorage[item] + '</td></tr>';
	}

	var h = {
		key: key,
		value: {
			rate: $rate,
			date: $date
		} 
	}

	model.addHistory(h);
	model.hotPage(hot);
	model.addRow(row);
}

ko.bindingHandlers['html'] = {
  //'init': function() {
  //  return { 'controlsDescendantBindings': true }; // this line prevents parse "injected binding"
  //},
  'update': function (element, valueAccessor) {
    // setHtml will unwrap the value if needed
    ko.utils.setHtml(element, valueAccessor());
  }
};

ko.applyBindings(model);


WebSocketTest();
