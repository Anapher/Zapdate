import {
   AppBar,
   Button,
   DialogActions,
   DialogContent,
   DialogTitle,
   IconButton,
   makeStyles,
   Toolbar,
   Typography,
   withMobileDialog,
} from '@material-ui/core';
import { WithMobileDialog } from '@material-ui/core/withMobileDialog';
import CloseIcon from '@material-ui/icons/Close';
import React, { useContext } from 'react';
import { DialogContext, ReponsiveDialogContext } from './ResponsiveDialog';

interface UserProps {
   onAffirmer: () => void;
   affirmerText: string;
   children: React.ReactNode;
   isAffirmerDisabled?: boolean;
   isCancelDisabled?: boolean;
}

interface DialogProps extends UserProps, ReponsiveDialogContext {}

const useStyles = makeStyles({
   appBar: {},
   flex: {
      flex: 1,
   },
   mobileContent: {
      margin: 16,
      height: '100%',
      overflowY: 'auto',
      overflowX: 'hidden',
   },
});

function NormalDialog({
   title,
   children,
   onClose,
   onAffirmer,
   affirmerText,
   isCancelDisabled,
   isAffirmerDisabled,
}: DialogProps) {
   return (
      <React.Fragment>
         <DialogTitle>{title}</DialogTitle>
         <DialogContent>{children}</DialogContent>
         <DialogActions>
            <Button onClick={onClose} color="primary" disabled={isCancelDisabled}>
               Cancel
            </Button>
            <Button onClick={onAffirmer} color="primary" autoFocus disabled={isAffirmerDisabled}>
               {affirmerText}
            </Button>
         </DialogActions>
      </React.Fragment>
   );
}

function FullscreenDialog({
   title,
   children,
   onClose,
   onAffirmer,
   affirmerText,
   isAffirmerDisabled,
}: DialogProps) {
   const classes = useStyles();

   return (
      <div style={{ height: '100%' }}>
         <AppBar position="sticky" className={classes.appBar}>
            <Toolbar>
               <IconButton color="inherit" onClick={onClose} aria-label="Close">
                  <CloseIcon />
               </IconButton>
               <Typography variant="h6" color="inherit" className={classes.flex}>
                  {title}
               </Typography>
               <Button color="inherit" onClick={onAffirmer} disabled={isAffirmerDisabled}>
                  {affirmerText}
               </Button>
            </Toolbar>
         </AppBar>
         <div className={classes.mobileContent}>{children}</div>
      </div>
   );
}

type Props = WithMobileDialog & UserProps;
function ResponsiveDialogContent({ fullScreen, ...props }: Props) {
   const context = useContext(DialogContext);

   return fullScreen ? (
      <FullscreenDialog {...props} {...context} />
   ) : (
      <NormalDialog {...props} {...context} />
   );
}

export default withMobileDialog()(ResponsiveDialogContent);
