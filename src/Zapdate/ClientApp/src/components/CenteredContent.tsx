import { makeStyles } from '@material-ui/core';
import React from 'react';

const useStyles = makeStyles({
   root: {
      width: '100%',
      height: '100%',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
   },
});

type Props = {
   children: React.ReactNode;
};
export default function CenteredContent({ children }: Props) {
   const classes = useStyles();
   return <div className={classes.root}>{children}</div>;
}
