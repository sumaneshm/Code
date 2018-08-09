import React from 'react';

class ClickCounter extends React.Component {
    constructor(props) {
        super(props);
        this.state = { clicks: 0 };
    }

    render() {
        return <div onClick={() => {
            this.setState({ clicks: this.state.clicks + 1 });
        }}>
            This div has been clicked {this.state.clicks} time(s)
        </div>;
    }
}

export default ClickCounter ;