import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CenovnikupdateComponent } from './cenovnikupdate.component';

describe('CenovnikupdateComponent', () => {
  let component: CenovnikupdateComponent;
  let fixture: ComponentFixture<CenovnikupdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CenovnikupdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CenovnikupdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
