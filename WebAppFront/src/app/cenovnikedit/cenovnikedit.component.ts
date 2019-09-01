import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-cenovnikedit',
  templateUrl: './cenovnikedit.component.html',
  styleUrls: ['./cenovnikedit.component.css']
})
export class CenovnikeditComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  cenovnici : number[];
  selected : number;
  poruka : string;

  ngOnInit() {
    this.http.GetPriceList().subscribe((allDep)=> {
      this.cenovnici = allDep;
      err => console.log(err);
  }
  );
  }

  deleteCenovnik() {
    this.http.DeletePriceList(this.selected).subscribe((odg)=> {
      this.poruka = odg;
    });
  }

}
