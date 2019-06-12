import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RedvoznjeeditComponent } from './redvoznjeedit.component';

describe('RedvoznjeeditComponent', () => {
  let component: RedvoznjeeditComponent;
  let fixture: ComponentFixture<RedvoznjeeditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedvoznjeeditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedvoznjeeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
