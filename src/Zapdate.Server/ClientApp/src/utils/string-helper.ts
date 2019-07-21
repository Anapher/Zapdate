export function getSafeFilename(s: string) {
   const result = s
      .replace(/[^a-z0-9]/gi, '_')
      .replace(/_{2,}/g, '_')
      .toLowerCase();

   return result.endsWith('_') ? result.slice(0, -1) : result;
}
