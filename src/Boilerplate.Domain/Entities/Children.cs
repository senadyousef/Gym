using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Boilerplate.Domain.Core.Entities;
using Boilerplate.Domain.Entities.Enums;

namespace Boilerplate.Domain.Entities
{
    public class Children : Entity
    {
        #region Parameters
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [MaxLength(50)]
        public string NameEn { get; set; } 
        [MaxLength(50)]
        public string NameAr { get; set; } 
        [Required]
        public string? Email { get; set; }  
        public string? Password { get; set; }  
        public string Role { get; set; } 
        public string Gender { get; set; } 
        public int Age { get; set; }
        public DateTime BOD { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public string MobilePhone { get; set; }  
        public PushToken PushToken { get; set; } 
        public string RefreshToken { get; set; } 
        public DateTime RefreshTokenExpiryTime { get; set; }  
        public string PhotoUri { get; set; }
        #endregion

        #region Relations 
        //public List<Events> Events { get; set; }
        //public List<UserEvents> UserEvents { get; set; }
        //public List<PersonalTrainersClasses> PersonalTrainerList { get; set; }
        //public List<PersonalTrainersClasses> TraineeLsit { get; set; }
        #endregion
    }
}

