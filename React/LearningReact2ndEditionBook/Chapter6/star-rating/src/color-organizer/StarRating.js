import { createArray } from "./lib";
import { useState } from "react";
import Star from "./Star";

export default function StarRating({style={}, totalStars=5, onRate=f=>f}) {
    const [selectedStars, setSelectedStars] = useState(2);
    return (
      <div style={{padding:"5px", ...style}}>{
        createArray(totalStars).map((n,i)=> <Star key={i} onSelect={()=>setSelectedStars(i+1)} selected={selectedStars > i} />)
      }
      <p>
        {selectedStars} out of {totalStars} 
      </p>
    </div>
    )
  }
  