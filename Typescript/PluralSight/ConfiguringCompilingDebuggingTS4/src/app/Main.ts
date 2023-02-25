import { TicketSchema } from "../interfaces";
import {emphasizeString} from "../../lib/language.js";

export class Main {
    render() : string {
        return `
            <div>
                <h2>${emphasizeString('Sumanesh', 10)}
            </div>`;
    }
}