import { Button, Typography } from '@material-ui/core';
import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import CenteredContent from 'src/components/CenteredContent';
import LoadingPlaceholder from 'src/components/LoadingPlaceholder';
import to from 'src/utils/to';
import { RootState } from 'zapdate';
import * as actions from '../actions';
import CreateProjectDialog from './CreateProjectDialog';
import ProjectsView from './ProjectsView';

const mapStateToProps = (state: RootState) => ({
   projects: state.projects.list,
   error: state.projects.loadingError,
});

const dispatchProps = {
   loadProjects: actions.loadProjectsAsync.request,
};

type Props = typeof dispatchProps & ReturnType<typeof mapStateToProps>;
function ProjectsOverview({ loadProjects, projects, error }: Props) {
   useEffect(() => {
      if (projects === null) {
         loadProjects();
      }
   }, [projects]);

   if (error !== null) {
      return (
         <CenteredContent>
            <Typography style={{ maxWidth: 600, margin: 24, maxHeight: 400 }} color="error">
               {error}
            </Typography>
            <Button onClick={loadProjects}>Retry</Button>
         </CenteredContent>
      );
   }

   if (projects === null) {
      return <LoadingPlaceholder label="Loading projects..." />;
   }

   return (
      <React.Fragment>
         <ProjectsView projects={projects} />
         <CreateProjectDialog />
      </React.Fragment>
   );
}

export default connect(
   mapStateToProps,
   dispatchProps,
)(ProjectsOverview);
