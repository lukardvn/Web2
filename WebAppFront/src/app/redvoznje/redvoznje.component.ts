import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { rasporedUpdate } from '../modeli';

@Component({
  selector: 'app-redvoznje',
  templateUrl: './redvoznje.component.html',
  styleUrls: ['./redvoznje.component.css']
})
export class RedvoznjeComponent implements OnInit {

  constructor(private http: AuthHttpService) { }
  linije : rasporedUpdate[];
  selectedLinija : rasporedUpdate;
  polasci : string;
  poruka : string;
  

  ngOnInit() {
    this.http.GetRedove2().subscribe((allLines)=> {
        this.linije = allLines;
        err => console.log(err);
    }
    );

  }

  onChange()
  {
    
    this.polasci = this.selectedLinija.TimeTable;

  }

}
