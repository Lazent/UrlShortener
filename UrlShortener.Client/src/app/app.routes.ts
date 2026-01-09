import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { UrlListComponent } from './components/url-list/url-list.component';
import { UrlDetailsComponent } from './components/url-details/url-details.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { 
    path: 'login', 
    component:  LoginComponent 
  },
  { 
    path: 'register', 
    component: RegisterComponent 
  },
  { 
    path: 'urls', 
    component: UrlListComponent
  },
  { 
    path: 'url/:id', 
    component:  UrlDetailsComponent,
    canActivate: [authGuard]
  },
  { 
    path: '', 
    redirectTo: '/urls', 
    pathMatch: 'full' 
  },
  { 
    path:  '**', 
    redirectTo: '/urls' 
  }
];