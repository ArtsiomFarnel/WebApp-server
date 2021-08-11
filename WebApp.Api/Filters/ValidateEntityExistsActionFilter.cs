using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Interfaces;
using WebApp.Data.Entities;
using WebApp.Infrastructure;

namespace WebApp.Api.Filters
{
    public class ValidateEntityExistsActionFilter<T> : IActionFilter where T : class, IEntityBase
    { 
        private readonly DatabaseContext _context;
        private readonly ILoggerManager _logger;

        public ValidateEntityExistsActionFilter(DatabaseContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int? id = null;
            if (context.ActionArguments.ContainsKey("id"))
                id = (int?)context.ActionArguments["id"];
            else
            {
                _logger.LogError("Bad id parameter");
                context.Result = new BadRequestObjectResult("Bad id parameter");
                return;
            }

            if (id == null)
            {
                _logger.LogError("Id parameter was null");
                context.Result = new BadRequestObjectResult("Id parameter was null");
                return;
            }

            var entity = _context.Set<T>().SingleOrDefault(x => x.Id == id);
            if (entity == null)
            {
                _logger.LogError("No entity found");
                context.Result = new NotFoundResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
