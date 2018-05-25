using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CarSale.Controllers.Resources;
using CarSale.Core.Models;
using CarSale.Persistence;
using CarSaleCore.Models;

namespace CarSale.Mapping {
    public class MappingProfile : Profile {
        public MappingProfile () {
            CreateMap (typeof (QueryResult<>), typeof (QueryResultResource<>));
            CreateMap<Make, MakeResource> ();
            CreateMap<Make, KeyValuePairResource> ();
            CreateMap<Model, KeyValuePairResource> ();
            CreateMap<Feature, KeyValuePairResource> ();
            CreateMap<Photo, PhotoResource> ();

            CreateMap<Vehicle, SaveVehicleResource> ()
                .ForMember (vr => vr.Contact, opt => opt.MapFrom (v => new ContactResource {
                    Name = v.ContactName, Phone = v.ContactEmail, Email = v.ContactPhone
                }))
                .ForMember (vr => vr.Features, opt => opt.MapFrom (v =>
                    v.Features.Select (vf => vf.FeatureId)));

            CreateMap<Vehicle, VehicleResource> ()
                .ForMember (vr => vr.Contact, opt => opt.MapFrom (v => new ContactResource {
                    Name = v.ContactName, Phone = v.ContactEmail, Email = v.ContactPhone
                }))
                .ForMember (vr => vr.Features, opt => opt.MapFrom (v =>
                    v.Features.Select (vf => new KeyValuePairResource { Id = vf.FeatureId, Name = vf.Feature.Name })))
                .ForMember (vr => vr.Make, opt => opt.MapFrom (v => v.Model.Make));
            // API Resource To domain
            CreateMap<VehicleQueryResource, VehicleQuery> ();
            CreateMap<SaveVehicleResource, Vehicle> ()
                .ForMember (v => v.Id, opt => opt.Ignore ())
                .ForMember (v => v.ContactName, opt => opt.MapFrom (vr => vr.Contact.Name))
                .ForMember (v => v.ContactPhone, opt => opt.MapFrom (vr => vr.Contact.Phone))
                .ForMember (v => v.ContactEmail, opt => opt.MapFrom (vr => vr.Contact.Email))
                .ForMember (v => v.Features, opt => opt.Ignore ())
                .AfterMap ((vr, v) => {

                    // remove Unselected Features
                    var removedFeatures = v.Features.Where (f => !vr.Features.Contains (f.FeatureId));
                    removedFeatures.ToList ().ForEach (r => {
                        v.Features.Remove (r);
                    });

                    // Add new Features
                    var addFeatures = vr.Features
                        .Where (id => !v.Features.Any (f => f.FeatureId == id))
                        .Select (id => new VehicleFeature { FeatureId = id });
                    addFeatures.ToList ().ForEach (f => {
                        v.Features.Add (f);
                    });
                });
        }
    }
}