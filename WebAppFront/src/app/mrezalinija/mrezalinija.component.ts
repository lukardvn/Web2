import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';

@Component({
  selector: 'app-mrezalinija',
  templateUrl: './mrezalinija.component.html',
  styleUrls: ['./mrezalinija.component.css']
})
export class MrezalinijaComponent implements OnInit {

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

  latitude = 45.267136;
  longitude = 19.833549;

}
