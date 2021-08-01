using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EatTogether.Models
{
    public class Session_ApplicationUser 
    {
        public int Id { get; set; }
        
        public string  ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }

        public int SessionId { get; set; }
        public SessionModel Session { get; set; }
    }
}