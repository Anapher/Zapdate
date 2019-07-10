import { ProjectDto } from 'MyModels';
import { combineReducers } from 'redux';
import { getType } from 'typesafe-actions';
import { RootAction } from 'zapdate';
import * as actions from './actions';

export type ProjectsState = Readonly<{
   projects: ProjectDto[] | null;
   projectsLoadingError: string | null;
}>;

export default combineReducers<ProjectsState, RootAction>({
   projects: (state = null, action) => {
      switch (action.type) {
         case getType(actions.loadProjectsAsync.success):
            return action.payload;
         default:
            return state;
      }
   },
   projectsLoadingError: (state = null, action) => {
      switch (action.type) {
         case getType(actions.loadProjectsAsync.failure):
            return action.payload.toString();
         case getType(actions.loadProjectsAsync.request):
            return null;
         default:
            return state;
      }
   },
});
