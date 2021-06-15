import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { AuthorizeComponent } from 'app/views/home/authorize/authorize.component';
import { HomeRoutingModule } from 'app/views/home/home-routing.module';
import { SharedComponentsModule } from 'app/views/shared/shared-components.module';
import { HomeComponent } from './home.component';


@NgModule({
  declarations: [HomeComponent, AuthorizeComponent],
  imports: [
    CommonModule,
    SharedComponentsModule,
    HomeRoutingModule,
    MatButtonModule,
    MatDialogModule
  ]
})
export class HomeModule {
}
