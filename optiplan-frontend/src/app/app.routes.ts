import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { LoadingComponent } from './loading/loading.component';
import { AtherLandingComponent } from './ather-landing/ather-landing.component';

export const routes: Routes = [
    {path:'register',component: RegisterComponent},
    {path:'login',component:LoginComponent},
    {path:'',component:LoadingComponent},
    {path:'landing', component:AtherLandingComponent}
];
