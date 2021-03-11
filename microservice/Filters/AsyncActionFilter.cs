using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using microservice.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace microservice.Filters
{
    public class AsyncActionFilter : IAsyncActionFilter
    {
        private AppDbContext db;

        public AsyncActionFilter(AppDbContext db)
        {
            this.db = db;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
        }
    }
}
