import { Room } from "./room.model";

export interface Zone {
    id: string;
    name: string;
    description: string;
    rooms?: Room[];
}
