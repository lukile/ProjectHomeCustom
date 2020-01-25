import { Device } from "./device.model";

export interface Room {
    id: string;
    name: string;
    description: string;
    devices: Device[];
}
