import { IRequestErrorResponse } from 'src/utils/error-result';
import { createAsyncAction } from 'typesafe-actions';
import { UpdatePackagePreviewDto } from 'UpdateSystemModels';

export const loadUpdatePackages = createAsyncAction(
   'UPDATEPACKAGES/LOAD_REQUEST',
   'UPDATEPACKAGES/LOAD_SUCCESS',
   'UPDATEPACKAGES/LOAD_FAILURE',
)<number, UpdatePackagePreviewDto[], IRequestErrorResponse>();
