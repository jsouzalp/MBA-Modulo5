import {
  Component,
  Output,
  EventEmitter,
  Input,
  ViewEncapsulation,
  OnInit,
} from '@angular/core';
import { MaterialModule } from 'src/app/material.module';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { MatButtonModule } from '@angular/material/button';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { MessageService } from 'src/app/services/message.service ';
import { NotificationMessage } from 'src/app/models/notificationMessage.model';
import { MatDialog } from '@angular/material/dialog';
import { MessagesComponent } from 'src/app/pages/messages/messages.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule, NgScrollbarModule, MaterialModule, MatButtonModule],
  templateUrl: './header.component.html',
  encapsulation: ViewEncapsulation.None,
})

export class HeaderComponent implements OnInit {
  @Input() showToggle = true;
  @Input() toggleChecked = false;
  @Output() toggleMobileNav = new EventEmitter<void>();
  @Output() toggleCollapsed = new EventEmitter<void>();
  email: string;
  messages: NotificationMessage[] = [];
  private subscription!: Subscription;
  hasMessage: boolean = false;
  messageCounter: number = 0;

  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private router: Router, private localStorageUtils: LocalStorageUtils, private messageService: MessageService, public dialog: MatDialog,) { }
  ngOnInit(): void {
    this.email = this.localStorageUtils.getEmail();

    this.subscription = this.messageService.messages$.subscribe(
      (messages) => {
        this.messages = messages;
        this.hasMessage = messages.length > 0;
        this.messageCounter = messages.length;
      }
    );
  }

  deleteMessage(id: number): void {
    this.messageService.deleteMessage(id);
  }

  clearAllMessages(): void {
    this.messageService.clearMessages();
  }

  getMessageCount(): number {
    return this.messages.length;
  }

  openMessages() {
    this.dialog.open(MessagesComponent, {
      width: '1070px',
      height: '600px',
      disableClose: true
    });


  }

  logout() {

    this.localStorageUtils.clear();
    this.router.navigate(['/login']);

  }
}
