import { Vehicle, KeyValuePair } from './../models/vehicle.';
import { VehicleService } from './../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-vehicles',
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.css']
})
export class VehiclesComponent implements OnInit {


  constructor(private vehicleService: VehicleService) { }
  vehicles: Vehicle[] = [];
  makes: KeyValuePair[] = [];
  query: any = {};
  columns = [
    { title: "Id" },
    { title: "Make", key: 'make', isSortable: true },
    { title: "Model", key: 'model', isSortable: true },
    { title: "Contact Name", key: 'contactName', isSortable: true },
    {}

  ]


  ngOnInit() {
    this.vehicleService.getMakes().subscribe((m: any) => this.makes = m);
    this.populateVehicles();
  }
  private populateVehicles() {
    this.vehicleService.getVehicles(this.query).subscribe((s: any) => this.vehicles = s);
  }
  onFilterChanged() {
    this.populateVehicles();

  }
  restFilter() {
    this.query = {};
    this.onFilterChanged();
  }
  sortBy(str: string) {
    if (this.query.sortingBy === str) {
      this.query.isSortingAscending = !this.query.isSortingAscending;
    }
    else {
      this.query.sortingBy = str;
      this.query.isSortingAscending = "true";
    }

    this.populateVehicles();

  }
}
