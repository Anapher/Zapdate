import { Fab, Input, InputAdornment, Paper, Toolbar } from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import SearchIcon from '@material-ui/icons/Search';
import { makeStyles } from '@material-ui/styles';
import React from 'react';
import to from 'src/utils/to';
import CreatePackageDialog, { createPackageRoute } from './CreatePackageDialog';

const useStyles = makeStyles({
   fab: {
      position: 'absolute',
      right: 32,
      bottom: 32,
   },
   root: {
      position: 'relative',
   },
   toolbar: {
      marginRight: 80,
   },
});

type Props = {
   projectId: number;
};
export default function PackagesActions({ projectId }: Props) {
   const classes = useStyles();

   return (
      <Paper className={classes.root} elevation={3} square>
         <Toolbar className={classes.toolbar} variant="dense">
            <Input
               fullWidth
               startAdornment={
                  <InputAdornment position="start">
                     <SearchIcon />
                  </InputAdornment>
               }
            />
         </Toolbar>
         <Fab color="primary" className={classes.fab} {...to(createPackageRoute(projectId))}>
            <AddIcon />
         </Fab>
         <CreatePackageDialog projectId={projectId} />
      </Paper>
   );
}
