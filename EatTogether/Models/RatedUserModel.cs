using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatTogether.Models
{
    public class RatedUserModel
    {
        public int Id { get; set; }
        
        public string UserRatedId { get; set; }
        public ApplicationUser UserRated { get; set; }
        public string UserRatesId{ get; set; }
        public ApplicationUser UserRates { get; set; }
    }
}