import { AxiosError } from 'axios';
import { defer, from, iif, of } from 'rxjs';
import { catchError, filter, ignoreElements, map, switchMap } from 'rxjs/operators';
import toErrorResult from 'src/utils/error-result';
import { isActionOf } from 'typesafe-actions';
import { ZapdateEpic } from 'zapdate';
import * as actions from './actions';

export const loadProjectsEpic: ZapdateEpic = (action$, state$, { api }) =>
   action$.pipe(
      filter(isActionOf(actions.selectProjectAsync.request)),
      switchMap(action =>
         iif(
            () => action.payload === null,
            of(ignoreElements()), // if the selected project is null, do nothing
            iif(
               () =>
                  state$.value.projects.list !== null &&
                  state$.value.projects.list.findIndex(x => x.id === action.payload) > -1,
               defer(() =>
                  of(
                     // of we already loaded the project, return it
                     actions.selectProjectAsync.success(
                        state$.value.projects.list!.find(x => x.id === action.payload!)!,
                     ),
                  ),
               ),
               defer(() =>
                  from(api.projects.get(action.payload!)).pipe(
                     // else we fetch the information
                     map(response => actions.selectProjectAsync.success(response)),
                     catchError((error: AxiosError) =>
                        of(actions.selectProjectAsync.failure(toErrorResult(error))),
                     ),
                  ),
               ),
            ),
         ),
      ),
   );
