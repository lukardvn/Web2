import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StaniceeditComponent } from './staniceedit.component';

describe('StaniceeditComponent', () => {
  let component: StaniceeditComponent;
  let fixture: ComponentFixture<StaniceeditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StaniceeditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StaniceeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
