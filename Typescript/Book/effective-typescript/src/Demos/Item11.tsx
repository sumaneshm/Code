const ItemHeader : string = "Item 11 : Recognize the Limits of Excess Property Checking";

interface Room {
    numOfDoors : number;
    celingHeightFt: number;
}

const strictRoom : Room = {
    numOfDoors: 0,
    celingHeightFt: 0,
    //elephant: 'present' // ERROR : eventhough adding this to strictRoom is going to make it compatible with Room
    //  it is not allowed due to "excess property checking" 
};

const nonRoomButCompatible = {
    numOfDoors: 0,
    celingHeightFt: 0,
    elephant: 'present' 
};

// this is fine, because nonRoomButCompatible is a superset of Room
const freeRoom : Room = nonRoomButCompatible;

interface Options{
    title : string;
    darkMode? : boolean;
}

function createWindow(options: Options) {
    if (options.darkMode){
        console.log('Dark Mode on');
    }
}

export default function Item11() 
{
    createWindow({title: 'Test'});                  // OK, darkMode is defaulted to false
    createWindow({title: 'Test', darkMode:true});   // OK, darkMode explicitly set to True
    // createWindow({title: 'Test', darkmode:true});   // ERROR : most likely the intention is not represented so TS errors

    const mockProperOpt1 = {title:'Test', darkMode: true};
    createWindow(mockProperOpt1);                    // OK, obviously mockProperOpt1 is perfectly representing Options

    const mockProperOpt2 = {title:'Test'};
    createWindow(mockProperOpt2);                    // OK, still ok as darkMode is defaulted to false 

    const mockProperOpt3 = {title:'Test', darkmode: true};
    createWindow(mockProperOpt3);                    // OK , WARNING : Need to be careful as TypeScript misses this because is assertion 

    // const mockProperOpt4 : Options = {title: 'Test', darkmode: true};
        // ERROR : Now we are using declaration to specify mockPropertyOpt4 is of type "Options" so TS found the issue
    return ( 
    <>
        <h1>{ItemHeader}</h1>
        <div>
            <b>What is Excess Property Checking?</b><br/>
            Excess property checking (not really excess, but less IMHO) ensures that any object when assigned to something contains
            all (and only) the properties are being passed.
            <br/><br/><b>Pitfalls of Excess Property Checking?</b><br/>
            When you introduce an intermediate variable of any other type, it can hide the errors (see the code)
            <br/><br/><b>What to remember?</b><br/>
            Always declare anything with proper type!
        </div>
    </>
    )
}
