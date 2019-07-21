import Axios from 'axios';
import { UpdatePackagePreviewDto } from 'UpdateSystemModels';

export async function getAll(projectId: number): Promise<UpdatePackagePreviewDto[]> {
   const response = await Axios.get<UpdatePackagePreviewDto[]>(
      `/api/v1/projects/${projectId}/updates`,
   );
   return response.data;
}
