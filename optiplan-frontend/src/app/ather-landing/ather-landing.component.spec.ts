import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AtherLandingComponent } from './ather-landing.component';

describe('AtherLandingComponent', () => {
  let component: AtherLandingComponent;
  let fixture: ComponentFixture<AtherLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AtherLandingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AtherLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
