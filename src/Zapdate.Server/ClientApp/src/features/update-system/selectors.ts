import { RootState } from 'zapdate.server';

export const getSelectedProject = (state: RootState) => state.updateSystem.selected;

export const getSelectedProjectId = (state: RootState) =>
   state.updateSystem.selected && state.updateSystem.selected.id;
