import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { User } from '../modeli';
import { RegUser2 } from '../modeli';
import { AuthHttpService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { FormArray } from '@angular/forms';
import { Validators } from '@angular/forms';
import { Sifra } from '../modeli';

@Component({
  selector: 'app-profil',
  templateUrl: './profil.component.html',
  styleUrls: ['./profil.component.css']
})
export class ProfilComponent implements OnInit {

  constructor(private http: AuthHttpService) { }

  sifra : Sifra;
  old : string;
  new : string;
  conf : string;
  name : string;
  surname : string;
  tip2 : string;
  adress : string;
  date : string;
  user : RegUser2;
  tip : string;
  poruka : boolean = false;
  tipovi: string[] = [ "Obican", "Student", "Penzioner" ];
  file: File;
  imgURL: any;


  ngOnInit() {

    let jwtData = localStorage.jwt.split('.')[1];
    let decodedJwtJsonData = window.atob(jwtData);
    let decodedJwtData = JSON.parse(decodedJwtJsonData);

    this.http.GetProfileInfo(decodedJwtData.nameid).subscribe((user)=> {
      // this.user = {
      //   name: user.Name,
      //   surname: user.Surname,
      //   adress: user.Adress,
      //   tip: user.Tip,
      //   date: user.Date, 
      //   username: user.Username,
      //   password: user.Password,
      //   confirmPassword: user.ConfirmPassword,
      //   email: user.Email
      // }
      this.user = user as RegUser2;
      console.log(this.user);
      err => console.log(err);
  }
  );
      // this.name = this.user.name;
      // this.surname = this.user.surname;
      // this.adress = this.user.adress;
      // this.tip2 = this.user.tip;
      console.log(this.user.Name);
  }

  UploadPicture() {

    this.http.UploadPicture(this.file).subscribe();
  }

  Update() {
    this.user.Name = this.name;
    this.user.Surname = this.surname;
    this.user.Adress = this.adress;
    this.user.Tip = this.tip2;
    this.user.Date = this.date;

    this.http.UpdateProfil(this.user).subscribe();
  }

  onFileChanged(event) {
    this.file = event.target.files[0];

    var reader = new FileReader();
    reader.readAsDataURL(this.file); 
    reader.onload = (_event) => { 
      this.imgURL = reader.result; 
    }
  }


  Change() {
    this.sifra = {
      OldPassword : this.old,
      NewPassword : this.new,
      ConfirmPassword : this.conf
    }

    this.http.ChangePW(this.sifra).subscribe((odg) => {
      this.poruka = odg;
      if (this.poruka)
      {
        alert("Uspesno promenjena sifra!");
      }
    });
  }

}
