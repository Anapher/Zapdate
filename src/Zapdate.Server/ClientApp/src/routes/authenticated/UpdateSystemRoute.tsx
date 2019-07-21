import {
   AppBar,
   Button,
   Divider,
   Drawer,
   Hidden,
   IconButton,
   List,
   ListItem,
   ListItemIcon,
   ListItemText,
   makeStyles,
   Theme,
   Toolbar,
   Typography,
} from '@material-ui/core';
import ArrowBackIcon from '@material-ui/icons/ArrowBack';
import FileCopyIcon from '@material-ui/icons/FileCopy';
import MailIcon from '@material-ui/icons/Mail';
import MenuIcon from '@material-ui/icons/Menu';
import SettingsIcon from '@material-ui/icons/Settings';
import InboxIcon from '@material-ui/icons/MoveToInbox';
import { useTheme } from '@material-ui/styles';
import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps, Route, Redirect } from 'react-router';
import LoadingPlaceholder from 'src/components/LoadingPlaceholder';
import * as actions from 'src/features/update-system/actions';
import { getSelectedProject } from 'src/features/update-system/selectors';
import to from 'src/utils/to';
import { RootState } from 'zapdate.server';
import { Switch } from 'react-router-dom';
import UpdatePackagesRoute from './update-system/UpdatePackagesRoute';
import SettingsRoute from './update-system/SettingsRoute';

const drawerWidth = 240;

const useStyles = makeStyles(theme => ({
   root: {
      display: 'flex',
      height: '100%',
   },
   drawer: {
      [theme.breakpoints.up('sm')]: {
         width: drawerWidth,
         flexShrink: 0,
      },
   },
   appBar: {
      marginLeft: drawerWidth,
      [theme.breakpoints.up('sm')]: {
         width: `calc(100% - ${drawerWidth}px)`,
      },
   },
   menuButton: {
      marginRight: theme.spacing(2),
      [theme.breakpoints.up('sm')]: {
         display: 'none',
      },
   },
   toolbar: {
      ...theme.mixins.toolbar,
      display: 'flex',
      alignItems: 'center',
   },
   drawerPaper: {
      width: drawerWidth,
   },
   content: {
      flexGrow: 1,
      height: '100%',
      flex: '1 1 auto',
      display: 'flex',
      flexDirection: 'column',
   },
   route: {
      flex: 1,
      overflowY: 'auto',
   },
}));

interface MenuItem {
   label: string;
   route: string;
   icon: React.ReactElement;
   component: React.ComponentType<any>;
}

interface MenuSection {
   items: MenuItem[];
}

const menu: MenuSection[] = [
   {
      items: [
         {
            label: 'Update Packages',
            route: '/updates',
            icon: <FileCopyIcon />,
            component: UpdatePackagesRoute,
         },
      ],
   },
   {
      items: [
         {
            label: 'Settings',
            route: '/settings',
            icon: <SettingsIcon />,
            component: SettingsRoute,
         },
      ],
   },
];

const flatMenu = menu.flatMap(x => x.items);

function getUrl(item: MenuItem, projectId: any) {
   return `/projects/${projectId}${item.route}`;
}

type MatchParams = { id?: string };
const dispatchProps = {
   selectProject: actions.selectProjectAsync.request,
};

const mapStateToProps = (state: RootState) => ({
   project: getSelectedProject(state),
});

type Props = RouteComponentProps<MatchParams> &
   typeof dispatchProps &
   ReturnType<typeof mapStateToProps>;

function UpdateSystemRoute({
   match: {
      params: { id },
   },
   location,
   selectProject,
   project,
}: Props) {
   const [mobileOpen, setMobileOpen] = useState(false);
   const classes = useStyles();
   const theme = useTheme<Theme>();

   const projectId = Number(id) || null;
   useEffect(() => {
      selectProject(projectId);
   }, [projectId, selectProject]);

   function handleDrawerToggle() {
      setMobileOpen(!mobileOpen);
   }

   if (project === null) {
      return <LoadingPlaceholder label="Loading project..." />;
   }

   const drawer = (
      <div>
         <div className={classes.toolbar}>
            <Button style={{ marginLeft: 8 }} {...to('/')}>
               <ArrowBackIcon style={{ marginRight: 16 }} />
               Go Back
            </Button>
         </div>
         <Divider />
         {menu.map((x, i) => (
            <React.Fragment key={i}>
               <List>
                  {x.items.map(item => (
                     <ListItem
                        button
                        key={item.route}
                        {...to(getUrl(item, id))}
                        selected={location.pathname.startsWith(getUrl(item, id))}
                     >
                        <ListItemIcon>{item.icon}</ListItemIcon>
                        <ListItemText primary={item.label} />
                     </ListItem>
                  ))}
               </List>
               {i !== menu.length - 1 && <Divider />}
            </React.Fragment>
         ))}
      </div>
   );

   return (
      <div className={classes.root}>
         <AppBar position="fixed" className={classes.appBar}>
            <Toolbar>
               <IconButton
                  color="inherit"
                  aria-label="Open drawer"
                  edge="start"
                  onClick={handleDrawerToggle}
                  className={classes.menuButton}
               >
                  <MenuIcon />
               </IconButton>
               <Typography variant="h6" noWrap>
                  {project && project.name}
               </Typography>
            </Toolbar>
         </AppBar>
         <nav className={classes.drawer} aria-label="Mailbox folders">
            {/* The implementation can be swapped with js to avoid SEO duplication of links. */}
            <Hidden smUp implementation="css">
               <Drawer
                  variant="temporary"
                  anchor={theme.direction === 'rtl' ? 'right' : 'left'}
                  open={mobileOpen}
                  onClose={handleDrawerToggle}
                  classes={{
                     paper: classes.drawerPaper,
                  }}
                  ModalProps={{
                     keepMounted: true, // Better open performance on mobile.
                  }}
               >
                  {drawer}
               </Drawer>
            </Hidden>
            <Hidden xsDown implementation="css">
               <Drawer
                  classes={{
                     paper: classes.drawerPaper,
                  }}
                  variant="permanent"
                  open
               >
                  {drawer}
               </Drawer>
            </Hidden>
         </nav>
         <main className={classes.content}>
            <div className={classes.toolbar} />
            <div className={classes.route}>
               <Switch>
                  {flatMenu.map(x => (
                     <Route key={x.route} component={x.component} path={getUrl(x, id)} />
                  ))}
                  <Route path="/" render={() => <Redirect to={getUrl(flatMenu[0], id)} />} />
               </Switch>
            </div>
         </main>
      </div>
   );
}

export default connect(
   mapStateToProps,
   dispatchProps,
)(UpdateSystemRoute);
