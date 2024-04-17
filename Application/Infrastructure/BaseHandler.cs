using CliveBot.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Infrastructure
{
    public class BaseHandler(ApplicationDbContext context, IConfiguration config)
    {
        protected readonly ApplicationDbContext _context = context;
        protected readonly IConfiguration _config = config;
    }
}
