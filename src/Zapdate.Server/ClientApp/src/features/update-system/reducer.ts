import { ProjectDto } from 'MyModels';
import { combineReducers } from 'redux';
import { getType } from 'typesafe-actions';
import { RootAction } from 'zapdate.server';
import * as actions from './actions';

export type UpdateSystemState = Readonly<{
   selected: ProjectDto | null;
}>;

export default combineReducers<UpdateSystemState, RootAction>({
   selected: (state = null, action) => {
      switch (action.type) {
         case getType(actions.selectProjectAsync.request):
            return null;
         case getType(actions.selectProjectAsync.success):
            return action.payload;
         default:
            return state;
      }
   },
});
