import React, { useState } from 'react';
import { withTranslation } from 'react-i18next';
import clsx from 'clsx';
import { Container, AppBar, Toolbar, IconButton, Typography, Button, Icon, Link, TextField, Drawer, Divider, List, ListItem, ListItemIcon, ListItemText } from '@material-ui/core';
import { makeStyles, useTheme } from '@material-ui/core/styles';
import { MenuOutlined, ChevronLeft as ChevronLeftIcon, ChevronRight as ChevronRightIcon, Mail as MailIcon, Inbox as InboxIcon } from '@material-ui/icons';
import { useHistory } from 'react-router-dom';
import * as Routes from '../../constants/route-names';
import * as Vars from '../../styles/vars';

const drawerWidth = 240;

const useStyles = makeStyles((theme) => ({
    appbarContainer: {

    },
    appBar: {
        background: 'transparent',
        paddingTop: theme.spacing(1),
        paddingBottom: theme.spacing(1),
        transition: theme.transitions.create(['margin', 'width'], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    },
    title: {
        flexGrow: 1
    },
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
    },
    drawerPaper: {

    },
    drawerHeader: {

    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    hide: {
        display: 'none',
    },
}));

function Header({ t }: any) {

    // Hooks:
    const classes = useStyles();
    const history = useHistory();
    const theme = useTheme();
    const [open, setOpen] = useState(false);


    // Event Listeners:
    const onLoginClick = () => {
        history.push(Routes.LOGIN);
    };

    const handleDrawerOpen = () => {
        setOpen(true);
    };

    const handleDrawerClose = () => {
        setOpen(false);
    };

    return (
        <div className={classes.appbarContainer}>
            <Container>
                <AppBar
                    position="static"
                    elevation={0}
                    className={classes.appBar}
                >
                    <Toolbar>
                        <IconButton
                            color="inherit"
                            aria-label="open drawer"
                            onClick={handleDrawerOpen}
                            edge="start"
                            className={classes.menuButton}
                        >

                            <MenuOutlined color="primary" />

                        </IconButton>

                        <div className="header-brand"></div>

                        <Button
                            color="primary"
                            onClick={onLoginClick}
                            startIcon={<Icon>login</Icon>}>
                            {t('login')}
                        </Button>
                    </Toolbar>
                </AppBar>

                <Drawer
                    className={classes.drawer}
                    variant="persistent"
                    anchor="left"
                    open={open}
                    classes={{
                        paper: classes.drawerPaper,
                    }}
                >
                    <div className={classes.drawerHeader}>
                        <IconButton onClick={handleDrawerClose}>
                            {theme.direction === 'ltr' ? <ChevronLeftIcon /> : <ChevronRightIcon />}
                        </IconButton>
                    </div>
                    <Divider />
                    <List>
                        {['Inbox', 'Starred', 'Send email', 'Drafts'].map((text, index) => (
                            <ListItem button key={text}>
                                <ListItemIcon>{index % 2 === 0 ? <InboxIcon /> : <MailIcon />}</ListItemIcon>
                                <ListItemText primary={text} />
                            </ListItem>
                        ))}
                    </List>
                    <Divider />
                    <List>
                        {['All mail', 'Trash', 'Spam'].map((text, index) => (
                            <ListItem button key={text}>
                                <ListItemIcon>{index % 2 === 0 ? <InboxIcon /> : <MailIcon />}</ListItemIcon>
                                <ListItemText primary={text} />
                            </ListItem>
                        ))}
                    </List>
                </Drawer>
            </Container>
        </div>
    )
}

export default withTranslation()(Header);