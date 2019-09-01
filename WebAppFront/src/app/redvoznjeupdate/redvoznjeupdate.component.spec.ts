import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RedvoznjeupdateComponent } from './redvoznjeupdate.component';

describe('RedvoznjeupdateComponent', () => {
  let component: RedvoznjeupdateComponent;
  let fixture: ComponentFixture<RedvoznjeupdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RedvoznjeupdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RedvoznjeupdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
