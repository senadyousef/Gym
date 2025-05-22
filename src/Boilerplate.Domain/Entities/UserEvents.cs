﻿using Boilerplate.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities
{
    public class UserEvents : Entity
    {
        #region Parameters
        [Required] 
        public int UserId { get; set; }

        [Required] 
        public int EventsId { get; set; }
         
        #endregion

        #region Relations
        #endregion
    }
}
