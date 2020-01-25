import { Zone } from "./zone.model";

export interface Home {
    id: string;
    uuid: string;
    token: string;
    zones: Zone[];
}
