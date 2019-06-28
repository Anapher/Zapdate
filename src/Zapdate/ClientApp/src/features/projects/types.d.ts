declare module 'MyModels' {
   export interface CreateProjectRequest {
      name: string;
      rsaKeyStorage: RsaKeyStorage;
      rsaKeyPassword?: string;
   }

   export type RsaKeyStorage = 'server' | 'serverEncrypted' | 'locally';
}
