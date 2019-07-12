import {
   Box,
   Button,
   ButtonGroup,
   Checkbox,
   Grid,
   Link,
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableRow,
   Theme,
} from '@material-ui/core';
import { useTheme } from '@material-ui/styles';
import { Field, FieldArray, FormikProps } from 'formik';
import { TextField } from 'formik-material-ui';
import React from 'react';
import { UpdatePackageDto } from 'UpdateSystemModels';
import { DateTimePicker } from '@material-ui/pickers';

type Props = {
   formikProps: FormikProps<UpdatePackageDto>;
};
export default function CreatePackageFormPackage({
   formikProps: { values, setFieldValue },
}: Props) {
   const theme = useTheme<Theme>();

   return (
      <Box m={2}>
         <Grid container spacing={3} style={{ margin: 0, width: '100%' }}>
            <Grid item xs={12} lg={4}>
               <Field
                  label="Version"
                  name="version"
                  fullWidth
                  style={{ maxWidth: 380 }}
                  required
                  component={TextField}
                  helperText={
                     <React.Fragment>
                        <span>The version must comply </span>
                        <Link href="https://semver.org/">Semantic Versioning 2.0.0</Link>
                     </React.Fragment>
                  }
               />
            </Grid>
            <Grid item xs={12} lg={8}>
               <Field
                  label="Description"
                  name="description"
                  fullWidth
                  style={{ maxWidth: 800 }}
                  component={TextField}
                  helperText="An optional description for display purposes"
               />
            </Grid>
            <Grid item xs={12}>
               <FieldArray
                  name="distribution"
                  render={() => (
                     <Table size="small">
                        <TableHead>
                           <TableRow>
                              <TableCell>Channel</TableCell>
                              <TableCell>Publish</TableCell>
                              <TableCell>Enforce</TableCell>
                           </TableRow>
                        </TableHead>
                        <TableBody>
                           {values.distribution!.map((x, index) => (
                              <TableRow key={x.name}>
                                 <TableCell>{x.name}</TableCell>
                                 <TableCell>
                                    <ButtonGroup variant="contained" color="primary" size="small">
                                       <Button
                                          style={{
                                             backgroundColor:
                                                x.publishDate === undefined
                                                   ? theme.palette.primary.dark
                                                   : undefined,
                                          }}
                                          onClick={() =>
                                             setFieldValue(
                                                `distribution[${index}].publishDate`,
                                                undefined,
                                             )
                                          }
                                       >
                                          Later
                                       </Button>
                                       <Button
                                          style={{
                                             backgroundColor:
                                                x.publishDate === 'now'
                                                   ? theme.palette.primary.dark
                                                   : undefined,
                                          }}
                                          onClick={() =>
                                             setFieldValue(
                                                `distribution[${index}].publishDate`,
                                                'now',
                                             )
                                          }
                                       >
                                          Now
                                       </Button>
                                       <Button>
                                          Schedule
                                          {x.publishDate && x.publishDate !== 'now' && (
                                             <DateTimePicker
                                                value={x.publishDate}
                                                disablePast
                                                ampm={false}
                                                onChange={() => {}}
                                             />
                                          )}
                                       </Button>
                                    </ButtonGroup>
                                 </TableCell>
                                 <TableCell>
                                    <Field
                                       component={Checkbox}
                                       name={`distribution[${index}].enforce`}
                                    />
                                 </TableCell>
                              </TableRow>
                           ))}
                        </TableBody>
                     </Table>
                  )}
               />
            </Grid>
         </Grid>
      </Box>
   );
}
