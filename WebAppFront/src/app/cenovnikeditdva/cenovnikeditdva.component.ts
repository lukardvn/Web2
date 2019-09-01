import { Component, OnInit } from '@angular/core';
import { CenovnikEdit } from '../modeli';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-cenovnikeditdva',
  templateUrl: './cenovnikeditdva.component.html',
  styleUrls: ['./cenovnikeditdva.component.css']
})
export class CenovnikeditdvaComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  cenovnik : CenovnikEdit;
  poruka : string;
  
  regularna : number;
  dnevna : number;
  mesecna : number;
  godisnja : number;

  ngOnInit() {
  }

  postCenovnik() {
    this.cenovnik = {
      CenaRegularna: this.regularna,
      CenaDnevna: this.dnevna,
      CenaMesecna: this.mesecna,
      CenaGodisnja: this.godisnja,
      ID: 0
    };
    this.http.PostCenovnik(this.cenovnik).subscribe((odg)=> {
      this.poruka = odg;
    });

  }
}
