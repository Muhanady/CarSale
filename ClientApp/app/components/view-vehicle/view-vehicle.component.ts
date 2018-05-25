import { ProgressService } from './../../services/progress.service';
import { PhotoService } from './../../services/photo.service';
import { ToastrService } from 'ngx-toastr';
import { Vehicle } from './../models/vehicle.';
import { VehicleService } from './../../services/vehicle.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {
  vehicleId: number = 0;
  vehicle: any;
  photos: any;
  @ViewChild("fileInput") fileInput!: ElementRef;
  constructor(private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private toastr: ToastrService,
    private photoService: PhotoService,
    private progressService: ProgressService) {

    route.params.subscribe(p => {
      this.vehicleId = +p["id"];
      if (isNaN(this.vehicleId) || this.vehicleId <= 0)
        this.router.navigate(['/vehicles']);
      return;
    });
  }

  ngOnInit() {
    this.vehicleService.getVehicle(this.vehicleId).subscribe(v =>
      this.vehicle = v,
      err => {
        if (err.status == 404) {
          this.router.navigate(['/vehicles']);
          return;
        }
      }
    );
    this.photoService.getPhotos(this.vehicleId).subscribe(s => this.photos = s);
  }
  uploadPhoto() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    this.progressService.createUploadProgress().subscribe((progress: any) => { console.log(progress) });
    this.photoService.upload(this.vehicleId, nativeElement.files![0]).subscribe(s => this.photos.push(s));

  }
  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id).subscribe(s => {
        this.toastr.success('Done', 'Vehicle Deleted');
        this.router.navigate(['/vehicles']);
      });
    }
  }
}
