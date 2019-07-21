import { Button, Theme } from '@material-ui/core';
import { useTheme } from '@material-ui/styles';
import { FieldProps } from 'formik';
import React from 'react';

type Props = {
   children?: React.ReactNode;
   toggleValue: any;
} & FieldProps;

export default function ToggleButtonField({ field, children, toggleValue, form }: Props) {
   const isSelected = field.value === toggleValue;
   const theme = useTheme<Theme>();

   return (
      <Button
         variant="contained"
         style={{ backgroundColor: isSelected ? 'red' : undefined }}
         onClick={() => form.setFieldValue(field.name, toggleValue)}
      >
         {children}
      </Button>
   );
}
