import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RedvoznjeeditdvaComponent } from './redvoznjeeditdva.component';

describe('RedvoznjeeditdvaComponent', () => {
  let component: RedvoznjeeditdvaComponent;
  let fixture: ComponentFixture<RedvoznjeeditdvaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedvoznjeeditdvaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedvoznjeeditdvaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
