import { CreateProjectRequest, CreateProjectResponse, ProjectDto } from 'MyModels';
import { IRequestErrorResponse } from 'src/utils/error-result';
import { createAsyncAction } from 'typesafe-actions';

export const createProjectAsync = createAsyncAction(
   'PROJECTS/CREATE_REQUEST',
   'PROJECTS/CREATE_SUCCESS',
   'PROJECTS/CREATE_FAILURE',
)<CreateProjectRequest, CreateProjectResponse, IRequestErrorResponse>();

export const loadProjectsAsync = createAsyncAction(
   'PROJECTS/LOAD_REQUEST',
   'PROJECTS/LOAD_SUCCESS',
   'PROJECTS/LOAD_FAILURE',
)<undefined, ProjectDto[], IRequestErrorResponse>();
