import { Component, OnInit } from '@angular/core';
import { linijica } from '../modeli';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-linijeeditdva',
  templateUrl: './linijeeditdva.component.html',
  styleUrls: ['./linijeeditdva.component.css']
})
export class LinijeeditdvaComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  linija : linijica;
  poruka : string;
  transportLine : string;
  fromTo : string;

  ngOnInit() {
  }

  postLine() {
    this.linija = {
      FromTo: this.fromTo,
      TransportLineID: this.transportLine
    };
    this.http.PostLine(this.linija).subscribe((odg)=> {
      this.poruka = odg;
    });

  }
}
