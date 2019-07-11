import { Box } from '@material-ui/core';
import { makeStyles } from '@material-ui/styles';
import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import * as actions from 'src/features/update-packages/actions';
import PackagesActions from 'src/features/update-packages/components/PackagesActions';
import PackagesList from 'src/features/update-packages/components/PackagesList';
import * as selectors from 'src/features/update-packages/selectors';
import * as updateSystemSelectors from 'src/features/update-system/selectors';
import { RootState } from 'zapdate';

const useStyles = makeStyles({
   root: {
      height: '100%',
      display: 'flex',
      flexDirection: 'row',
   },
   packagesColumn: {
      height: '100%',
      display: 'flex',
      flexDirection: 'column',
      flex: 1,
   },
});

const mapStateToProps = (state: RootState) => ({
   projectId: updateSystemSelectors.getSelectedProjectId(state),
   loaded: selectors.packagesLoaded(state),
});

const dispatchProps = {
   loadPackages: actions.loadUpdatePackages.request,
};

type Props = typeof dispatchProps & ReturnType<typeof mapStateToProps>;

function UpdatePackagesRoute({ loadPackages, projectId, loaded }: Props) {
   const classes = useStyles();

   useEffect(() => {
      if (projectId !== null && !loaded) {
         loadPackages(projectId);
      }
   }, [projectId, loadPackages, loaded]);

   if (projectId === null) {
      return null;
   }

   return (
      <div className={classes.root}>
         <div className={classes.packagesColumn}>
            <Box flex={1}>
               <PackagesList />
            </Box>
            <PackagesActions projectId={projectId} />
         </div>
      </div>
   );
}

export default connect(
   mapStateToProps,
   dispatchProps,
)(UpdatePackagesRoute);
