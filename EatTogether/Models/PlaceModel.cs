using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EatTogether.Models
{
    public class PlaceModel
    {
        public int Id { get; set; }

        [DisplayName("Venue")]
        public string Name { get; set; }
        public int Point { get; set; }
        public string Info { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
    }
}