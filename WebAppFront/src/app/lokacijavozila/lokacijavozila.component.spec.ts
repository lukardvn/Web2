import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LokacijavozilaComponent } from './lokacijavozila.component';

describe('LokacijavozilaComponent', () => {
  let component: LokacijavozilaComponent;
  let fixture: ComponentFixture<LokacijavozilaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LokacijavozilaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LokacijavozilaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
