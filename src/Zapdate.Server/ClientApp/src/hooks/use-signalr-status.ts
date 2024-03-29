import { RootState } from 'zapdate.server';
import { useCallback } from 'react';
import { useMappedState } from 'redux-react-hook';

export function useSignalrStatus(): boolean {
   const mapState = useCallback((state: RootState) => state.signalr.isConnected, []);
   return useMappedState(mapState);
}
