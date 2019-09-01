import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StanicaupdateComponent } from './stanicaupdate.component';

describe('StanicaupdateComponent', () => {
  let component: StanicaupdateComponent;
  let fixture: ComponentFixture<StanicaupdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StanicaupdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StanicaupdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
