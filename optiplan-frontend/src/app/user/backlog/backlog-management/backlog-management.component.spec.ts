import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BacklogManagementComponent } from './backlog-management.component';

describe('BacklogManagementComponent', () => {
  let component: BacklogManagementComponent;
  let fixture: ComponentFixture<BacklogManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BacklogManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BacklogManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
