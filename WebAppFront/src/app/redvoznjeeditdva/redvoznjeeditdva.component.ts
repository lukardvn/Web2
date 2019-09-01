import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { raspored } from '../modeli';

@Component({
  selector: 'app-redvoznjeeditdva',
  templateUrl: './redvoznjeeditdva.component.html',
  styleUrls: ['./redvoznjeeditdva.component.css']
})
export class RedvoznjeeditdvaComponent implements OnInit {

  constructor(private http: AuthHttpService) { }
  linije : string[];
  selectedLinija : string;
  polasci : string;
  poruka : string;
  raspored : raspored;

  ngOnInit() {
    this.http.GetLinije().subscribe((allLines)=> {
        this.linije = allLines;
        err => console.log(err);
    }
    );
  }

  postDeparture() {
    this.raspored = {
      timeTable: this.polasci,
      transportLineID: this.selectedLinija
    };
    this.http.PostDeparture(this.raspored).subscribe((odg)=> {
      this.poruka = odg;
    });
    console.log(this.polasci);
  }

}
