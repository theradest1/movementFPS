const dgram = require('dgram');
const server = dgram.createSocket('udp4');
currentID = 0;

playerPorts = {};
playerAddresses = {};

server.on('error', (err) => {
	console.log(`server error:\n${err.stack}`);
	server.close();
});

server.on('message', (msg, senderInfo) => {
	console.log('Messages received '+ msg)
	messages = (msg + "").split("|");
	console.log(msg.split("~"))

	server.send(currentID + "",senderInfo.port,senderInfo.address,()=>{
		currentID++;
		//console.log(`Message sent to ${senderInfo.address}:${senderInfo.port}`)
	})
});

server.on('listening', () => {
	const address = server.address();
	console.log(`server listening on ${address.address}:${address.port}`);
});

server.bind(4000);