export interface Message {
  id?: string;
  senderId: string;
  senderUsername?: string;
  displaySender?: string; // For UI display
  content: string;
  sentAt: Date;
  chatId?: string;
}