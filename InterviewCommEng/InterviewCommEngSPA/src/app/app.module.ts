import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule} from '@angular/common/http';
import { ProductComponent } from './product/product.component';
import { appRoutes } from './route';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule, MatButtonModule, MatSidenavModule,
   MatIconModule, MatListModule, MatCardModule, MatGridList,
   MatGridListModule, MatDialogModule, MAT_DIALOG_DATA, MatDialogRef,
   MatCheckboxModule, MatTableModule, MatFormFieldModule, MatChipsModule,
   MatInputModule, MatPaginator, MatSortModule, MatPaginatorModule, MatSelectModule,
   MatDatepickerModule, MatNativeDateModule, MAT_DATE_LOCALE, MatSliderModule } from '@angular/material';

import { CategoryComponent } from './category/category.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
@NgModule({
   declarations: [
      AppComponent,
      ProductComponent,
      HomeComponent,
      CategoryComponent,
      NavbarComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      RouterModule.forRoot(appRoutes),
      BrowserAnimationsModule,
      MatSliderModule,
      MatToolbarModule,
      MatButtonModule,
      MatSidenavModule,
      MatIconModule,
      MatListModule,
      MatDatepickerModule,
      MatNativeDateModule,
      MatCardModule,
      MatGridListModule,
      MatDialogModule,
      MatCheckboxModule,
      MatTableModule,
      MatFormFieldModule,
      MatChipsModule,
      MatInputModule,
      MatPaginatorModule,
      MatSortModule,
      MatSelectModule,
      FormsModule,
      ReactiveFormsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
