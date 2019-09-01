import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-lokacijavozila',
  templateUrl: './lokacijavozila.component.html',
  styleUrls: ['./lokacijavozila.component.css']
})
export class LokacijavozilaComponent implements OnInit {


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
}