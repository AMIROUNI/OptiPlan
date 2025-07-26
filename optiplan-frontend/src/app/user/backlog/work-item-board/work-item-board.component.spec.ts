import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemBoardComponent } from './work-item-board.component';

describe('WorkItemBoardComponent', () => {
  let component: WorkItemBoardComponent;
  let fixture: ComponentFixture<WorkItemBoardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkItemBoardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkItemBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
