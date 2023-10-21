import React from "react";
import {SettingRow} from "./Setting";
import { acmeSettings } from "./SettingData";

export default function SettingsPanel () {
    return <SettingRow setting={acmeSettings[0]}/>
}