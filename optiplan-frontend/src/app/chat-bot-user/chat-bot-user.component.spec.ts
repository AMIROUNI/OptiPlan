import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatBotUserComponent } from './chat-bot-user.component';

describe('ChatBotUserComponent', () => {
  let component: ChatBotUserComponent;
  let fixture: ComponentFixture<ChatBotUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChatBotUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatBotUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
