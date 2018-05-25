import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private http: HttpClient) { }

  upload(vehicleId: number, photo: any) {
    var formData = new FormData();
    formData.append("file", photo);
    return this.http.post(`/api/vehicles/${vehicleId}/photos`, formData);
  }
  getPhotos(vehicleId: number) {
    return this.http.get(`/api/vehicles/${vehicleId}/photos`);
  }
}
