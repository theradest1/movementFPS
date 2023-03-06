const dgram = require('dgram');
const { join } = require('path');
const { send, cpuUsage } = require('process');
const server = dgram.createSocket('udp4');
var osu = require('node-os-utils');
var cpu = osu.cpu;
var mem = osu.mem;
const fs = require('fs');
const validCommands = ['u', 'newClient', 'ue', 'leave', 'youOnBruv', 'skipMap']; // u = update, ue = universal event (short for conservation of bandwidth)
currentID = 0;
TPS = 128;
minTPS = 10;
maxTPS = 128;
gameClock = 300;
gameLength = 300;
mapCount = 2;
currentMap = 0;

const maxChecksBeforeDisconnect = 10; //this times diconnect interval is how long it takes (in ms) for a player to get disconnected
const disconnectInterval = 1000; //in ms
const settingCheckInterval = 60000; //in ms
setInterval(checkDisconnectTimers, disconnectInterval);
setInterval(checkSettings, settingCheckInterval);
setInterval(updateGameClock, 1000);
checkSettings();
packetCounter = 0; //will be inaccurate if disconnect interval is different than 1000
settings = "";

playerTransformInfo = []; //position and rotation
playerInfo = []; //usernames, might be more later
currentPlayerIDs = []; //IDs (to find where other information is without having to do larger calculations)
playerDisconnectTimers = []; //disconnect timers that go up untill they are disconnected because of not updating their transform
eventsToSend = []; //events that que up untill the client calls an update, where they are dumped and the client then processes them

function updateGameClock(){
	gameClock --;
	if(gameClock <= 0){
		currentMap = getRandomInt(mapCount);
		addEventToAll("newMap~" + currentMap);
		addEventToAll("setClock~" + gameLength);
		gameClock = gameLength;
	}
}

function checkSettings(){
	fs.readFile('quickSettings.txt', (err, data) => {
		var dataString = data.toString();
		dataString = dataString.replace(/(\r\n|\n|\r)/gm, "");
		if(dataString != settings){
			console.log("Updated settings");
			addEventToAll("updateSettings~" + dataString);
			settings = dataString;
		}
	})
}

server.on('error', (err) => {
	console.log(`server error:\n${err.stack}`);
	server.close();
});

server.on('listening', () => {
	const address = server.address();
	console.log(`server listening on ${address.address}:${address.port}`);
});

server.on('message', (msg, senderInfo) => {
	packetCounter++;
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
	var startingToLag = false;
	var increasingTPS = false;

	cpu.free().then(cpuFree => {
		mem.free().then(memInfo => {
			memFree = memInfo.freeMemMb/memInfo.totalMemMb
			if((memFree < .2 || cpuFree < 20) && TPS > minTPS){
				TPS--;
				console.log("Decreased TPS: " + TPS);
				addEventToAll("tps~" + TPS);
			}
			else if((memFree > .4 || cpuFree > 40) && TPS < maxTPS){
				TPS++;
				console.log("Increased TPS: " + TPS);
				addEventToAll("tps~" + TPS);
			}
		});
	});
	//console.log("PPS: " + packetCounter);
	//packetCounter = 0;
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
			disconnectClient(playerIndexesToDisconnect[playerIndexesToDisconnect.length - 1 - playerIndexesID]);
		}
	}	
}

function leave(info, senderPort, senderAddress){
	disconnectClient(currentPlayerIDs.indexOf(parseInt(info.split("~")[1])));
	console.log("Player with ID " + info.split("~")[1] + " has left the game");
}

function disconnectClient(playerIndex){
	addEventToAll("removeClient~" + currentPlayerIDs[playerIndex] + "|");
	delete currentPlayerIDs[playerIndex];
	delete playerDisconnectTimers[playerIndex];
	delete playerTransformInfo[playerIndex];
	delete playerInfo[playerIndex];
	delete eventsToSend[playerIndex];
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
function skipMap(info, senderPort, senderAddress){
	gameClock = 1;
}

function youOnBruv(info, senderPort, senderAddress){
	server.send("ong", senderPort, senderAddress);
}

function newClient(info, senderPort, senderAddress){
	console.log("Quick set data: " + settings);
	console.log(currentID + "|" + settings);
	console.log(currentID);
	server.send(currentID + "|" + settings, senderPort, senderAddress);

	splitInfo = info.split("~");

	addEventToAll("newClient~" + currentID + "~" + splitInfo[1]);
	console.log("---------------------");
	console.log("Date/Time: " + Date());
	console.log("New client: ID = " + currentID + ", Username = " + splitInfo[1]);
	console.log("---------------------");

	allPlayerJoinInfo = "";
	for(playerIndex in currentPlayerIDs){
		allPlayerJoinInfo += "newClient~" + currentPlayerIDs[playerIndex] + "~" + playerInfo[playerIndex] + "|";
	}
	eventsToSend.push(allPlayerJoinInfo);
	playerInfo.push(splitInfo[1]);
	playerTransformInfo.push("(0, 0, 0)~(0, 0, 0, 1)");
	playerDisconnectTimers.push(0);
	currentPlayerIDs.push(currentID);

	eventsToSend[currentPlayerIDs.indexOf(parseInt(currentID))] += "tps~" + TPS + "|setClock~" + gameClock + "|newMap~" + currentMap + "|";

	currentID++;
}

function ue(info, senderPort, senderAddress){
	splitInfo = info.split("~");
	newEvent = info.substring(splitInfo[0].length + 1, info.length);
	//console.log(newEvent);
	addEventToAll(newEvent);
}

async function u(info, senderPort, senderAddress){
	splitInfo = info.split("~")
	//console.log("Player with ID " + splitInfo[1] + " updated");
	transformsToSend = "";
	for(playerIndex in currentPlayerIDs){
		if(currentPlayerIDs[playerIndex] != splitInfo[1]){
			transformsToSend += "u~" + currentPlayerIDs[playerIndex] + "~" + playerTransformInfo[playerIndex] + "|"
		}
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


// tools
function sleep(milliseconds) {
	const date = Date.now();
	let currentDate = null;
	do {
		currentDate = Date.now();
	} while (currentDate - date < milliseconds);
}
function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}

server.bind(4000);
