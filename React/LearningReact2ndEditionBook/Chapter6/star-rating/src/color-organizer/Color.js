import { FaTrash } from "react-icons/fa";
import StarRating from "./StarRating";

export default function Color({
    id,
    title,
    color,
    rating,
    onRemove= f=> f,
    onRate = f => f
}) {
    return (
        <>
            <h1>{title}</h1>
            <button onClick={()=>onRemove(id)}>
                <FaTrash/>
            </button>
            <div style={{ height: 50, backgroundColor: color }} />
            <StarRating
                selectedStars={rating}
                onRate={rating=>onRate(id, rating)}
                />
        </>
    )
}