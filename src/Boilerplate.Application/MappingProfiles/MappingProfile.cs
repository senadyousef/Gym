using AutoMapper;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Application.Gallery;
using Boilerplate.Application.Interfaces; 
using Boilerplate.Domain.Entities;  

namespace Boilerplate.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        { 
            // User Map
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, GetUserExtendedDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdatePasswordDto, User>();
            
            // Events
            CreateMap<Events, GetEventsDto>().ReverseMap();

            // UserEvents
            CreateMap<UserEvents, GetUserEventsDto>().ReverseMap();
             
            // Personal Trainers Classes
            CreateMap<PersonalTrainersClasses, GetPersonalTrainersClassesDto>().ReverseMap();
             
            // News
            CreateMap<News, GetNewsDto>().ReverseMap();

            // AllServices
            CreateMap<Domain.Entities.AllServices, GetAllServicesDto>().ReverseMap(); 
            
            // UserAllServices
            CreateMap<Domain.Entities.UserAllServices, GetUserAllServicesDto>().ReverseMap();

            // Items
            CreateMap<Domain.Entities.Items, GetItemsDto>().ReverseMap(); 
            
            // Items
            CreateMap<Domain.Entities.Gallery, GetGalleryDto>().ReverseMap(); 
        }
    }
}
