import { Stats } from "./stats";

export interface Post {
    id: number;
    title: string;
    body: string;
    url: string;
    stats?: Stats;
}