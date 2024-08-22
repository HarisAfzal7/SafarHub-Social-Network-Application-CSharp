using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaFarHub.Models
{
    public class FetchTaggedUsernames
    {
        private List<string> similerUsernames;

        public List<string> SimilerUsernames
        {
            get { return similerUsernames; }
            set { similerUsernames = value; }
        }

        public FetchTaggedUsernames()
        {
            similerUsernames = new List<string>();
        }
    }
}