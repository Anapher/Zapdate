import { AxiosError } from 'axios';
import { from, of } from 'rxjs';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import toErrorResult from 'src/utils/error-result';
import { isActionOf } from 'typesafe-actions';
import { RootEpic } from 'zapdate';
import * as actions from './actions';

export const createProjectEpic: RootEpic = (action$, _, { api }) =>
   action$.pipe(
      filter(isActionOf(actions.createProjectAsync.request)),
      switchMap(({ payload }) =>
         from(api.projects.create(payload)).pipe(
            map(response => actions.createProjectAsync.success(response)),
            catchError((error: AxiosError) =>
               of(actions.createProjectAsync.failure(toErrorResult(error))),
            ),
         ),
      ),
   );

export const loadProjectsEpic: RootEpic = (action$, _, { api }) =>
   action$.pipe(
      filter(isActionOf(actions.loadProjectsAsync.request)),
      switchMap(() =>
         from(api.projects.load()).pipe(
            map(response => actions.loadProjectsAsync.success(response)),
            catchError((error: AxiosError) =>
               of(actions.loadProjectsAsync.failure(toErrorResult(error))),
            ),
         ),
      ),
   );
