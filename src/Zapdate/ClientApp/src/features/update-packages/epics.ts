import { AxiosError } from 'axios';
import { from, of } from 'rxjs';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import toErrorResult from 'src/utils/error-result';
import { isActionOf } from 'typesafe-actions';
import { ZapdateEpic } from 'zapdate';
import * as actions from './actions';

export const loadPackagesEpic: ZapdateEpic = (action$, _, { api }) =>
   action$.pipe(
      filter(isActionOf(actions.loadUpdatePackages.request)),
      switchMap(({ payload }) =>
         from(api.updatePackages.getAll(payload)).pipe(
            map(response => actions.loadUpdatePackages.success(response)),
            catchError((error: AxiosError) =>
               of(actions.loadUpdatePackages.failure(toErrorResult(error))),
            ),
         ),
      ),
   );
