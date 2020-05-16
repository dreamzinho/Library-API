using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace libraryAPI.Entities
{
    public class User : IdentityUser
    {
        [JsonIgnore]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
