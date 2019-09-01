import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { Validators } from '@angular/forms';
import { StanicaUpdate } from '../modeli';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-stanicaupdate',
  templateUrl: './stanicaupdate.component.html',
  styleUrls: ['./stanicaupdate.component.css']
})
export class StanicaupdateComponent implements OnInit {

  constructor(private http: AuthHttpService) { }
  name : string;
  adresa : string;
  x : number;
  y : number;
  poruka : string;
  stanice : StanicaUpdate[];
  selected : StanicaUpdate;

  ngOnInit() {
    this.http.GetStanice2().subscribe((allStanice)=> {
      this.stanice = allStanice as StanicaUpdate[];
      err => console.log(err);
      console.log(this.stanice);
 });}

  postStanica() {

      this.selected.Naziv = this.name;
      this.selected.Adresa = this.adresa;
      this.selected.X = this.x;
      this.selected.Y = this.y;
      
    this.http.UpdateStation(this.selected).subscribe((odg)=> {
      this.poruka = odg;
    });
  }

  onChange()
    {
      console.log(this.selected.Naziv);
      this.name = this.selected.Naziv;
      this.x = this.selected.X;
      this.y = this.selected.Y;
      this.adresa = this.selected.Adresa;

    }
}