import styles from './Home.module.scss'

import React, { useContext } from 'react';
import * as Actions from '../../redux/actions';
import { connect } from 'react-redux';
import AppContext from '../../models/app-context';
import { Text, Button, Container, Grid, Paper, Typography } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
    },
    paper: {
        padding: theme.spacing(2),
        textAlign: 'center',
        color: theme.palette.text.primary,
    },
}));

function Home(props) {

    const classes = useStyles();

    return (
        <div className={classes.root}>
            <Container>
                <Typography>
                    <Grid container spacing={3} className={styles.container}>
                        <Grid item sm={12} xs={12} className={styles.item}>
                            <Button color="primary" variant="contained">
                                Hello Material UI
                            </Button>
                        </Grid>
                        <Grid item sm={6} xs={12} className={styles.item}>
                            <Paper className={classes.paper}>Item 2</Paper>
                        </Grid>
                        <Grid item sm={6} xs={12} className={styles.item}>
                            <Paper className={classes.paper}>Item 3</Paper>
                        </Grid>
                    </Grid>
                </Typography>
            </Container>
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