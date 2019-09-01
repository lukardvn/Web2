import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { Validators } from '@angular/forms';
import { Stanica } from '../modeli';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-staniceeditdva',
  templateUrl: './staniceeditdva.component.html',
  styleUrls: ['./staniceeditdva.component.css']
})
export class StaniceeditdvaComponent implements OnInit {

  constructor(private http: AuthHttpService) { }
  name : string;
  adresa : string;
  x : number;
  y : number;
  poruka : string;
  stanica : Stanica;

  ngOnInit() {

 }

  postStanica() {
    this.stanica = {
      naziv : this.name,
      adresa : this.adresa,
      x : this.x,
      y : this.y
    }
    this.http.DodajStanicu(this.stanica).subscribe((odg)=> {
      this.poruka = odg;
    });
  }
}
