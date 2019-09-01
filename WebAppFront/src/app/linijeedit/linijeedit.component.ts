import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-linijeedit',
  templateUrl: './linijeedit.component.html',
  styleUrls: ['./linijeedit.component.css']
})
export class LinijeeditComponent implements OnInit {

 
  constructor(private http: AuthHttpService) { }

  linije : number[];
  selectedLinija : number;
  poruka : string;

  ngOnInit() {
    this.http.GetLinije().subscribe((allLines)=> {
      this.linije = allLines;
      err => console.log(err);
  }
  );
  }

  deleteLinija() {
    this.http.DeleteLinija(this.selectedLinija).subscribe((odg)=> {
      this.poruka = odg;
    });
  }

}
