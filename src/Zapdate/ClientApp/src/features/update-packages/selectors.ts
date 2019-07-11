import { RootState } from 'zapdate';

export const packagesLoaded = (state: RootState) => state.updatePackages.list !== null;
