declare module 'MyModels' {
   export interface CreateProjectRequest {
      name: string;
      rsaKeyStorage: RsaKeyStorage;
      rsaKeyPassword?: string;
   }

   export interface CreateProjectResponse {
      projectId: number;
      asymmetricKey?: string;
   }

   export interface ProjectDto {
      id: number;
      name: string;
   }

   export type RsaKeyStorage = 'server' | 'serverEncrypted' | 'locally';
}
