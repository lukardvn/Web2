import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinijeeditdvaComponent } from './linijeeditdva.component';

describe('LinijeeditdvaComponent', () => {
  let component: LinijeeditdvaComponent;
  let fixture: ComponentFixture<LinijeeditdvaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinijeeditdvaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinijeeditdvaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
