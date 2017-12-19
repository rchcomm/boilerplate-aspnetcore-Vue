using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vue2Spa.Models
{
    public static class DbInitializer
    {
        public static void Initialize(HttpFileContext context)
        {
            var isEnsulreCreated = context.Database.EnsureCreated();
            if (isEnsulreCreated)
            {
                // db operations
            }

            context.SaveChanges();
        }
    }
}
