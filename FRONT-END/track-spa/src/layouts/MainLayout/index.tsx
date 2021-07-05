import React from 'react';
import { AppBar, Container, Toolbar, Typography, makeStyles, IconButton, Button } from '@material-ui/core';
import { MenuOutlined } from '@material-ui/icons';
import { useHistory } from 'react-router-dom';
import * as Routes from '../../constants/route-names';
import * as Vars from '../../styles/vars';

const useStyles = makeStyles((theme) => ({
    appbarContainer: {
        background: theme.palette.primary.main
    },
    title: {
        flexGrow: 1
    },
    menuButton: {

    },
    content: {
        marginTop: theme.spacing(2)
    },
    footer: {
        marginTop: theme.spacing(2),
        borderTop: `solid 1px ${Vars.COLOR_GRAY_L3}`,
        padding: theme.spacing(1),
        color: Vars.COLOR_GRAY_L1,
        textAlign: "center"
    }
}));

export default function MainLayout(props: any) {

    const {
        children
    } = props;

    // Hooks:
    const history = useHistory();
    const classes = useStyles();

    // Event Handlers:
    const onLoginClick = () => {
        history.push(Routes.LOGIN);
    };

    return (
        <>
            <div className={classes.appbarContainer}>
                <Container>
                    <AppBar position="static" elevation={0}>
                        <Toolbar>
                            <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu">
                                <MenuOutlined />
                            </IconButton>
                            <Typography variant="h6" className={classes.title} color="secondary">
                                ozTracker
                            </Typography>
                            <Button color="inherit" onClick={onLoginClick}>Login</Button>
                        </Toolbar>
                    </AppBar>
                </Container>
            </div>
            <Container className={classes.content}>
                <div>
                    {children}
                </div>
                <div className={classes.footer}>
                    <Typography variant="body2">
                        Developed by: Mohammad Sakhidel
                    </Typography>
                </div>
            </Container>
        </>
    )
}
