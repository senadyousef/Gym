﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Boilerplate.Domain.Core.Entities;
using Boilerplate.Domain.Entities.Enums;

namespace Boilerplate.Domain.Entities
{
    public class User : Entity
    {
        #region Parameters
        [MaxLength(50)]
        public string NameEn { get; set; } 
        [MaxLength(50)]
        public string NameAr { get; set; } 
        [Required]
        public string Email { get; set; } 
        [Required]
        public string Password { get; set; }  
        public string MobilePhone { get; set; }
        public string? Gender { get; set; }
        public string Role { get; set; } 
        public int? Age { get; set; }
        public DateTime? DOB { get; set; }
        public string PhotoUri { get; set; }
        public int GymId { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public PushToken PushToken { get; set; } 
        public string RefreshToken { get; set; } 
        public DateTime RefreshTokenExpiryTime { get; set; } 
        public bool IsInGym { get; set; }
        #endregion

        #region Relations 
        public List<Gallery> Gallery { get; set; }
        public List<Events> Events { get; set; }
        public List<Items> Items { get; set; }
        public List<News> News { get; set; }
        public List<UserAllServices> UserAllServices { get; set; }
        public List<UserEvents> UserEvents { get; set; }
        public List<PersonalTrainersClasses> PersonalTrainerList { get; set; }
        public List<PersonalTrainersClasses> TraineeLsit { get; set; } 
        #endregion
    }
}

