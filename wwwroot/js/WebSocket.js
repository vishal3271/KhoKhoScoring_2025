// var baseURLWebSocket = "ws://192.168.10.3:9070/";
// var socketHoldTasks = [];
// /* WebSocket Lib
// */
// var isConnected = false;
// var wSocket = CreateSocket();			
			
// /* WebSocket Lib
// */
// function cycleCheck() 
// {
// 	if (!isConnected)
// 	{		
// 		console.log("trying to reconnect socket...");
// 		CreateSocket();
// 	}
// }

// function cycleSocketTasksCheck() 
// {
// 	if (isConnected && socketHoldTasks && wSocket && wSocket.readyState == 1 && socketHoldTasks.length > 0){	
// 		setTimeout("cycleSocketTasksCheck()", 1000);
// 	}	
	
// 	setTimeout(function () {
// 		callExecussionTasks();
// 	}, 1000);
	
// }

// function callExecussionTasks() 
// {
// 	if (socketHoldTasks.length > 0)
// 	{	
// 		for (var t=0; t<socketHoldTasks.length; t++) 
// 		{
// 			var taskMessage = socketHoldTasks.pop();
// 			// console.log(taskMessage);
// 			wSocket.sendMessage(taskMessage);
// 		}
// 	}
// }

// function CreateSocket() 
// {
// 	// console.log("creating socket...");
// 	wSocket = new WebSocket(baseURLWebSocket);
		
// 	wSocket.onopen = function(evt)
// 	{
// 		isConnected = true;
// 		cycleSocketTasksCheck();
// 		// console.log("INFO: WebSocket CONNECTED");
// 	};

// 	wSocket.onclose = function(evt)
// 	{
// 		isConnected = false;
// 		// console.log("WebSocket DISCONNECTED");
// 		wSocket.close();
// 		setTimeout("cycleCheck()", 5000);
// 	};

// 	wSocket.onmessage = function(evt)
// 	{
// 		// console.log("SOCKET MESSAGE: " + evt.data);
		
// 		var isValidJson = false;
// 		try {
// 			isValidJson = $.parseJSON(evt.data);
// 			// console.log("Data isValidJson::");
// 			// console.log(isValidJson);
			
// 			} catch (err) {
// 			console.log("ERROR - Json invalid Format on Socket Message. " + err.message);
// 			console.log("Data: " + evt.data);
// 		}
// 		if (isValidJson) {
// 			appSocketMessageParser(isValidJson);
// 		}
// 	};
	
// 	wSocket.sendMessage = function(msg)
// 	{
// 		if ( msg && isConnected == false ) 
// 		{
// 			socketHoldTasks.push(msg);
// 			return;
// 		}
// 		wSocket.send(msg);
// 	};

// 	wSocket.onerror = function(evt)
// 	{
// 		if (isConnected) {
// 			// console.log("WebSocket on error!");
// 		}
// 	};
// 	return wSocket;
// }

// function appSocketMessageParser(jsonMessage) 
// {
//   if ( jsonMessage.hasOwnProperty("callfunction") )
//   {
// 	try {
// 		// console.log("APLY ON - function: " + jsonMessage['callfunction']);
// 		window[jsonMessage['callfunction']].apply(window, [jsonMessage['data']]);
// 	} catch (err) {
		
// 	  console.log("ERROR - exec invalid function. " + err.message);
// 	}
//   }
//   else{ onSocketMessageRecieved(jsonMessage);
//   }
// }















// // var ws = new WebSocket("wss://192.168.10.3:9070/");

// // ws.onopen = function () {
// //     console.log("Connected to WebSocket server");
// //     ws.send(JSON.stringify({ message: "Hello from the new app!" }));
// // };

// // ws.onmessage = function (event) {
// //     console.log("Received message:", event.data);
// // };

// // ws.onclose = function () {
// //     console.log("WebSocket connection closed");
// // };

// // ws.onerror = function (error) {
// //     console.log("WebSocket Error:", error);
// // };
