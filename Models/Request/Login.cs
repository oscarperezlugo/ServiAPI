using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiAPI.Models.Request
{
    public class Login
    {
        public string User { get; set; }
        public string Pass { get; set; }
    }
}