const dgram = require('dgram');
const { join } = require('path');
const { send } = require('process');
const server = dgram.createSocket('udp4');
currentID = 0;

const maxChecksBeforeDisconnect = 3;
const disconnectInterval = 1000; //in ms
setInterval(checkDisconnectTimers, disconnectInterval);

playerTransformInfo = []; //position and rotation
playerInfo = []; //usernames, might be more later
currentPlayerIDs = []; //IDs (to find where other information is without having to do larger calculations)
playerDisconnectTimers = []; //disconnect timers that go up untill they are disconnected because of not updating their transform
eventsToSend = []; //events that que up untill the client calls an update, where they are dumped and the client then processes them

server.on('error', (err) => {
	console.log(`server error:\n${err.stack}`);
	//server.close();
});

server.on('message', (msg, senderInfo) => {
	msg = msg + "";
	eval(msg.split("~")[0] + "(\"" + msg + "\", " + senderInfo.port + ", \"" + senderInfo.address + "\")");
});

function checkDisconnectTimers(){
	/*console.log("___________________________________________")
	console.log("Transforms: ");
	console.log(playerTransformInfo);
	console.log("Current player IDs: ");
	console.log(currentPlayerIDs);
	console.log("Player disconnect timers: ");
	console.log(playerDisconnectTimers);
	console.log("Player usernames: ");
	console.log(playerInfo);
	console.log("Events to send: ");
	console.log(eventsToSend);*/

	playerIndexesToDisconnect = [];
	for(playerListID in playerDisconnectTimers){
		playerDisconnectTimers[playerListID]++;
		if(playerDisconnectTimers[playerListID] > maxChecksBeforeDisconnect){
			playerIndexesToDisconnect.push(playerListID);
		}
	}
	if(playerIndexesToDisconnect.length >= 1){
		console.log("Players to disconnect: " + playerIndexesToDisconnect);
		for(playerIndexesID in playerIndexesToDisconnect){
			playerIndex = playerIndexesToDisconnect[playerIndexesToDisconnect.length - 1 - playerIndexesID];
			addEventToAll("removeClient~" + currentPlayerIDs[playerIndex] + "|");
			delete currentPlayerIDs[playerIndex];
			delete playerDisconnectTimers[playerIndex];
			delete playerTransformInfo[playerIndex];
			delete playerInfo[playerIndex];
			delete eventsToSend[playerIndex];
		}
	}	

}

function newClient(info, senderPort, senderAddress){
	server.send(currentID + "", senderPort, senderAddress);
	splitInfo = info.split("~");

	addEventToAll("newClient~" + currentID + "~" + splitInfo[1]); //move to top when done tetsting to get rid of ghost player
	console.log("New client: " + currentID);
	//console.log(info);

	allPlayerJoinInfo = "";
	for(playerIndex in currentPlayerIDs){
		allPlayerJoinInfo += "newClient~" + currentPlayerIDs[playerIndex] + "~" + playerInfo[playerIndex] + "|";
	}
	eventsToSend.push(allPlayerJoinInfo);
	playerInfo.push(splitInfo[1]);
	playerTransformInfo.push("(0, 0, 0)~(0, 0, 0, 1)");
	playerDisconnectTimers.push(0);
	currentPlayerIDs.push(currentID);


	currentID++;
}

function addEventToAll(eventString){
	for(eventsToSendID in eventsToSend){
		eventsToSend[eventsToSendID] += eventString + "|";
	}
}

function u(info, senderPort, senderAddress){
	//sleep(500);
	splitInfo = info.split("~")
	//console.log(splitInfo[1]);
	transformsToSend = "";
	for(playerIndex in currentPlayerIDs){
		transformsToSend += "u~" + currentPlayerIDs[playerIndex] + "~" + playerTransformInfo[playerIndex] + "|"
	}
	playerIndex = currentPlayerIDs.indexOf(parseInt(splitInfo[1]));
	if(playerIndex != -1){
		server.send(eventsToSend[playerIndex] + transformsToSend, senderPort, senderAddress); //send events, and later other players new positions
		eventsToSend[playerIndex] = "";
		playerTransformInfo[playerIndex] = splitInfo[2] + "~" + splitInfo[3];
		playerDisconnectTimers[playerIndex] = 0;
	}
	else{
		console.log("ERROR: player with ID " + splitInfo[1] + " is not currently in the game but tried to update transform");
	}
}

function sleep(milliseconds) { //for debuging
	const date = Date.now();
	let currentDate = null;
	do {
		currentDate = Date.now();
	} while (currentDate - date < milliseconds);
}

server.on('listening', () => {
	const address = server.address();
	console.log(`server listening on ${address.address}:${address.port}`);
});

server.bind(4000);