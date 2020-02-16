import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatButtonModule, MatCheckboxModule, MatToolbarModule, MatInputModule, MatCardModule} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { MatSelectModule } from '@angular/material';
import { RouterModule, Routes } from '@angular/router'
import { AgmCoreModule } from '@agm/core'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { HttpService } from './services/http.service';
import { AuthHttpService } from './services/auth.service';
import { from } from 'rxjs';
import { TokenInterceptor } from './interceptors/token.interceptor';



import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ControllerviewComponent } from './controllerview/controllerview.component';
import { HeaderComponent } from './header/header.component';
import { RedvoznjeComponent } from './redvoznje/redvoznje.component';
import { MrezalinijaComponent } from './mrezalinija/mrezalinija.component';
import { LokacijavozilaComponent } from './lokacijavozila/lokacijavozila.component';
import { CenovnikComponent } from './cenovnik/cenovnik.component';
import { RegistracijaComponent } from './registracija/registracija.component';
import { HomeComponent } from './home/home.component';
import { analyzeNgModules } from '@angular/compiler';
import { FooterComponent } from './footer/footer.component';
import { InterceptorsComponent } from './interceptors/interceptors.component';
import { AdminviewComponent } from './adminview/adminview.component';
import { LinijeeditComponent } from './linijeedit/linijeedit.component';
import { RedvoznjeeditComponent } from './redvoznjeedit/redvoznjeedit.component';
import { StaniceeditComponent } from './staniceedit/staniceedit.component';
import { StaniceeditdvaComponent } from './staniceeditdva/staniceeditdva.component';
import { LinijeeditdvaComponent } from './linijeeditdva/linijeeditdva.component';
import { RedvoznjeeditdvaComponent } from './redvoznjeeditdva/redvoznjeeditdva.component';
import { SpajanjeeditComponent } from './spajanjeedit/spajanjeedit.component';
import { LinijeupdateComponent } from './linijeupdate/linijeupdate.component';
import { StanicaupdateComponent } from './stanicaupdate/stanicaupdate.component';
import { RedvoznjeupdateComponent } from './redvoznjeupdate/redvoznjeupdate.component';
import { ProfilComponent } from './profil/profil.component';
import { CenovnikeditComponent } from './cenovnikedit/cenovnikedit.component';
import { CenovnikeditdvaComponent } from './cenovnikeditdva/cenovnikeditdva.component';
import { CenovnikupdateComponent } from './cenovnikupdate/cenovnikupdate.component';
import { VerifyUserComponent } from './verify-user/verify-user.component';

const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'registracija', component: RegistracijaComponent },
  { path: 'redvoznje', component: RedvoznjeComponent },
  { path: 'lokacijavozila', component: LokacijavozilaComponent },
  { path: 'cenovnik', component: CenovnikComponent },
  { path: 'mrezalinija', component: MrezalinijaComponent },
  { path: 'adminview', component: AdminviewComponent },
  { path: 'linijeedit', component: LinijeeditComponent },
  { path: 'redvoznjeedit', component: RedvoznjeeditComponent },
  { path: 'staniceedit', component: StaniceeditComponent },
  { path: 'linijeeditdva', component: LinijeeditdvaComponent },
  { path: 'redvoznjeeditdva', component: RedvoznjeeditdvaComponent },
  { path: 'staniceeditdva', component: StaniceeditdvaComponent },
  { path: 'spajanjeedit', component: SpajanjeeditComponent },
  { path: 'linijeupdate', component: LinijeupdateComponent },
  { path: 'stanicaupdate', component: StanicaupdateComponent },
  { path: 'redvoznjeupdate', component: RedvoznjeupdateComponent },
  { path: 'profil', component: ProfilComponent },
  { path: 'cenovnikedit', component: CenovnikeditComponent },
  { path: 'cenovnikeditdva', component: CenovnikeditdvaComponent },
  { path: 'cenovnikupdate', component: CenovnikupdateComponent },
  { path: 'controllerview', component: ControllerviewComponent},
  { path: 'verify-user', component: VerifyUserComponent},

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
    HomeComponent,
    FooterComponent,
    InterceptorsComponent,
    AdminviewComponent,
    LinijeeditComponent,
    RedvoznjeeditComponent,
    StaniceeditComponent,
    StaniceeditdvaComponent,
    LinijeeditdvaComponent,
    RedvoznjeeditdvaComponent,
    SpajanjeeditComponent,
    LinijeupdateComponent,
    StanicaupdateComponent,
    RedvoznjeupdateComponent,
    ProfilComponent,
    CenovnikeditComponent,
    CenovnikeditdvaComponent,
    CenovnikupdateComponent,
    ControllerviewComponent,
    VerifyUserComponent
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatToolbarModule,
    MatSelectModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    RouterModule.forRoot(routes),
     AgmCoreModule.forRoot(
     {
       apiKey: 'AIzaSyDEcXc-akV1qndV5LW7eWlPrLYMeHJZ-NU'
    }),
    HttpClientModule

   
  ],
  providers: [HttpService, {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true}, AuthHttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
