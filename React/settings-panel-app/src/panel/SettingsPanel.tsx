import {SettingRow} from "./SettingRow";
import { acmeSettings } from "./SettingData";
import { useState } from "react";


export default function SettingsPanel () {
    const [settings, setSettings] = useState(acmeSettings);
    
    const onSettingChange = (event:any) =>{
        const updatedSettings = {...settings};
        const newValue = event.target.value;
        const title = event.target.title;
        const ivmTspPath = updatedSettings[0];
        const editabilityModeSetting = updatedSettings[1];
        const contribsModeSetting = updatedSettings[2];

        if(title==="IVM TSP Path"){
             const ownServer = newValue === "sumanesh";
             ivmTspPath.value=newValue;
             editabilityModeSetting!.value=ownServer.toString();
             contribsModeSetting!.value=ownServer.toString();
         }
         else if (title === "Editability Mode") {
            const newEditabilityMode = newValue ==="true";
            editabilityModeSetting.value = newValue;
            if (!newEditabilityMode){
                contribsModeSetting.value = "false";
            }
         }
         else if (title==="Contribs Mode"){
            contribsModeSetting.value = newValue;
         }

         setSettings(updatedSettings);
    }

    return (
        <div style={{padding:"15px"}}> 
            {
                acmeSettings.map((s,i) =>  <SettingRow setting={s} onSettingChange={onSettingChange}/>)
            }
        </div>
    ) 
}