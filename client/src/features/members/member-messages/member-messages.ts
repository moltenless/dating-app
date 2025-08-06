import { Component, inject, OnInit, signal } from '@angular/core';
import { MessageService } from '../../../core/services/message-service';
import { MemberService } from '../../../core/services/member-service';
import { Message } from '../../../types/message';
import { DatePipe } from '@angular/common';
import { TimeagoPipe } from '../../../core/pipes/timeago-pipe';
import { FormsModule } from '@angular/forms';
import { subscribeOn } from 'rxjs';

@Component({
  selector: 'app-member-messages',
  imports: [DatePipe, TimeagoPipe, FormsModule],
  templateUrl: './member-messages.html',
  styleUrl: './member-messages.css'
})
export class MemberMessages implements OnInit{
  private messageService = inject(MessageService);
  private memberService = inject(MemberService);
  protected messages = signal<Message[]>([]);
  protected messageContent = '';

  ngOnInit(): void {
    this.loadMessages();;
  }
  
  loadMessages() {
    const memberId = this.memberService.member()?.id;
    if (memberId) {
      this.messageService.getMessageThread(memberId).subscribe({
        next: messages => this.messages.set(messages.map(message => ({
          ...message,
          currentUserSender: message.senderId !== memberId
        })))
      })
    }
  }

  sendMessage() {
    const recipientId = this.memberService.member()?.id;
    if (!recipientId) return;
    this.messageService.sendMessage(recipientId, this.messageContent).subscribe({
      next: message => {
        this.messages.update(messages => {
          message.currentUserSender = true;
          return [...messages, message]
        });
        this.messageContent = '';
      }
    })
  }
}
