import * as _ from 'underscore';
import { SaveVehicle, Vehicle } from './../models/vehicle.';
import { VehicleService } from './../../services/vehicle.service';
import { MakeService } from './../../services/make.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { forkJoin } from "rxjs/observable/forkJoin";



@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any = [];
  models: any = [];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: "",
      email: "",
      phone: ""
    }
  };
  features: any = [];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private toastr: ToastrService) {
    route.params.subscribe(p => {
      this.vehicle.id = +p["id"];
    });

  }

  ngOnInit() {
    var sources = [
      this.vehicleService.getFeatures(),
      this.vehicleService.getMakes()];
    if (this.vehicle.id)
      sources.push(this.vehicleService.getVehicle(this.vehicle.id))
    Observable.forkJoin(sources).subscribe(data => {
      this.features = data[0];
      this.makes = data[1];
      if (this.vehicle.id) {
        this.setVehicle(data[2] as Vehicle)
        this.populateModels();
      }
    }, error => {
      if (error.status == 404) {
        this.router.navigate(['/home']);
      }
    });
  }
  private setVehicle(v: Vehicle) {
    this.vehicle.modelId = v.model.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.features = _.pluck(v.features, 'id');
    this.vehicle.contact = v.contact;
  }
  onMakeChange(makeId: number) {
    this.populateModels();
    delete this.vehicle.modelId;
  }
  private populateModels() {
    var selectedVehicle = this.makes.find((f: any) => f.id == this.vehicle.makeId);
    this.models = selectedVehicle ? selectedVehicle.models : [];
  }
  onFeatureToggle(featureId: any, $event: any) {
    if ($event.target.checked) {
      this.vehicle.features.push(featureId);
    }
    else {
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }
  submit() {
    if (this.vehicle.id) {
      this.vehicleService.update(this.vehicle).subscribe(s => { this.toastr.success('Done', 'Vehicle Updated') },
        err => { this.toastr.error('Error', 'Some thing went wrong') });
    }
    else {
      this.vehicleService.create(this.vehicle).subscribe(s =>
        this.toastr.success('Done', 'Vehicle Saved'));
    }
  }
  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id).subscribe(s=>{
        this.router.navigate(['/home']);
      });
    }
    
  }
}
