import { Component, inject, OnInit, signal } from '@angular/core';
import { Photo } from '../../../types/member';
import { AdminService } from '../../../core/services/admin-service';

@Component({
  selector: 'app-photo-management',
  imports: [],
  templateUrl: './photo-management.html',
  styleUrl: './photo-management.css'
})
export class PhotoManagement implements OnInit{
  adminService = inject(AdminService);
  photos = signal<Photo[]>([]);

  ngOnInit(): void {
    this.adminService.getPhotosForModeration().subscribe({
      next: photos => {
        this.photos.set(photos);
      }
    })
  }

  moderatePhoto(photoId: number, approve: boolean){
    this.adminService.moderatePhoto(photoId, approve).subscribe({
      next: _ => {
        this.photos.update(p => p.filter(p => p.id != photoId));
      },
      error: error => {
        console.log(error);
      }
    })
  }
}
