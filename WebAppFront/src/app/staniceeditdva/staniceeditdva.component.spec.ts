import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StaniceeditdvaComponent } from './staniceeditdva.component';

describe('StaniceeditdvaComponent', () => {
  let component: StaniceeditdvaComponent;
  let fixture: ComponentFixture<StaniceeditdvaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StaniceeditdvaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StaniceeditdvaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
