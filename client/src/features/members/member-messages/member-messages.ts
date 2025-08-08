import { Component, effect, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { MessageService } from '../../../core/services/message-service';
import { MemberService } from '../../../core/services/member-service';
import { DatePipe } from '@angular/common';
import { TimeagoPipe } from '../../../core/pipes/timeago-pipe';
import { FormsModule } from '@angular/forms';
import { PresenceService } from '../../../core/services/presence-service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-messages',
  imports: [DatePipe, TimeagoPipe, FormsModule],
  templateUrl: './member-messages.html',
  styleUrl: './member-messages.css'
})
export class MemberMessages implements OnInit{
  @ViewChild('messageEndRef') messageEndRef!: ElementRef
  protected messageService = inject(MessageService);
  private memberService = inject(MemberService);
  protected presenceService = inject(PresenceService);
  private route = inject(ActivatedRoute);
  protected messageContent = '';

  constructor() {
    effect(() => {
      const currentMessages = this.messageService.messageThread();
      if (currentMessages.length > 0) {
        this.scrollToBottom();
      }
    })
  }

  ngOnInit(): void {
    this.route.parent?.paramMap.subscribe({
      next: params => {
        const otherUserId = params.get('id')
        if (!otherUserId) throw new Error('Cannot connect to hub');
        this.messageService.createHubConnection(otherUserId);
      }
    })
  }

  sendMessage() {
    const recipientId = this.memberService.member()?.id;
    if (!recipientId) return;
    this.messageService.sendMessage(recipientId, this.messageContent)?.then(() => {
      this.messageContent = '';
    })
  }

  scrollToBottom() {
    setTimeout(() => {
      if (this.messageEndRef){
        this.messageEndRef.nativeElement.scrollIntoView({behavior: 'smooth'})
      }
    })
  }
}
