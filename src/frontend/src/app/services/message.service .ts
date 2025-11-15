import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NotificationMessage } from '../models/notificationMessage.model';


@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private messages: NotificationMessage[] = [];
  private nextId = 1;

  // BehaviorSubject to emit messages whenever they change
  private messagesSubject = new BehaviorSubject<NotificationMessage[]>(this.messages);

  // Observable that components can subscribe to
  messages$ = this.messagesSubject.asObservable();

  setMessage(type: string, message: string): number {
    const id = this.nextId++;
    this.messages.push({ id, type, message });
    // Emit the updated list of messages
    this.messagesSubject.next(this.messages);
    return id;
  }

  getMessages(): NotificationMessage[] {
    return this.messages;
  }

  deleteMessage(id: number): void {
    this.messages = this.messages.filter((msg) => msg.id !== id);

    // Emit the updated list of messages
    this.messagesSubject.next(this.messages);
  }

  clearMessages(): void {
    this.messages = [];

    // Emit the updated list of messages
    this.messagesSubject.next(this.messages);
  }
}
