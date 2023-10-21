import React from "react";
import Color from "./Color";


export default function ColorList ({
    colors = [],
    onRemoveColor = f=>f,
    onRateColor= f=>f
}) {
    if (!colors.length) return <div>No colors listed</div>;
    return (
        <div>
            {
                colors.map(c=>
                    <Color key={c.id}
                    {...c}
                    onRemove={onRemoveColor}
                    onRate={onRateColor}/>
                    )
            }
        </div>
    )
}