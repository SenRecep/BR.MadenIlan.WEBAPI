using System.Collections.Generic;
using System.Linq;

using BR.MadenIlan.Shared.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.MadenIlan.Shared.ExtensionMethods
{
    public static class ActionContextExtensions
    {
        public static ErrorDto GetErrorDtoForModelState(this ActionContext context)
        {
            IEnumerable<string> errors = context.ModelState.Values
                   .Where(x => x.Errors?.Count > 0)
                   .SelectMany(x => x.Errors)
                   .Select(x => x.ErrorMessage);
            ErrorDto dto = new ErrorDto();
            dto.Errors.AddRange(errors);
            dto.StatusCode = StatusCodes.Status400BadRequest;
            dto.Path = context.HttpContext.Request.Path;
            dto.IsShow = true;
            return dto;
        }
        public static BadRequestObjectResult GetBadRequestResultErrorDtoForModelState(this ActionContext context)
        {
            return new BadRequestObjectResult(context.GetErrorDtoForModelState());
        }
    }
}
