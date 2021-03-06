import express from 'express';
import * as bodyParser from 'body-parser';
import {dialogflow, actionssdk, Image, Carousel, List, BasicCard, Button, SimpleResponse} from 'actions-on-google';
import { OpenhabClient } from './openhabClient'
import { Item } from './models/openhab/item.model';
import { Device } from "./models/device.model";
import { Room } from "./models/room.model";
import {Zone} from "./models/zone.model";

const openhabClient = new OpenhabClient('https://openhabproxyapi-dev-as.azurewebsites.net', '01c9e186-c1f1-4e94-8bf5-ad86b297d9ba');

const server = express();
const assistant = dialogflow({ debug: false });

server.set('port', process.env.PORT || 8080);
server.use(bodyParser.json({ type: 'application/json' }));

assistant.intent('projecthomecustom.smarthome.device.state.check', async (conv, { room, deviceType, all }) => {
    let _device: Device;
    let _devices: Device[];
    let _room: Room;
    let _openhabItem: Item;

    if (!all) {
        await openhabClient.getAllDevices()
            .then(x => _device = x.find(d => d.room === room && d.type == deviceType));

        await openhabClient.getAllRooms()
            .then(x => _room = x.find(r => r.name === room));

        if (!_device)
            conv.close(`Sorry, couldn't find any device of type ${deviceType} in ${_room.description}`);

        await openhabClient.getItem(_device.id)
            .then(x => _openhabItem = x);
        let state = isNaN(+_openhabItem.state) ? _openhabItem.state : (Math.round(+_openhabItem.state * 100) / 100).toFixed(0);
        conv.close(`${deviceType} is ${state} in ${_room.description}`);

    } else {
        let responseMessage: string = "State : \n";

        await openhabClient.getAllDevices()
            .then(x => _devices = x.filter(d => d.type == deviceType));

        if (!_devices || _devices.length === 0) {
            conv.close(`Sorry, couldn't find any device of type ${deviceType}`);
            return;
        }

        await Promise.all(_devices.map(async _device => {
            let _openhabItem: Item;
            let itemState: string;

            await openhabClient.getItem(_device.id)
                .then(x => {
                    _openhabItem = x;
                    itemState = isNaN(+_openhabItem.state) ? _openhabItem.state : (Math.round(+_openhabItem.state * 100) / 100).toFixed(0);
                    responseMessage = responseMessage.concat(`${_device.room} => ${_openhabItem.state} \n`);
                });
        }));
        conv.ask(new SimpleResponse({
            text: responseMessage,
            speech: 'Are you talking to me ?'
        }));
        /*conv.close(new BasicCard({
            title: responseMessage,
            image: new Image({
                url: 'https://goo.gl/Fz9nrQ',
                alt: 'AngularFirebase Logo'
            })
        }));*/
    }
});

assistant.intent('projecthomecustom.smarthome.device.command', async (conv, { room, deviceType, command, value }) => {
    let _device: Device;
    let _room: Room;
    let _openhabItem: Item;

    await openhabClient.getAllDevices()
        .then(x => _device = x.find(d => d.room === room && d.type == deviceType));
    await openhabClient.getAllRooms()
        .then(x => _room = x.find(r => r.name === room));

    if (!_device)
        conv.close(`Sorry, couldn't find any device of type ${deviceType} in ${_room.description}`);

    let updatedState: string;

    await openhabClient.getItem(_device.id)
        .then(x => _openhabItem = x);
    let initialState = isNaN(+_openhabItem.state) ? _openhabItem.state : (Math.round(+_openhabItem.state * 100) / 100).toFixed(0);
    await openhabClient.sendCommand(_openhabItem.name, (command === undefined || command === "") ? value : command)
        .then(async _ => {
            await openhabClient.getItem(_device.id).then(x => updatedState = x.state);
        });

    console.log("command:", command);
    if (conv.surface.capabilities.has('actions.capability.SCREEN_OUTPUT') && conv.surface.capabilities.has('actions.capability.WEB_BROWSER')) {

        conv.close(new SimpleResponse({
            text: 'responseMessage',
            speech: `${deviceType} in ${_room.description} has been updated from ${initialState} to ${updatedState}`
        }));
    } else {
        conv.close(`${deviceType} in ${_room.description} has been updated`);
    }
});

assistant.intent('projecthomecustom.smarthome.zone.state.check', async(conv, { zone, deviceType, value, all }) => {
    let _devices: Device[];
    let _zone: Zone;
    let itemState: string;

    await openhabClient.getAllDevices()
        .then(x => _devices = x.filter(d => d.zone === zone && d.type == deviceType));
    await openhabClient.getAllZones()
        .then(x => _zone = x.find(r => r.name === zone));

    if (!_devices || _devices.length === 0) {
        conv.close(`Sorry, couldn't find any device of type ${deviceType} in ${_zone.description}`);
        return;
    }
    let responseMessage: string = `Zone: ${zone} for ${deviceType} \n`;

    await Promise.all(_devices.map(async _device => {
        let _openhabItem: Item;

        await openhabClient.getItem(_device.id)
            .then(x => {
                _openhabItem = x;

                itemState = isNaN(+_openhabItem.state) ? _openhabItem.state : (Math.round(+_openhabItem.state * 100) / 100).toFixed(0);
                responseMessage = responseMessage.concat(` in ${_device.room} state is ${_openhabItem.state} \n`);
            });
    }));
    conv.ask(new SimpleResponse({
        text: responseMessage,
        speech: 'Are you talking to me ?'
    }));

    /*conv.close(new BasicCard({
        title: responseMessage,
        image: new Image({
            url: 'https://goo.gl/Fz9nrQ',
            alt: 'AngularFirebase Logo'
        }),
        buttons: new Button({
            title: 'Watch',
            url: 'https://angularfirebase.com/lessons',
        }),
    }));*/
});

assistant.intent('projecthomecustom.smarthome.zone.command', async (conv, { zone, deviceType, command, value, all }) => {
    let _devices: Device[];
    let _zone: Zone;

    if (!zone && all === 'true') {
        await openhabClient.getAllDevices()
            .then(x => _devices = x.filter(d => d.type == deviceType));
        _zone = {
            id: '//Todo',
            description: 'Home',
            name: 'Home'
        };
    } else {
        await openhabClient.getAllDevices()
            .then(x => _devices = x.filter(d => d.zone === zone && d.type == deviceType));
        await openhabClient.getAllZones()
            .then(x => _zone = x.find(r => r.name === zone));
    }

    if (!_devices || _devices.length === 0) {
        conv.close(`Sorry, couldn't find any device of type ${deviceType} in ${_zone.description}`);
        return;
    }
    let responseMessage: string = `Zone: ${zone}  \n`;
    let updatedState: string;

    await Promise.all(_devices.map(async _device => {
        let _openhabItem: Item;

        await openhabClient.getItem(_device.id)
            .then(x => _openhabItem = x);
        let initialState = isNaN(+_openhabItem.state) ? _openhabItem.state : (Math.round(+_openhabItem.state * 100) / 100).toFixed(0);

        await openhabClient.sendCommand(_openhabItem.name, (command === undefined || command === "") ? value : command)
            .then(async _ => {
                await openhabClient.getItem(_device.id).then(x => updatedState = x.state);
            });
        responseMessage.concat(`=> ${deviceType} has benn updated from ${initialState} to ${updatedState}  \n`);
        responseMessage = responseMessage.concat(`=> ${deviceType} has benn updated from ${initialState} to ${updatedState}  \n`);

    }));
    conv.ask(new SimpleResponse({
        text: responseMessage,
        speech: 'Are you talking to me ?'
    }));

    /*conv.close(new BasicCard({
        title: responseMessage,
        image: new Image({
            url: 'https://goo.gl/Fz9nrQ',
            alt: 'AngularFirebase Logo'
        }),
        buttons: new Button({
            title: 'Watch',
            url: 'https://angularfirebase.com/lessons',
        }),
    }));*/
});

server.post('/webhook', assistant);
server.listen(server.get('port'), function () {
    console.log('Express server started on port', server.get('port'));
});
