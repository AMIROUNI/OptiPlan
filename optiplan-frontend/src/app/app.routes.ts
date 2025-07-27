import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { LoadingComponent } from './loading/loading.component';
import { AtherLandingComponent } from './ather-landing/ather-landing.component';
import { DashboardComponent } from './user/dashboard/dashboard.component';
import { CreateProjectComponent } from './user/dashboard/create-project/create-project.component';
import { ProjectDetailsComponent } from './user/project-details/project-details.component';
import { BacklogComponent } from './user/backlog/backlog.component';
import { AddTaskComponent } from './user/backlog/add-task/add-task.component';
import { AddSprintComponent } from './user/work-item-management/add-sprint/add-sprint.component';
import { BacklogManagementComponent } from './user/backlog/backlog-management/backlog-management.component';
import { WorkItemBoardComponent } from './user/backlog/work-item-board/work-item-board.component';
import { WorkItemManagementComponent } from './user/work-item-management/work-item-management.component';

export const routes: Routes = [
    {path:'register',component: RegisterComponent},
    {path:'login',component:LoginComponent},
    {path:'',component:LoadingComponent},
    {path:'landing', component:AtherLandingComponent},
    {path:'user-dashboard',component:DashboardComponent},
    {path:'create-project', component: CreateProjectComponent},
    {path:'project-details/:id',component:ProjectDetailsComponent},
    {path:'backlog/:id', component:BacklogManagementComponent},
    {path: 'work-item/:id',component:WorkItemManagementComponent},

    //**************************************************     */
      {path:'add-sprint/:id', component:AddSprintComponent},
      

     //**************************************************     */
];
