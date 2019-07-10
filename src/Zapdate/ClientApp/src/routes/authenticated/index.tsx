import React from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';
import MainRoute from './MainRoute';
import UpdateSystemRoute from './UpdateSystemRoute';

export default function AuthenticatedRoutes() {
   return (
      <Switch>
         <Route path="/projects/:id(\d+)" component={UpdateSystemRoute} />
         <Route path="/projects" component={MainRoute} />
         <Route path="/" render={() => <Redirect to="/projects" />} />
      </Switch>
   );
}
