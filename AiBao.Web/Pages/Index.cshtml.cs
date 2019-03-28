using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages
{
    public class IndexModel : PageModel
    {
        private ABDbContext _dbContext;
        public IndexModel(ABDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        { 
        }
    }
}
