import { Setting } from "./SettingData";
import React from "react";

export const  SettingRow = ({setting}) => {
   return (
        <>
            <h1>{setting.title}</h1>
            <h2>{setting.value}</h2>
         </>
   ) 
}