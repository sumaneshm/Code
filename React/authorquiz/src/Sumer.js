import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import PropTypes from 'prop-types';

//ReactDOM.render(<Welcome Name="Sumanesh" />, document.getElementById("root"));

class SumComp extends React.Component {
	render() {
		return <h1>The sum is :
			{this.props.a} + {this.props.b} = {this.props.a + this.props.b}
        </h1>;
	}
}

function SumWithPropValidation (props) {
		return <h1>The sum is :
			{props.a} + {props.b} = {props.a + props.b}
		</h1>;
};

SumWithPropValidation.propTypes = {
	 	a:PropTypes.number.isRequired,
	 	b:PropTypes.number.isRequired
	 };


//ReactDOM.render(<SumComp a="1" b = "2" />, document.getElementById("root"));

//The function name or Component name doesn't matter, 
// the function/component that is 'exported' as default will be
// the one that will be used on the callers

export default SumWithPropValidation;