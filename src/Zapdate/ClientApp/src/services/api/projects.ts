import Axios from 'axios';
import { CreateProjectRequest, CreateProjectResponse, ProjectDto } from 'MyModels';

export async function create(dto: CreateProjectRequest): Promise<CreateProjectResponse> {
   const response = await Axios.post<CreateProjectResponse>('/api/v1/projects', dto);
   return response.data;
}

export async function load(): Promise<ProjectDto[]> {
   const response = await Axios.get<ProjectDto[]>('/api/v1/projects');
   return response.data;
}
