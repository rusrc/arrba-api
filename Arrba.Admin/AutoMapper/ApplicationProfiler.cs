using Arrba.Admin.ViewModels;
using Arrba.Domain.Models;
using Arrba.ImageLibrary.ModelViews;
using AutoMapper;

namespace Arrba.Admin.AutoMapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            ValidateInlineMaps = false;

            CreateMap<CategType, TypeWithCategory>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(m => m.Categ.Name));

            CreateMap<User, UserModelEdit>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(m => m.Id));

            CreateMap<AdVehicle, AdVehicleCreateEdit>();
        }
    }
}
