import { Routes } from '@angular/router';
import { BlankComponent } from './layouts/blank/blank.component';
import { FullComponent } from './layouts/full/full.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';

import { AuthConnectedGuard, AuthGuard } from './auth.guard';
import { LoginComponent } from './pages/user/login/login.component';
import { ForgotPasswordComponent } from './pages/user/forgot-password/forgot-password.component';
import { RestorePasswordComponent } from './pages/user/restore-password/restore-password.component';

export const routes: Routes = [
  {
    path: '',
    component: FullComponent,
    children: [
      {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full',
      },
      {
        path: 'pages',
        loadChildren: () => import('./pages/pages.routes').then((m) => m.PagesRoutes),
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard]
      }
      ,
    ],
  },
  {
    path: '',
    component: BlankComponent,
    children: [
      {
        path: 'authentication',
        loadChildren: () => import('./pages/user/user.routes').then((m) => m.UserRoutes),
      },
    ],
  },
  { path: 'login', component: LoginComponent, canActivate: [AuthConnectedGuard] },
  { path: 'authentication/forgot-password', component: ForgotPasswordComponent, canActivate: [AuthConnectedGuard] },
  { path: 'restore-password', component: RestorePasswordComponent, canActivate: [AuthConnectedGuard] },
  { path: '**', component: NotFoundComponent }
];
