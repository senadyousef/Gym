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
        [Required]
        public string Role { get; set; } 
        public string Gender { get; set; } 
        public int Age { get; set; }  
        public string MobilePhone { get; set; }  
        public PushToken PushToken { get; set; } 
        public string RefreshToken { get; set; } 
        public DateTime RefreshTokenExpiryTime { get; set; } 
        public string PhotoUri { get; set; }  
        #endregion

        #region Relations 
        #endregion
    }
}

