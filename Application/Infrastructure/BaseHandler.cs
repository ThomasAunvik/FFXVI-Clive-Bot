using CliveBot.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Infrastructure
{
    public class BaseHandler
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IConfiguration _config;

        public BaseHandler(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
    }
}
