import { Box, Fab, Grid } from '@material-ui/core';
import AddIcon from '@material-ui/icons/Add';
import { ProjectDto } from 'MyModels';
import React from 'react';
import to from 'src/utils/to';
import { createProjectRoute } from './CreateProjectDialog';
import ProjectCard from './ProjectCard';

type Props = {
   projects: ProjectDto[];
};

export default function ProjectsView({ projects }: Props) {
   return (
      <Box position="relative" height="100%">
         <Box overflow="auto" height="100%">
            <Grid container spacing={3} style={{ width: '100%', margin: 0 }}>
               {projects.map(x => (
                  <Grid key={x.id} item xs={12} sm={6}>
                     <ProjectCard project={x} />
                  </Grid>
               ))}
            </Grid>
         </Box>
         <Fab
            style={{ position: 'absolute', right: 24, bottom: 24 }}
            {...to(createProjectRoute)}
            color="primary"
         >
            <AddIcon />
         </Fab>
      </Box>
   );
}
