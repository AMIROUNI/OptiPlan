import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { LoadingComponent } from './loading/loading.component';
import { AtherLandingComponent } from './ather-landing/ather-landing.component';
import { DashboardComponent } from './user/dashboard/dashboard.component';
import { CreateProjectComponent } from './user/dashboard/create-project/create-project.component';
import { ProjectDetailsComponent } from './user/project-details/project-details.component';
import { AddSprintComponent } from './user/work-item-management/add-sprint/add-sprint.component';
import { WorkItemManagementComponent } from './user/work-item-management/work-item-management.component';
import { InvitationListComponent } from './invitation-list/invitation-list.component';
import { DisplayProjectComponent } from './user/dashboard/display-project/display-project.component';
import { ChatBotService } from './services/chat-bot.service';
import { ChatBotUserComponent } from './chat-bot-user/chat-bot-user.component';

export const routes: Routes = [
    {path:'register',component: RegisterComponent},
    {path:'login',component:LoginComponent},
    {path:'',component:LoadingComponent},
    {path:'landing', component:AtherLandingComponent},
    {path:'user-dashboard',component:DashboardComponent},
    {path:'create-project', component: CreateProjectComponent},
    {path:'project-details/:id',component:ProjectDetailsComponent},
    {path: 'work-item/:id',component:WorkItemManagementComponent},
    {path:'invitation-list',component:InvitationListComponent},
    {path:'my-projects',component:DisplayProjectComponent},
    {path:'chat-bot',component:ChatBotUserComponent},
    

    //**************************************************     */
  
      

     //**************************************************     */
];
