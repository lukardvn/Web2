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
  //@ViewChild('korisnici',{static: false}) kor : ElementRef;
  constructor(private http: AuthHttpService, private fb: FormBuilder) { }

  ngOnInit() {

    this.http.GetUsersToVerify().subscribe(data=>{
      this.korisnici = data;
      //console.log('data',data);
      //console.log('korisnici',this.korisnici);
      for(let k in data) {
        this.korisnici[k].Tip = this.GetUserType(data[k].UserType.TypeOfUser);
      }
      //console.log('korisnici',this.korisnici);
      this.imgURL = data[0].Files;
      this.korisnik = this.korisnici[0];
      //this.kor.nativeElement
      //console.log('korisnik ',this.korisnik);
      //console.log(this.GetUserType(data[0].UserType.TypeOfUser));
      //this.korisnik.Tip = this.GetUserType(data[0].UserType.UserTypeId);
      //console.log(this.korisnik.Tip);
      //console.log(data);
      //console.log(data[0].Files,this.imgURL);
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
      console.log(this.korisnici[k].Email, username);
      if(this.korisnici[k].Email == username)
      {
        console.log('true');
        return this.korisnici[k].Tip;
      }
    }
  }

  selectChange(){
    console.log('usao sam u selectChange napokon');
    this.korisnik = this.rf.get('korisnici').value;
    //this.korisnik.Tip = this.GetUserType(this.)
    //console.log('korisnik iz selectchange',this.korisnik);
    this.tip = this.UserType(this.korisnik);
    this.imgURL = this.korisnik.Files;
    //console.log(this.f.type.value);
  }

  Verify() {
    console.log('usao sam u verify');
    console.log(this.korisnik);
    this.http.VerifyUser(this.korisnik.Id).subscribe(data=>{
      //this.imgURL = data.Files;
      console.log(data);
      err=>console.log(data);
    });
    this.rf.reset();
  }

  Deny() {
    console.log('usao sam u deny');
    this.http.DenyUser(this.korisnik.Id).subscribe(data=>{
      console.log(data);
      err=>console.log(data);
    });
    this.rf.reset();
  }
}
