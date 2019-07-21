import React from 'react';
import { RouteComponentProps, withRouter } from 'react-router';
import ResponsiveDialog from 'src/components/ResponsiveDialog';
import CreatePackageForm from './CreatePackageForm';

type Props = RouteComponentProps & { projectId: number };

export const createPackageRoute = (projectId: number) => `/projects/${projectId}/updates/create`;

function CreatePackageDialog({ location, history, projectId }: Props) {
   const isOpen = location.pathname === createPackageRoute(projectId);

   return (
      <ResponsiveDialog
         open={isOpen}
         title="Create Update Package"
         maxWidth="lg"
         onClose={() => history.push(`/projects/${projectId}/updates`)}
      >
         <CreatePackageForm />
      </ResponsiveDialog>
   );
}

export default withRouter(CreatePackageDialog);
