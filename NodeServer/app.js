const dgram = require('dgram');
const { join } = require('path');
const { send } = require('process');
const server = dgram.createSocket('udp4');
const validCommands = ['u', 'newClient', 'universalEvent', 'soloEvent'];
currentID = 0;

const maxChecksBeforeDisconnect = 3; //this times diconnect interval is how long it takes (in ms) for a player to get disconnected
const disconnectInterval = 1000; //in ms
setInterval(checkDisconnectTimers, disconnectInterval);

playerTransformInfo = []; //position and rotation
playerInfo = []; //usernames, might be more later
currentPlayerIDs = []; //IDs (to find where other information is without having to do larger calculations)
playerDisconnectTimers = []; //disconnect timers that go up untill they are disconnected because of not updating their transform
eventsToSend = []; //events that que up untill the client calls an update, where they are dumped and the client then processes them

server.on('error', (err) => {
	console.log(`server error:\n${err.stack}`);
	server.close();
});

server.on('listening', () => {
	const address = server.address();
	console.log(`server listening on ${address.address}:${address.port}`);
});

server.on('message', (msg, senderInfo) => {
	msg = msg + "";
	try {
		if(validCommands.includes(msg.split("~")[0])){
			eval(msg.split("~")[0] + "(\"" + msg + "\", " + senderInfo.port + ", \"" + senderInfo.address + "\")");
		}
		else{
			logSenderInfo(msg, senderInfo);
			console.warn("Unknown command (prob from a dumb bot, but maybe not)");
			console.log("---------------------");
		}
	} catch (error) {
		logSenderInfo(msg, senderInfo);
		console.error(error);
		console.log("---------------------");
	}
});


//Server functions -----------------------------------------------------------------------------
function checkDisconnectTimers(){
	/*console.log("___________________________________________")
	console.log("Transforms: ");
	console.log(playerTransformInfo);
	console.log("Current player IDs: ");
	console.log(currentPlayerIDs);
	console.log("Player disconnect timers: ");       //this basically debugs everything in one second intervals
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

function addEventToAll(eventString){
	for(eventsToSendID in eventsToSend){
		eventsToSend[eventsToSendID] += eventString + "|";
	}
}

function logSenderInfo(msg, senderInfo){
	console.log("---------------------");
	console.log("Date/Time: " + Date());
	console.log("Message: " + msg);
	console.log("Port: " + senderInfo.port);
	console.log("Address: " + senderInfo.address);
}


//Client functions -----------------------------------------------------------------------------
function newClient(info, senderPort, senderAddress){
	server.send(currentID + "", senderPort, senderAddress);
	splitInfo = info.split("~");

	addEventToAll("newClient~" + currentID + "~" + splitInfo[1]);
	console.log("---------------------");
	console.log("Date/Time: " + Date());
	console.log("New client: ID = " + currentID + ", Username = " + splitInfo[1]);
	console.log("---------------------");
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

function universalEvent(info, senderPort, senderAddress){
	splitInfo = info.split("~");
	newEvent = (splitInfo.slice(1, splitInfo.length) + "").replaceAll(",", "~");
	addEventToAll(newEvent);
}

function u(info, senderPort, senderAddress){
	splitInfo = info.split("~")
	//console.log("Player with ID " + splitInfo[1] + " updated");
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

server.bind(4000);