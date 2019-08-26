import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatButtonModule, MatCheckboxModule, MatToolbarModule, MatInputModule, MatCardModule} from '@angular/material';
import { FormsModule } from '@angular/forms'
import { RouterModule, Routes } from '@angular/router'
import { AgmCoreModule } from '@agm/core'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { HttpService } from './services/http.service';
import { AuthHttpService } from './services/auth.service';
import { from } from 'rxjs';
import { TokenInterceptor } from './interceptors/token.interceptor';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
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
    SpajanjeeditComponent
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
