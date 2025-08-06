import { Component, inject, OnInit, signal } from '@angular/core';
import { MessageService } from '../../core/services/message-service';
import { PaginatedResult } from '../../types/pagination';
import { Message } from '../../types/message';
import { Paginator } from "../../shared/paginator/paginator";

@Component({
  selector: 'app-messages',
  imports: [Paginator],
  templateUrl: './messages.html',
  styleUrl: './messages.css'
})
export class Messages implements OnInit {
  private messageService = inject(MessageService);
  protected container = 'Inbox';
  protected pageNumber = 1;
  protected pageSize = 10;
  protected paginatedMessages = signal<PaginatedResult<Message> | null>(null);

  tabs = [
    {label: 'Inbox', value: 'Inbox'},
    {label: 'Outbox', value: 'Outbox'}
  ]

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messageService.getMessages(this.container, this.pageNumber, this.pageSize).
      subscribe({
        next: response => this.paginatedMessages.set(response)
      });
  }

  get isInbox() {
    return this.container === 'Inbox';
  }

  setContainer(container: string){
    this.container = container;
    this.pageNumber = 1;
    this.loadMessages();
  }

  onPageChange(event: {pageNumber: number, pageSize: number}) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageNumber;
    this.loadMessages();
  }
}
