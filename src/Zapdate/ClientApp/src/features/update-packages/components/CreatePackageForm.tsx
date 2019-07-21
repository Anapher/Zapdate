import { makeStyles, Tab, Tabs } from '@material-ui/core';
import { Formik } from 'formik';
import React, { useState } from 'react';
import SwipeableViews from 'react-swipeable-views';
import semver from 'semver';
import ResponsiveDialogContent from 'src/components/ResponsiveDialogContent';
import {
   UpdatePackageDto,
   UpdateFileDto,
   UpdateChangelogInfo,
   UpdatePackageDistributionInfo,
} from 'UpdateSystemModels';
import * as yup from 'yup';
import CreatePackageFormPackage from './CreatePackageFormPackage';
import { RootState } from 'zapdate';
import { connect } from 'react-redux';
import CreatePackageFormFields from './CreatePackageFormFields';

const useStyles = makeStyles({
   root: {
      maxHeight: '100%',
   },
});

const mapStateToProps = (state: RootState) => ({
   distributionChannels: state.updateSystem.selected!.distributionChannels,
});

const initialValues = (channels: string[]): UpdatePackageDto => ({
   version: '1.0.0',
   distribution: channels.map(x => ({ name: x, isRolledBack: false, isEnforced: false })),
   changelogs: [],
   customFields: {},
   description: '',
   files: [],
});

const schema = yup.object().shape({
   version: yup
      .string()
      .required()
      .test(
         'semversion',
         'Please provide a valid semantic version',
         value => semver.valid(value, { loose: false }) !== null,
      ),
   description: yup.string(),
});

type Props = ReturnType<typeof mapStateToProps>;

function CreatePackageForm({ distributionChannels }: Props) {
   const [selectedTab, setSelectedTab] = useState(0);
   const classes = useStyles();

   return (
      <Formik<UpdatePackageDto>
         onSubmit={() => {}}
         initialValues={initialValues(distributionChannels)}
         validationSchema={schema}
      >
         {formikProps => (
            <ResponsiveDialogContent
               affirmerText="Create"
               onAffirmer={formikProps.submitForm}
               disableMargin
            >
               <div className={classes.root}>
                  <Tabs
                     value={selectedTab}
                     variant="scrollable"
                     onChange={(_, i) => setSelectedTab(i)}
                  >
                     <Tab label="Package" />
                     <Tab label="Custom Fields" />
                     <Tab label="Changelog" />
                     <Tab label="Files" />
                  </Tabs>
                  <SwipeableViews index={selectedTab}>
                     <CreatePackageFormPackage formikProps={formikProps} />
                     <CreatePackageFormFields formikProps={formikProps} />
                  </SwipeableViews>
               </div>
            </ResponsiveDialogContent>
         )}
      </Formik>
   );
}

export default connect(mapStateToProps)(CreatePackageForm);
