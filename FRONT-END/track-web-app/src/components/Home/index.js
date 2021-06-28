import React, { useContext } from 'react';
import * as Actions from '../../redux/actions';
import { connect } from 'react-redux';
import AppContext from '../../models/app-context';

function Home(props) {

    const appContext = useContext(AppContext);

    return (
        <div>
            <h1>{props.counter}</h1>

            <button onClick={() => props.inc()}>INC</button>
            <button onClick={() => props.dec()}>DEC</button>



            <h2>
                {appContext.theme}
            </h2>
            <button onClick={() => appContext.setTheme('blue')}>Change Theme</button>
        </div>
    );
}

const mapStateToProps = (state) => ({
    counter: state.counter
});

const mapDispatchToProps = (dispatch) => ({
    inc: () => dispatch(Actions.getInc(10)),
    dec: () => dispatch(Actions.getDec(5))
});

export default connect(mapStateToProps, mapDispatchToProps)(Home);