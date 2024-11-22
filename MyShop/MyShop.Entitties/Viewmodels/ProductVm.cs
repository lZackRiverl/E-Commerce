﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Viewmodels
{
    public class ProductVm
    {

        public Product product { get; set; }

        [ValidateNever]

        public IEnumerable<SelectListItem> CategoryList { get; set; }

    }
}
