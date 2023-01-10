

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectFinalEngineer.Models.AggregateRole
{
    public class RoleModel : IdentityRole
    {
        public string[] Claims { get; set; }

    }
}
