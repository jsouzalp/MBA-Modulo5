import { Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { UserRegisterComponent } from './register/register.component';
import { AuthConnectedGuard } from 'src/app/auth.guard';

export const UserRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'login',
        component: LoginComponent, canActivate: [AuthConnectedGuard],
      },
      {
        path: 'register',
        component: UserRegisterComponent, canActivate: [AuthConnectedGuard],
      },
    ],
  },
];
