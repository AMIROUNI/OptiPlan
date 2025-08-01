import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayProjectComponent } from './display-project.component';

describe('DisplayProjectComponent', () => {
  let component: DisplayProjectComponent;
  let fixture: ComponentFixture<DisplayProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DisplayProjectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DisplayProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
