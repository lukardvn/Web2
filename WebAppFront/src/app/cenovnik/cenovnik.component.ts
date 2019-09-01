import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth.service';
import { NgForm } from '@angular/forms';
import { Kupac, Karta } from '../modeli';

@Component({
  selector: 'app-cenovnik',
  templateUrl: './cenovnik.component.html',
  styleUrls: ['./cenovnik.component.css']
})
export class CenovnikComponent implements OnInit {

  karte : string[] = ["regularna", "dnevna", "mesecna", "godisnja" ];
  kupci : string[] = ["regularan", "student", "penzioner"];

  tipKarte : string;
  tipKupca : string;
  email : string;
  retVal : boolean;
  role : any;

  cenaTemp : number ;

  constructor(private http: AuthHttpService) { }
  
  ngOnInit() {
    if(localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != ""){
      let jwtData = localStorage.jwt.split('.')[1]
      let decodedJwtJsonData = window.atob(jwtData)
      let decodedJwtData = JSON.parse(decodedJwtJsonData)


       this.role = decodedJwtData.nameid;
    }
  }

  proveriCenu() {
    this.http.GetCenaKarte(this.tipKarte, this.tipKupca).subscribe((cena)=>{
      this.cenaTemp = cena;
     
      err => console.log(err);
    });
    console.log(this.cenaTemp);
  }

  kupiKartu(email : string, form : NgForm) {
    console.log(email);
    if(this.IsAnonymus())
    {
    this.http.GetKupiKartu(email).subscribe((cena)=>{
      this.retVal = cena;
     
      err => console.log(err);
    });
    console.log(this.retVal);
  }
  }

  kupiKartu2(email : string, form : NgForm) {
    console.log(email);
    if(this.IsNormal())
    {
    this.http.KupiKartu(email, this.tipKarte).subscribe((cena)=>{
      this.retVal = cena;
     
      err => console.log(err);
    });
    console.log(this.retVal);
  }
  }

  IsAnonymus()
  {
    if(localStorage.jwt == "undefined")
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  IsNormal(){
  
    if(this.role == "admin"){
      return false;
     }
     else if (localStorage.jwt != "undefined")
     {
       return true;
     }
     
  }
// regularna,
// dnevna,
// mesecna,
// godisnja




}
