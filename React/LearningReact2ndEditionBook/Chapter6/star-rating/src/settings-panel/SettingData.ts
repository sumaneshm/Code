export enum SettingType{
    TextBox,
    CheckBox,
    DropDown
}

export type Setting = {
    id :String,
    title :String;
    value : String|boolean,
    settingType : SettingType,
    possibleValues? : String[]
}

export const acmeSettings : Setting[] =[
    {
        id:"tsp-path",
        title:"IVM TSP Path",
        value:"sumanesh",
        settingType: SettingType.TextBox    
    },
    {
        id:"editability-mode",
        title:"Editability Mode",
        value:true,
        settingType:SettingType.CheckBox
    },
    {
        id:"contribs-mode",
        title:"Contribs Mode",
        value:false,
        settingType:SettingType.CheckBox
    },
    {
        id:"styling-environment",
        title:"Styling Environment",
        value:"Production",
        settingType:SettingType.DropDown,
        possibleValues : ["Production", "UAT", "Dev"]
    }
]