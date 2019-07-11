import { combineReducers } from 'redux';
import auth from '../features/auth/reducer';
import projects from '../features/projects/reducer';
import updatePackages from '../features/update-packages/reducer';
import updateSystem from '../features/update-system/reducer';
import signalrReducer from './signalr/signalr-reducer';

const rootReducer = combineReducers({
   auth,
   projects,
   updateSystem,
   updatePackages,
   signalr: signalrReducer(),
});

export default rootReducer;
