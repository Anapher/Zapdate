import { combineReducers } from 'redux';

import auth from '../features/auth/reducer';
import projects from '../features/projects/reducer';
import signalrReducer from './signalr/signalr-reducer';

const rootReducer = combineReducers({
   auth,
   projects,
   signalr: signalrReducer(),
});

export default rootReducer;
