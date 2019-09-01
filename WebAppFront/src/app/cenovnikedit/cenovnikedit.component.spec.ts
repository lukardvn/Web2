import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CenovnikeditComponent } from './cenovnikedit.component';

describe('CenovnikeditComponent', () => {
  let component: CenovnikeditComponent;
  let fixture: ComponentFixture<CenovnikeditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CenovnikeditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CenovnikeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
