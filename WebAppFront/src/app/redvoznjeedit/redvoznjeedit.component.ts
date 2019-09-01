import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-redvoznjeedit',
  templateUrl: './redvoznjeedit.component.html',
  styleUrls: ['./redvoznjeedit.component.css']
})
export class RedvoznjeeditComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  redovi : number[];
  selectedRed : number;
  poruka : string;

  ngOnInit() {
    this.http.GetRedove().subscribe((allDep)=> {
      this.redovi = allDep;
      err => console.log(err);
  }
  );
  }

  deleteRed() {
    this.http.DeleteRed(this.selectedRed).subscribe((odg)=> {
      this.poruka = odg;
    });
  }

}
