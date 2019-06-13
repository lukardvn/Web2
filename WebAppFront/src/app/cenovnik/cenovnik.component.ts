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

  cenaTemp : number = 1000;

  constructor(private http: AuthHttpService) { }
  
  ngOnInit() {
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
    this.http.GetKupiKartu(email).subscribe((cena)=>{
      this.retVal = cena;
     
      err => console.log(err);
    });
    console.log(this.retVal);
  }
// regularna,
// dnevna,
// mesecna,
// godisnja




}
