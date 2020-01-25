import { Tag } from "./tag.model";
import { Type } from "./type.model";

export interface Item {
    link: string;
    state: string;
    editable: boolean;
    type: Type;
    name: string;
    label: string;
    category: string;
    tags: Tag[];
    metadata?: { [key: string]: any; };
    groupNames: string[];
    transformedState?: string;
    members?: any[];
    groupType?: Type;
}


