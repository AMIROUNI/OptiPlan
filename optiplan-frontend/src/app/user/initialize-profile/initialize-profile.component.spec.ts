import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitializeProfileComponent } from './initialize-profile.component';

describe('InitializeProfileComponent', () => {
  let component: InitializeProfileComponent;
  let fixture: ComponentFixture<InitializeProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InitializeProfileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InitializeProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
