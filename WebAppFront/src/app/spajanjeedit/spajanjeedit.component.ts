import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { StaniceNaLiniji } from '../modeli';
import { StaniceeditComponent } from '../staniceedit/staniceedit.component';

@Component({
  selector: 'app-spajanjeedit',
  templateUrl: './spajanjeedit.component.html',
  styleUrls: ['./spajanjeedit.component.css']
})
export class SpajanjeeditComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  linije : string[];
  selectedLinija : string;
  stanice : number[];
  selectedStanica : number;
  polasci : string;
  poruka : string;
  stanicice : StaniceNaLiniji;

  ngOnInit() {
    this.http.GetLinije().subscribe((allLines)=> {
      this.linije = allLines;
      err => console.log(err);
  }
  );

  this.http.GetStanice().subscribe((allStanice)=> {
    this.stanice = allStanice;
    err => console.log(err);
  }
  );
  }

  spoji() {
    console.log(this.selectedStanica);
    console.log(this.selectedLinija);
    this.stanicice = {
      stationID: this.selectedStanica,
      transportLineID: this.selectedLinija
    };
    // this.stanicice.transportLineID = this.selectedLinija;
    // this.stanicice.stationID = this.selectedStanica;
    this.http.DodajStanicuNaLiniju(this.stanicice).subscribe((odg)=> {
      this.poruka = odg;
    });
    console.log(this.polasci);
  }

}
