import React from 'react';
import { FormikProps, FieldArray } from 'formik';
import { UpdatePackageDto } from 'UpdateSystemModels';
import { Box, Input, InputAdornment, IconButton } from '@material-ui/core';
import DeleteIcon from '@material-ui/icons/Delete';

type Props = {
   formikProps: FormikProps<UpdatePackageDto>;
};

export default function CreatePackageFormFields({
   formikProps: {
      values: { customFields },
   },
}: Props) {
   return (
      <Box m={1}>
         <FieldArray
            name="customFields"
            render={({ handleRemove }) =>
               Object.keys(customFields!).map(key => (
                  <Box m={1} key={key}>
                     <Input
                        fullWidth
                        endAdornment={
                           <InputAdornment position="end">
                              <IconButton onClick={handleRemove()}>
                                 <DeleteIcon />
                              </IconButton>
                           </InputAdornment>
                        }
                     />
                  </Box>
               ))
            }
         />
      </Box>
   );
}
