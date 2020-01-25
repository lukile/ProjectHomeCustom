import { StringifyOptions, stringify } from 'query-string';
import { Item } from './models/openhab/item.model';
import { IRestResponse, RestClient } from 'typed-rest-client/RestClient';
import {HttpClient, HttpClientResponse} from 'typed-rest-client/HttpClient';
import { IRequestOptions, IHttpClientResponse } from 'typed-rest-client/Interfaces';
import { Device } from "./models/device.model";
import { Room } from "./models/room.model";
import { Home } from "./models/home.model";

export class OpenhabClient {
    private apiBaseUrl: string;
    private restClient: RestClient;
    private httpClient: HttpClient;
    private stringifyOptions: StringifyOptions = {
        arrayFormat: 'comma',
        skipNull: true
    };
    private options: IRequestOptions;

    constructor(baseUrl: string, authorizationToken: string) {
        this.apiBaseUrl = baseUrl;
        this.options = {
            ignoreSslError: true,
            headers: {
                'Authorization': authorizationToken,
                'Content-Type': 'text/plain'
            }
        };
        this.restClient = new RestClient('ts', null, null, this.options);
        this.httpClient = new HttpClient('ts', null, this.options)
    }

    async getHomeConfiguration(): Promise<Home> {
        let requestEndpoint = `/api/Configuration`;
        let response: IRestResponse<Home> = await this.restClient.get<Home>(`${this.apiBaseUrl}${requestEndpoint}`);
        return response.result;
    }

    async getAllDevices(): Promise<Device[]> {
        let requestEndpoint = `/api/home/devices`;
        let response: IRestResponse<Device[]> = await this.restClient.get<Device[]>(`${this.apiBaseUrl}${requestEndpoint}`);
        return response.result;
    }

    async getAllRooms(): Promise<Room[]> {
        let requestEndpoint = `/api/home/rooms`;
        let response: IRestResponse<Room[]> = await this.restClient.get<Room[]>(`${this.apiBaseUrl}${requestEndpoint}`);
        return response.result;
    }

    async getAllItems(args?: { type?: string, tags?: string[], metadata?: string[], fields?: string[], recursive?: boolean }): Promise<Item[]> {
        let requestEndpoint = `/api/items?`;
        if (args) {
            const query = stringify({
                type: args.type,
                tags: args.tags,
                metadata: args.metadata,
                fields: args.fields,
                recursive: args.recursive
            }, this.stringifyOptions);
            requestEndpoint += query;
        }

        let response: IRestResponse<Item[]> = await this.restClient.get<Item[]>(`${this.apiBaseUrl}${requestEndpoint}`);
        return response.result;
    }

    async getItem(name: string, metadata?: string[]): Promise<Item> {
        const query = stringify({
            metadata: metadata,
        }, this.stringifyOptions);
        let requestEndpoint = `/api/items/${name}?${query}`;
        let response: IRestResponse<Item> = await this.restClient.get<Item>(`${this.apiBaseUrl}${requestEndpoint}`);
        return response.result;
    }

    async getItemState(name: string): Promise<any> {
        let requestEndpoint = `api/items/${name}/state`;
        let response: IRestResponse<any> = await this.restClient.get<any>(`${this.apiBaseUrl}${requestEndpoint}`);
        return response.result;
    }

    async uodateItemState(name: string, value: any): Promise<any> {
        let requestEndpoint = `/api/items/${name}/state`;
        let response: IHttpClientResponse = await this.httpClient.put(`${this.apiBaseUrl}${requestEndpoint}`, value);
        return response;
    }

    async sendCommand(name: string, command: any): Promise<any> {
        console.log('command in oh client', command);
        let requestEndpoint = `/api/items/${name}/state`;
        let response: IHttpClientResponse = await this.httpClient.post(`${this.apiBaseUrl}${requestEndpoint}`, command.toString());
        return response;
    }
}