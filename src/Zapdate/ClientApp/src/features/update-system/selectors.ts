import { RootState } from 'zapdate';

export const getSelectedProject = (state: RootState) => state.updateSystem.selected;

export const getSelectedProjectId = (state: RootState) =>
   state.updateSystem.selected && state.updateSystem.selected.id;
