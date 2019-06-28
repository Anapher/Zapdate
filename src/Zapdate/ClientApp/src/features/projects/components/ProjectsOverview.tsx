import { Button } from '@material-ui/core';
import React from 'react';
import to from 'src/utils/to';
import CreateProjectDialog from './CreateProjectDialog';

export default function ProjectsOverview() {
   return (
      <div>
         <Button {...to('/create-project')}>Create Project</Button>
         <CreateProjectDialog />
      </div>
   );
}
