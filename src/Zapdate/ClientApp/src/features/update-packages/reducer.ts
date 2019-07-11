import { combineReducers } from 'redux';
import { getType } from 'typesafe-actions';
import { UpdatePackagePreviewDto } from 'UpdateSystemModels';
import { RootAction } from 'zapdate';
import { selectProjectAsync } from '../update-system/actions';
import * as actions from './actions';

export type UpdatePackagesState = Readonly<{
   list: UpdatePackagePreviewDto[] | null;
}>;

export default combineReducers<UpdatePackagesState, RootAction>({
   list: (state = null, action) => {
      switch (action.type) {
         case getType(selectProjectAsync.request):
            return null;
         case getType(actions.loadUpdatePackages.success):
            return action.payload;
         default:
            return state;
      }
   },
});
