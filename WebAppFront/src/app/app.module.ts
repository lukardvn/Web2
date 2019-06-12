import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatButtonModule, MatCheckboxModule, MatToolbarModule, MatInputModule, MatCardModule} from '@angular/material';
import { FormsModule } from '@angular/forms'
import { RouterModule, Routes } from '@angular/router'


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { RedvoznjeComponent } from './redvoznje/redvoznje.component';
import { MrezalinijaComponent } from './mrezalinija/mrezalinija.component';
import { LokacijavozilaComponent } from './lokacijavozila/lokacijavozila.component';
import { CenovnikComponent } from './cenovnik/cenovnik.component';
import { RegistracijaComponent } from './registracija/registracija.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'registracija', component: RegistracijaComponent },
  { path: 'redvoznje', component: RedvoznjeComponent },
  { path: 'lokacijavozila', component: LokacijavozilaComponent },
  { path: 'cenovnik', component: CenovnikComponent },
  { path: 'mrezalinija', component: MrezalinijaComponent },
  { path: '**', redirectTo: 'home' }
]

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    RedvoznjeComponent,
    MrezalinijaComponent,
    LokacijavozilaComponent,
    CenovnikComponent,
    RegistracijaComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatToolbarModule,
    MatInputModule,
    FormsModule,
    MatCardModule,
    RouterModule.forRoot(routes) 
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
