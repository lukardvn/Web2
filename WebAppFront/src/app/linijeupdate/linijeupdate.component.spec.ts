import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinijeupdateComponent } from './linijeupdate.component';

describe('LinijeupdateComponent', () => {
  let component: LinijeupdateComponent;
  let fixture: ComponentFixture<LinijeupdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinijeupdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinijeupdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
