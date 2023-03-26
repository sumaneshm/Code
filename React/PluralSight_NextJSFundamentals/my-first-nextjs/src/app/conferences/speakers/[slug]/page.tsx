import {speakerJson} from "../page";

import styles from "../../conference.module.css";
function fetchSpeakerInfo(params:{slug:string}){
    const speakerInfo = speakerJson().speakers.find((speaker: {name:string})=> speaker.name === decodeURIComponent(params.slug) )
    return speakerInfo;
}
export default async function Page ({params}:{params:any}) {
    const speakerInfo = fetchSpeakerInfo(params);
    const { name, bio, sessions } : {name:string, bio:string, sessions:any} = speakerInfo;

    return (
      <div className={styles.infoContainer}>
        <h3 className={styles.titleText}>{name}</h3>
        <h5 className={styles.descText}>About: {bio}</h5>
        {sessions &&
          sessions.map(({ name, id }: {name:string, id:string}) => (
            <div key={id}>
              <h5 className={styles.descText}>Session: {name}</h5>
            </div>
          ))}
      </div>
    )
}

// export default async function Page ({params}: {params:any}) {
//   console.log('Logging...')
//   console.log(speakerJson().speakers.find((speaker:{name:string})=>speaker.name=='Macey Duncan'))
//   return (
//     <h4>This is a test : {decodeURIComponent(params.slug)}</h4>
//   )
// }