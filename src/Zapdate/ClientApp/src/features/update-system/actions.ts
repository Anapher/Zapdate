import { ProjectDto } from 'MyModels';
import { IRequestErrorResponse } from 'src/utils/error-result';
import { createAsyncAction } from 'typesafe-actions';

export const selectProjectAsync = createAsyncAction(
   'UPDATESYSTEM/SELECT_REQUEST',
   'UPDATESYSTEM/SELECT_SUCCESS',
   'UPDATESYSTEM/SELECT_FAILURE',
)<number | null, ProjectDto, IRequestErrorResponse>();
