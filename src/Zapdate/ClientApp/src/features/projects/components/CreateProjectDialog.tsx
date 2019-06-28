import React from 'react';
import { RouteComponentProps, withRouter } from 'react-router';
import ResponsiveDialog from 'src/components/ResponsiveDialog';
import CreateProjectForm from './CreateProjectForm';

type Props = RouteComponentProps;

export const createProjectRoute = '/create-project';

function CreateProjectDialog({ location, history }: Props) {
   const isOpen = location.pathname === createProjectRoute;

   return (
      <ResponsiveDialog
         open={isOpen}
         title="Create Project"
         maxWidth="sm"
         onClose={() => history.push('/')}
      >
         <CreateProjectForm />
      </ResponsiveDialog>
   );
}

export default withRouter(CreateProjectDialog) as React.ComponentType;
