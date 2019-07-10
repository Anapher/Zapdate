import { CircularProgress, makeStyles, Typography } from '@material-ui/core';
import React from 'react';
import CenteredContent from './CenteredContent';

const useStyles = makeStyles({
   centerAligned: {
      display: 'flex',
      alignItems: 'center',
   },
});

type Props = {
   label: string;
};

export default function LoadingPlaceholder({ label }: Props) {
   const classes = useStyles();
   return (
      <CenteredContent>
         <div className={classes.centerAligned}>
            <CircularProgress size={32} />
            <Typography style={{ marginLeft: 16 }}>{label}</Typography>
         </div>
      </CenteredContent>
   );
}
