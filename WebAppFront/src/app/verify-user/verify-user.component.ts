import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthHttpService } from '../services/auth.service';
import { RegUser2 } from '../modeli';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-verify-user',
  templateUrl: './verify-user.component.html',
  styleUrls: ['./verify-user.component.css']
})
export class VerifyUserComponent implements OnInit {

  korisnici: RegUser2[];
  korisnik: RegUser2;
  tip: any;
  imgURL: any;
  rf = this.fb.group({
    korisnici : ['']
  });
  constructor(private http: AuthHttpService, private fb: FormBuilder) { }

  ngOnInit() {

    this.http.GetUsersToVerify().subscribe(data=>{
      this.korisnici = data;
      console.log(data);
      
      for(let k in data) {
        this.korisnici[k].Tip = this.GetUserType(data[k].UserType.TypeOfUser);
      }
      
      //this.imgURL = `http://localhost:52295/imgs/users/${this.korisnici[0].Id}/${data[0].Files}`;
      this.korisnik = this.korisnici[0];

      err=> console.log(err);
    });
  }

  get f() { return this.rf.controls; }

  GetUserType(userTypeId: number) {
    switch(userTypeId){
      case 0: return "regularan";
      case 1: return "student";
      case 2: return "penzioner";
    }
  }

  UserType(username: any) {
    for(let k in this.korisnici){
      if(this.korisnici[k].Email == username)
      {
        return this.korisnici[k].Tip;
      }
    }
  }

  selectChange(){
    //this.korisnik = this.rf.get('korisnici').value;
    //console.log('this.korisnik iz selectchange',this.korisnik);
    this.korisnik = this.korisnici.find(a=>a.Email == this.rf.get('korisnici').value);
    console.log(this.korisnik);
    this.tip = this.UserType(this.korisnik.Email);
    this.imgURL = `http://localhost:52295/imgs/users/${this.korisnik.Id}/${this.korisnik.Files}`;
  }

  Verify() {
    this.http.VerifyUser(this.korisnik.Id).subscribe(data=>{
      console.log(data);
      this.imgURL = null;
      this.tip = null;
      err=>console.log(data);
    });
    this.http.GetUsersToVerify().subscribe(data=>{
      this.korisnici = data;
      console.log(data);
      
      for(let k in data) {
        this.korisnici[k].Tip = this.GetUserType(data[k].UserType.TypeOfUser);
      }
      
      //this.imgURL = `http://localhost:52295/imgs/users/${this.korisnici[0].Id}/${data[0].Files}`;
      this.korisnik = this.korisnici[0];

      err=> console.log(err);
    });
    this.rf.reset();
  }

  Deny() {
    this.http.DenyUser(this.korisnik.Id).subscribe(data=>{
      console.log(data);
      this.imgURL = null;
      this.tip = null;
      err=>console.log(data);
    });
    this.http.GetUsersToVerify().subscribe(data=>{
      this.korisnici = data;
      console.log(data);
      
      for(let k in data) {
        this.korisnici[k].Tip = this.GetUserType(data[k].UserType.TypeOfUser);
      }
      
      //this.imgURL = `http://localhost:52295/imgs/users/${this.korisnici[0].Id}/${data[0].Files}`;
      this.korisnik = this.korisnici[0];

      err=> console.log(err);
    });
    this.rf.reset();
  }
}
