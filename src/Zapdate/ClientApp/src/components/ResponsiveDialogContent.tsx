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
import classnames from 'classnames';

interface UserProps {
   onAffirmer: () => void;
   affirmerText: string;
   children: React.ReactNode;
   isAffirmerDisabled?: boolean;
   isCancelDisabled?: boolean;
   disableMargin?: boolean;
}

interface DialogProps extends UserProps, ReponsiveDialogContext {}

const useStyles = makeStyles(theme => ({
   flex: {
      flex: 1,
   },
   mobileContent: {
      overflowY: 'auto',
      overflowX: 'hidden',
      height: '100%',
   },
   mobileContentMargin: {
      margin: 16,
   },
   mobileDialogRoot: {
      height: '100%',
   },
   toolbarPlaceholder: { ...theme.mixins.toolbar },
}));

function NormalDialog({
   title,
   children,
   onClose,
   onAffirmer,
   affirmerText,
   isCancelDisabled,
   isAffirmerDisabled,
   disableMargin,
}: DialogProps) {
   return (
      <React.Fragment>
         <DialogTitle>{title}</DialogTitle>
         {disableMargin ? children : <DialogContent>{children}</DialogContent>}
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
   disableMargin,
}: DialogProps) {
   const classes = useStyles();

   return (
      <div className={classes.mobileDialogRoot}>
         <AppBar position="absolute">
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
         <div
            className={classnames(classes.mobileContent, {
               [classes.mobileContentMargin]: !disableMargin,
            })}
         >
            <div className={classes.toolbarPlaceholder} />
            {children}
         </div>
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
