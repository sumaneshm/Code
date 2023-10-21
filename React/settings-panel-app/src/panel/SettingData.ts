export enum SettingType{
    TextBox,
    CheckBox,
    DropDown
}

export type Setting = {
    id :string,
    title :string;
    value : string,
    settingType : SettingType,
    possibleValues? : string[],
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
        value:"true",
        settingType:SettingType.TextBox
    },
    {
        id:"contribs-mode",
        title:"Contribs Mode",
        value:"false",
        settingType:SettingType.TextBox
    },
    // {
    //     id:"styling-environment",
    //     title:"Styling Environment",
    //     value:"Production",
    //     settingType:SettingType.DropDown,
    //     possibleValues : ["Production", "UAT", "Dev"]
    // }
]

