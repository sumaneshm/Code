import {FaStar} from "react-icons/fa";

export default function Star({onSelect=f=>f, selected=false}) {
    return <FaStar onClick={onSelect} color={selected?"red":"black"}/>
}
   