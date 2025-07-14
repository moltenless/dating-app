import { Component, inject, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EditableMember, Member } from '../../../types/member';
import { DatePipe } from '@angular/common';
import { MemberService } from '../../../core/services/member-service';
import { NgForm } from '@angular/forms';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-member-profile',
  imports: [DatePipe],
  templateUrl: './member-profile.html',
  styleUrl: './member-profile.css'
})
export class MemberProfile implements OnInit, OnDestroy  {
  @ViewChild('editForm') editForm?: NgForm;
  protected memberService = inject(MemberService);
  private toast = inject(ToastService);
  private route = inject(ActivatedRoute);
  protected member = signal<Member | undefined>(undefined);
  protected editableMember?: EditableMember; 

  ngOnInit(): void {
    this.route.parent?.data.subscribe(data => {
      this.member.set(data['member']);
    })
    this.editableMember = {
      displayName: this.member()?.displayName || '',
      description: this.member()?.description || '',
      city: this.member()?.city || '',
      country: this.member()?.country || '',
    }
  }

  ngOnDestroy(): void {
    if (this.memberService.editMode()){
      this.memberService.editMode.set(false);
    }
  }

  updateProfile() {
    if (!this.member()) return;
    const updatedMember = {...this.member(), ...this.editableMember}
    console.log(updatedMember);
    this.toast.success('Profile updated successfully')
    this.memberService.editMode.set(false)
  }
}
