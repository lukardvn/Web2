import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CenovnikeditdvaComponent } from './cenovnikeditdva.component';

describe('CenovnikeditdvaComponent', () => {
  let component: CenovnikeditdvaComponent;
  let fixture: ComponentFixture<CenovnikeditdvaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CenovnikeditdvaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CenovnikeditdvaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
