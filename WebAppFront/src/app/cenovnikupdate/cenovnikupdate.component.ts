import { Component, OnInit } from '@angular/core';
import { CenovnikEdit } from '../modeli';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-cenovnikupdate',
  templateUrl: './cenovnikupdate.component.html',
  styleUrls: ['./cenovnikupdate.component.css']
})
export class CenovnikupdateComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  cenovnik : CenovnikEdit;
  tempStr : string;
  poruka : string;

  ngOnInit() {
    this.http.GetTrenutniCenovnik().subscribe((allLines)=> {
      this.cenovnik = allLines as CenovnikEdit;
      err => console.log(err);
      }  
    );
  }

    postCenovnik()
    { 
      this.http.UpdateCenovnik(this.cenovnik).subscribe();
    }
   
}
