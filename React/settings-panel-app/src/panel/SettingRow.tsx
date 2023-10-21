import { Setting, SettingType } from "./SettingData";
import React from "react";

export const SettingRow = (props: { setting: Setting, onSettingChange: any }) =>{
   const {setting, onSettingChange} = props;
   let control;
   switch (setting.settingType)
   { 
      case SettingType.TextBox:
         control = <input type="text" title={setting.title} value={setting.value} key={setting.id} onChange={onSettingChange}/>
         break;
      case SettingType.CheckBox:
         control=<input type="checkbox" title={setting.title} checked={JSON.parse(setting.value)} onChange={onSettingChange}/>
         break
      case SettingType.DropDown: 
         control=<input type="text" title={setting.title} value={setting.value}/>
         break;

   }
   return (
        <>
            <h1>{setting.title}</h1>
            {control}
         </>
   ) 
}