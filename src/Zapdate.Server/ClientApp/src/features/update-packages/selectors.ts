import { RootState } from 'zapdate.server';

export const packagesLoaded = (state: RootState) => state.updatePackages.list !== null;
