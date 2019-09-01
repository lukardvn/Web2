import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-staniceedit',
  templateUrl: './staniceedit.component.html',
  styleUrls: ['./staniceedit.component.css']
})
export class StaniceeditComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  stanice : number[];
  selectedStanica : number;
  poruka : string;

  ngOnInit() {
    this.http.GetStanice().subscribe((allStanice)=> {
      this.stanice = allStanice;
      err => console.log(err);
  }
  );
  }

  deleteStanica() {
    this.http.DeleteStation(this.selectedStanica).subscribe((odg)=> {
      this.poruka = odg;
    });
  }

}
