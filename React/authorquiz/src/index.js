import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import registerServiceWorker from './registerServiceWorker';
import ClickCounter from './ClickCounter.js'


//ReactDOM.render(<AuthorQuiz />, document.getElementById('root'));
//registerServiceWorker();

//We can call the component as something else other than what it is
//here the actual component exposed inside 'Sumer.js' is SumComp,
//however we can refer to it as 'SumerAlias'  and use it
import SumWithPropValidation from './Sumer'

ReactDOM.render(<div><div id='sumerRnD'/><div id='clickCounterRnD'/></div>,document.getElementById('root'))

ReactDOM.render(<SumWithPropValidation a="1" b="2"/>, document.getElementById('sumerRnD'));
ReactDOM.render(<ClickCounter/>,document.getElementById('clickCounterRnD'))

registerServiceWorker();