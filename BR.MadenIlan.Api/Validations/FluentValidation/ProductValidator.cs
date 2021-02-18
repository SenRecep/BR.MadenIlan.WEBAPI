using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Api.Models;

using FluentValidation;

namespace BR.MadenIlan.Api.Validations.FluentValidation
{
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x=>x.CategoryId).ExclusiveBetween(0,int.MaxValue).WithMessage("Category is required");
            RuleFor(x=>x.Stock).ExclusiveBetween(0,int.MaxValue).WithMessage("Stock is required");
            RuleFor(x=>x.Price).ExclusiveBetween(0,decimal.MaxValue).WithMessage("Price is required");

        }
    }
}
