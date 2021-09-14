using BlazorFirstBlood.Server.Data;
using BlazorFirstBlood.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
    

namespace BlazorFirstBlood.Server.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UtilityService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<User> GetUser()
        {
            var userId = int.Parse(this.httpContextAccessor.HttpContext.User
                    .FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == 1);
            return user;
        }
    }
}
