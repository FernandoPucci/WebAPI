using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ResourcesServer.Models
{
    public class UserAccess
    {

        public UserAccess() {

            Id = Guid.NewGuid();
        }

        [Key]
        [JsonIgnore]
        public Guid Id
        {get; set;}
        [JsonIgnore]
        public string User { get; set; }
        public string UserName { get; set; }
        public string PermissionRole { get; set; }
        public string Operation { get; set; }
        public Boolean IsGranted { get; set;  }
        public DateTime AccessDate { get; set; }


        public class UserAccessesContext : DbContext
        {
            public UserAccessesContext()
                    : base("name=DefaultConnection")
            {
            }
            public DbSet<UserAccess> UserAccesses { get; set; }
        }


    }
}