declare module 'UpdateSystemModels' {
   export interface UpdatePackagePreviewDto {
      version: string;
      description?: string;
   }

   export interface UpdateFileDto {
      path: string;
      hash: string;
      signature?: string;
   }

   export interface UpdateChangelogInfo {
      language: string;
      content: string;
   }

   export interface UpdatePackageDistributionInfo {
      name: string;
      publishDate?: string;
      isRolledBack: boolean;
      isEnforced: boolean;
   }

   export interface UpdatePackageDto {
      version: string;
      description?: string;
      customFields?: { [key: string]: string };

      files: UpdateFileDto[];
      changelogs?: UpdateChangelogInfo[];
      distribution?: UpdatePackageDistributionInfo[];
   }
}
