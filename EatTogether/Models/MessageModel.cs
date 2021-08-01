﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatTogether.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime MessageTime { get; set; }
    }
}