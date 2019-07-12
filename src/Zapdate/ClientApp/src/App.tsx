import LuxonUtils from '@date-io/luxon';
import { createMuiTheme, CssBaseline } from '@material-ui/core';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import { ThemeProvider } from '@material-ui/styles';
import React from 'react';
import { connect } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { RootState } from 'zapdate';
import AnonymousRoutes from './routes/anonymous';
import AuthenticatedRoutes from './routes/authenticated';

const theme = createMuiTheme({
   palette: {
      type: 'dark',
      primary: {
         main: '#2980b9',
      },
   },
});

const mapStateToProps = (state: RootState) => ({ isAuthenticated: state.auth.isAuthenticated });

type Props = ReturnType<typeof mapStateToProps>;

function App({ isAuthenticated }: Props) {
   return (
      <ThemeProvider theme={theme}>
         <CssBaseline />
         <MuiPickersUtilsProvider utils={LuxonUtils}>
            <BrowserRouter>
               {isAuthenticated ? <AuthenticatedRoutes /> : <AnonymousRoutes />}
            </BrowserRouter>
         </MuiPickersUtilsProvider>
      </ThemeProvider>
   );
}

export default connect(mapStateToProps)(App);
