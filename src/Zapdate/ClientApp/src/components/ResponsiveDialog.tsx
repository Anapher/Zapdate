import { Dialog, Slide, withMobileDialog } from '@material-ui/core';
import { DialogProps } from '@material-ui/core/Dialog';
import { TransitionProps } from '@material-ui/core/transitions/transition';
import { WithMobileDialog } from '@material-ui/core/withMobileDialog';
import React, { useEffect, useState } from 'react';

export interface ReponsiveDialogContext {
   title?: string;
   onClose?: any;
}

export const DialogContext = React.createContext<ReponsiveDialogContext>({} as any);

const FullscreenTransition = React.forwardRef<unknown, TransitionProps>(function Transition(
   props,
   ref,
) {
   return <Slide direction="up" ref={ref} {...props} />;
});

type Props = DialogProps & WithMobileDialog;

function ResponsiveDialog({ children, ...props }: Props) {
   const { fullScreen, title, open, onClose } = props;

   const [dialogContext, setDialogContext] = useState<ReponsiveDialogContext>({
      title,
      onClose,
   });

   useEffect(() => {
      setDialogContext({ title, onClose });
   }, [title, onClose]);

   return (
      <Dialog
         fullWidth
         TransitionComponent={fullScreen ? FullscreenTransition : undefined}
         aria-labelledby={title}
         {...props}
      >
         <DialogContext.Provider value={dialogContext}>{open && children}</DialogContext.Provider>
      </Dialog>
   );
}

export default withMobileDialog()(ResponsiveDialog);
