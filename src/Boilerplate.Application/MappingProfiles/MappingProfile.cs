﻿using AutoMapper;
using Boilerplate.Application.DTOs.User;
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

            // Items
            CreateMap<Items, GetItemsDto>().ReverseMap();

            // Top Of Top
            CreateMap<TopOfTop, GetTopOfTopDto>().ReverseMap();

            // Item Photos
            CreateMap<ItemPhotos, GetItemPhotosDto>().ReverseMap();
            
            // Events
            CreateMap<Events, GetEventsDto>().ReverseMap();

            // UserEvents
            CreateMap<UserEvents, GetUserEventsDto>().ReverseMap();
            
            // Branches
            CreateMap<Branches, GetBranchesDto>().ReverseMap();

            // Personal Trainers Classes
            CreateMap<PersonalTrainersClasses, GetPersonalTrainersClassesDto>().ReverseMap();
            
            // Personal Trainers Classes
            CreateMap<Children, GetChildrenDto>().ReverseMap();
        }
    }
}
