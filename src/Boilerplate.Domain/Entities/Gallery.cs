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
    public class Gallery : Entity
    {
        #region Parameters   
        public string PhotoUri { get; set; }
        #endregion

        #region Relations
        #endregion
    }
}
