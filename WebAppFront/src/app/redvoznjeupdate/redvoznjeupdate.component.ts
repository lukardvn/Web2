import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { rasporedUpdate } from '../modeli';

@Component({
  selector: 'app-redvoznjeupdate',
  templateUrl: './redvoznjeupdate.component.html',
  styleUrls: ['./redvoznjeupdate.component.css']
})
export class RedvoznjeupdateComponent implements OnInit {

  constructor(private http: AuthHttpService) { }
  linije : string[];
  selectedLinija : string;
  polasci : string;
  poruka : string;
  rasporedi : rasporedUpdate[];
  selected : rasporedUpdate;

  ngOnInit() {
    this.http.GetRedove2().subscribe((allLines)=> {
        this.rasporedi = allLines;
        err => console.log(err);
    }
    );

    this.http.GetLinije().subscribe((allLines)=> {
      this.linije = allLines;
      err => console.log(err);
    });
    
  }

  postDeparture() {
    this.selected.TimeTable = this.polasci;
    this.selected.TransportLineID = this.selectedLinija;
    this.http.UpdateDeparture(this.selected).subscribe((odg)=> {
      this.poruka = odg;
    });
    console.log(this.polasci);
  }

  onChange()
  {
    this.selectedLinija = this.selected.TransportLineID;
    this.polasci = this.selected.TimeTable;

  }

}