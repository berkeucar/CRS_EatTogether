using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EatTogether.Models
{
    public class SessionModel
    {
        public int Id { get; set; }
        [Required]
        [RangeAttribute(2,100)]
        [DisplayName("Maximum Quota")]
        public int UserCount { get; set; }
        [Required]
        [Remote("IsValidDate", "Validation", areaReference: AreaReference.UseRoot, HttpMethod = "POST", ErrorMessage = "Please enter a date that is valid.")]
        public DateTime Date { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ApplicationUser User { get; set; }

        // field for adding a place model to the session
        [Required]
        public string City { get; set; }
        public virtual PlaceModel Place { get; set; }
        public string PlaceName { get; set;}

        // session users
        [DisplayName("Remaining Quota")]
        public int SpaceRemaining { get; set; }
        public List<Session_ApplicationUser> SessionUserTable { get; set; }

    }
}