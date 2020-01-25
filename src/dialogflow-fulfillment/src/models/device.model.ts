import { OpenhabType } from "./openhab/openhab-type.enum";

export interface Device {
    id: string;
    description: string;
    room: string;
    zone: string;
    type: string;
    openhabType: OpenhabType;
}
