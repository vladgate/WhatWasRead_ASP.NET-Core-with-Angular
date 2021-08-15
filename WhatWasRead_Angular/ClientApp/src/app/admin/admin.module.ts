import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "../shared/shared.module";
import { AdminOverviewComponent } from './admin-overview/admin-overview.component';
import { AuthenticationGuard } from "../auth/authentication.guard";
import { AuthModule } from "../auth/auth.module";
import { LoginPageComponent } from "../auth/login-page/login-page.component";
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { TagListComponent } from './tag-list/tag-list.component';

const routes: Routes = [
  { path: "login", component: LoginPageComponent },
  {
    path: "", component: AdminLayoutComponent, canActivateChild: [AuthenticationGuard],
      children: [
        { path: "tags", component: TagListComponent },
        //{ path: "orders", component: OrderAdminComponent },
        { path: "overview", component: AdminOverviewComponent },
        //{ path: "", redirectTo: '/admin/login', pathMatch: 'full' },
        { path: "", component: AdminOverviewComponent }
      ]
  }
];

@NgModule({
  imports: [SharedModule, RouterModule.forChild(routes), AuthModule],
  declarations: [AdminOverviewComponent, AdminLayoutComponent, TagListComponent]
})
export class AdminModule {

}
