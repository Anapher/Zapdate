import { combineEpics } from 'redux-observable';
import * as authEpics from 'src/features/auth/epics';
import * as projectsEpics from 'src/features/projects/epics';
import * as updatePackageEpics from 'src/features/update-packages/epics';
import * as updateSystemEpics from 'src/features/update-system/epics';

export default combineEpics(
   ...Object.values(authEpics),
   ...Object.values(projectsEpics),
   ...Object.values(updateSystemEpics),
   ...Object.values(updatePackageEpics),
);
