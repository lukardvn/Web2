import { Component, OnInit } from '@angular/core';
import { Kupac, Karta } from '../modeli';

@Component({
  selector: 'app-cenovnik',
  templateUrl: './cenovnik.component.html',
  styleUrls: ['./cenovnik.component.css']
})
export class CenovnikComponent implements OnInit {

  kupci = new Kupac();
  karte = new Karta();
  cena : number = 0;

  constructor() { }
  
  ngOnInit() {
  }

  

}
