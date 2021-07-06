import React from 'react';
import { withTranslation } from 'react-i18next';
import { Container, AppBar, Toolbar, IconButton, Typography, Button, makeStyles } from '@material-ui/core';
import { MenuOutlined } from '@material-ui/icons';
import { useHistory } from 'react-router-dom';
import * as Routes from '../../constants/route-names';
import * as Vars from '../../styles/vars';

const useStyles = makeStyles((theme) => ({
    appbarContainer: {
        
    },
    appBar: {
        background: 'transparent',
        borderBottom: `solid 1px ${Vars.COLOR_GRAY_L3}`
    },
    title: {
        flexGrow: 1
    }
}));

function Header({ t }: any) {

    // Hooks:
    const classes = useStyles();
    const history = useHistory();


    // Event Listeners:
    const onLoginClick = () => {
        history.push(Routes.LOGIN);
    };

    return (
        <div className={classes.appbarContainer}>
            <Container>
                <AppBar position="static" elevation={0} className={classes.appBar}>
                    <Toolbar>
                        <IconButton edge="start" color="inherit" aria-label="menu">
                            <MenuOutlined />
                        </IconButton>
                        <Typography variant="h6" className={classes.title} color="secondary">
                            ozTracker
                        </Typography>
                        <Button color="inherit" onClick={onLoginClick}>
                            {t('login')}
                        </Button>
                    </Toolbar>
                </AppBar>
            </Container>
        </div>
    )
}

export default withTranslation()(Header);