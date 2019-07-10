import { Card, CardActionArea, Typography } from '@material-ui/core';
import { ProjectDto } from 'MyModels';
import React from 'react';
import to from 'src/utils/to';

type Props = {
   project: ProjectDto;
};
export default function ProjectCard({ project }: Props) {
   return (
      <Card>
         <CardActionArea {...to(`/projects/${project.id}`)}>
            <Typography style={{ height: 200 }}>{project.name}</Typography>
         </CardActionArea>
      </Card>
   );
}
