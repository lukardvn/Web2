import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinijeeditComponent } from './linijeedit.component';

describe('LinijeeditComponent', () => {
  let component: LinijeeditComponent;
  let fixture: ComponentFixture<LinijeeditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinijeeditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinijeeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
