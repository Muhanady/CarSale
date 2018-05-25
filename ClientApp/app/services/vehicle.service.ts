import { VehiclesComponent } from './../components/vehicles/vehicles.component';
import { Vehicle, SaveVehicle } from './../components/models/vehicle.';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';


@Injectable()
export class VehicleService {

  private readonly vehiclesEndPoint = "/api/vehicle/";
  constructor(private http: HttpClient) { }

  getMakes() {
    return this.http.get("/api/makes");
  }
  getFeatures() {
    return this.http.get("/api/features");
  }
  create(vehicle: any) {

    return this.http.post(this.vehiclesEndPoint, vehicle);

  }
  update(vehicle: SaveVehicle) {
    return this.http.put(this.vehiclesEndPoint + vehicle.id, vehicle);
  }
  delete(id: number) {
    return this.http.delete(this.vehiclesEndPoint + id);
  }
  getVehicle(id: any) {

    return this.http.get(this.vehiclesEndPoint + id);
  }
  getVehicles(filter: any) {
    {

      return this.http.get("/api/vehicle?" + this.toQueryString(filter));
    }
  }
  toQueryString(obj: any) {
    var parts: any = [];
    Object.keys(obj).map((e: any) => {
      var value = obj[e];
      if (value != null || value != undefined) {
        parts.push(encodeURIComponent(e) + "=" + encodeURIComponent(obj[e]));
      }
    });
      return parts.join("&");
  }
}
