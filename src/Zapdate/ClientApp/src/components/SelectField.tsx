import { FormControl, FormHelperText, InputLabel, Select } from '@material-ui/core';
import { FieldProps } from 'formik';
import React from 'react';

type Props = FieldProps & {
   label: string;
   children: React.ReactNode;
   helperText?: string;
};

export default function SelectField({
   label,
   children,
   helperText,
   field,
   form,
   ...otherProps
}: Props) {
   const isError = Boolean(form.errors[field.name] && form.touched[field.name]);

   return (
      <FormControl fullWidth>
         <InputLabel htmlFor="select-field" error={isError}>
            {label}
         </InputLabel>
         <Select
            inputProps={{
               name: field.name,
               id: 'select-field',
            }}
            fullWidth
            {...field}
            {...otherProps}
         >
            {children}
         </Select>
         {(isError || helperText) && (
            <FormHelperText error={isError}>
               {isError ? form.errors[field.name] : helperText}
            </FormHelperText>
         )}
      </FormControl>
   );
}
