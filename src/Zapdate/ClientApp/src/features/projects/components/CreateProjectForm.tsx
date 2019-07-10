import { Box, Grid, MenuItem, Typography } from '@material-ui/core';
import InfoIcon from '@material-ui/icons/Info';
import { Field, Formik, FormikActions } from 'formik';
import { TextField } from 'formik-material-ui';
import { CreateProjectRequest } from 'MyModels';
import React, { useCallback } from 'react';
import { RouteComponentProps, withRouter } from 'react-router';
import ResponsiveDialogContainer from 'src/components/ResponsiveDialogContent';
import SelectField from 'src/components/SelectField';
import useAsyncFunction from 'src/hooks/use-async-function';
import { applyError } from 'src/utils/formik-helpers';
import * as yup from 'yup';
import * as actions from '../actions';
import download from 'src/utils/memory-download';
import { getSafeFilename } from 'src/utils/string-helper';

const initialValues: CreateProjectRequest = {
   name: '',
   rsaKeyPassword: undefined,
   rsaKeyStorage: 'server',
};

const schema = yup.object().shape({
   name: yup.string().required(),
   rsaKeyStorage: yup.string().required(),
   rsaKeyPassword: yup.string().when('rsaKeyStorage', {
      is: x => x === 'serverEncrypted',
      then: yup
         .string()
         .required()
         .min(6, 'The password requires at least 6 characters.'),
   }),
});

function CreateProjectForm({ history }: RouteComponentProps) {
   const createProjectAction = useAsyncFunction(
      actions.createProjectAsync.request,
      actions.createProjectAsync.success,
      actions.createProjectAsync.failure,
   );

   const submit = useCallback(
      async (values: CreateProjectRequest, formikActions: FormikActions<CreateProjectRequest>) => {
         const { setSubmitting } = formikActions;
         try {
            const result = await createProjectAction!(values);
            if (result.asymmetricKey) {
               const filename = getSafeFilename(values.name) + '-private_key.json';
               download(filename, result.asymmetricKey);
            }

            history.push('/');
         } catch (error) {
            applyError(error, formikActions);
         } finally {
            setSubmitting(false);
         }
      },
      [createProjectAction, history],
   );

   return (
      <Formik<CreateProjectRequest>
         initialValues={initialValues}
         validationSchema={schema}
         onSubmit={submit}
      >
         {({ submitForm, values: { rsaKeyStorage }, isValid, isSubmitting, status }) => (
            <ResponsiveDialogContainer
               affirmerText="Create"
               onAffirmer={submitForm}
               isAffirmerDisabled={!isValid || isSubmitting}
            >
               <Grid spacing={2} container>
                  <Grid item xs={12}>
                     <Field
                        label="Project Name"
                        name="name"
                        fullWidth
                        required
                        component={TextField}
                     />
                  </Grid>
                  <Grid item xs={12}>
                     <Field name="rsaKeyStorage" component={SelectField} label="RSA Key Storage">
                        <MenuItem value="server">Server</MenuItem>
                        <MenuItem value="serverEncrypted">Server (encrypted)</MenuItem>
                        <MenuItem value="locally">Locally</MenuItem>
                     </Field>
                  </Grid>
                  {rsaKeyStorage === 'serverEncrypted' && (
                     <Grid item xs={12}>
                        <Field
                           label="RSA Key Password"
                           name="rsaKeyPassword"
                           fullWidth
                           type="password"
                           required
                           component={TextField}
                        />
                     </Grid>
                  )}
                  <Grid item xs={12}>
                     <Typography variant="subtitle2">
                        The RSA key is used to sign every update file. If a cracker gains control
                        over your server, it won't be possible to infect your files without having
                        access to the key. On the other side, if you loose the key, the update
                        system will also be unusable.
                     </Typography>
                  </Grid>
                  <Grid item xs={12}>
                     <Box display="flex" flexDirection="row">
                        <InfoIcon />
                        <Box flex={1} ml={1}>
                           <Typography variant="caption">
                              {rsaKeyStorage === 'server' && (
                                 <React.Fragment>
                                    The security mechanism is rendered useless. The key will be
                                    stored in clear text on the server.
                                 </React.Fragment>
                              )}
                              {rsaKeyStorage === 'serverEncrypted' && (
                                 <React.Fragment>
                                    The RSA key will be encrypted with a password and stored on the
                                    server. Every time you create an update, you have to input this
                                    password. If you forget it, the update system is unusable.
                                 </React.Fragment>
                              )}
                              {rsaKeyStorage === 'locally' && (
                                 <React.Fragment>
                                    After the project is created, the private key will be deleted on
                                    the server and downloaded to your computer. Every time you
                                    create an update, you have to submit this file. If you loose it,
                                    the update system is unusable.
                                 </React.Fragment>
                              )}
                           </Typography>
                        </Box>
                     </Box>
                  </Grid>
                  {status && <Typography color="error">{status}</Typography>}
               </Grid>
            </ResponsiveDialogContainer>
         )}
      </Formik>
   );
}

export default withRouter(CreateProjectForm);
