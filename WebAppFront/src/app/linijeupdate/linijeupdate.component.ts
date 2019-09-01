import { Component, OnInit } from '@angular/core';
import { linijica } from '../modeli';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-linijeupdate',
  templateUrl: './linijeupdate.component.html',
  styleUrls: ['./linijeupdate.component.css']
})
export class LinijeupdateComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  linije : linijica[];
  selectedLinija : linijica;
  i : number; 
  tempStr : string;
  poruka : string;

  ngOnInit() {
    this.http.GetLinije2().subscribe((allLines)=> {
      this.linije = allLines as linijica[];
      this.i = 0;
      console.log(allLines[this.i].FromTo);
      this.selectedLinija = this.linije[this.i];
      console.log(this.selectedLinija);
      err => console.log(err);
      }  
    );
  }

    postLine(str : string)
    { 
      this.selectedLinija.FromTo = str;
      console.log(this.selectedLinija);
      this.http.UpdateLine(this.selectedLinija).subscribe();
    }
    onChange()
    {
      this.tempStr = this.selectedLinija.FromTo;
    }
}
