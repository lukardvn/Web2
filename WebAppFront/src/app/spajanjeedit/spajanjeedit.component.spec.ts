import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpajanjeeditComponent } from './spajanjeedit.component';

describe('SpajanjeeditComponent', () => {
  let component: SpajanjeeditComponent;
  let fixture: ComponentFixture<SpajanjeeditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpajanjeeditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpajanjeeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
