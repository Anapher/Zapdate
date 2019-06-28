import { createAsyncAction } from 'typesafe-actions';
import { CreateProjectRequest } from 'MyModels';
import { IRequestErrorResponse } from 'src/utils/error-result';

export const createProjectAsync = createAsyncAction(
   'PROJECTS/CREATE_REQUEST',
   'PROJECTS/CREATE_SUCCESS',
   'PROJECTS/CREATE_FAILURE',
)<CreateProjectRequest, any, IRequestErrorResponse>();
