import { combineEpics } from 'redux-observable';
import * as authEpics from 'src/features/auth/epics';
import * as projectsEpics from 'src/features/projects/epics';

export default combineEpics(...Object.values(authEpics), ...Object.values(projectsEpics));
