import React from 'react';
import { AppBar, Container, Toolbar, Typography, makeStyles, IconButton, Button } from '@material-ui/core';
import { MenuOutlined } from '@material-ui/icons';
import { useHistory } from 'react-router-dom';
import * as Routes from '../../constants/route-names';
import * as Vars from '../../styles/vars';
import { withTranslation } from 'react-i18next';
import Header from '../../components/Header';

const useStyles = makeStyles((theme) => ({
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

function MainLayout(props: any) {

    const {
        t, children
    } = props;

    // Hooks:
    const history = useHistory();
    const classes = useStyles();

    return (
        <>
            <Header />
            <Container className={classes.content}>
                <div>
                    {children}
                </div>
                <div className={classes.footer}>
                    <Typography variant="body2">
                        {t('footer')}
                    </Typography>
                </div>
            </Container>
        </>
    )
}

export default withTranslation()(MainLayout);