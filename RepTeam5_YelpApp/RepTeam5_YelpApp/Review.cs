using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepTeam5_YelpApp
{
    public class Review
    {
        private User user;
        public string uname { get; set; }
        public string bname { get; set; }
        public string city { get; set; }
        public string text { get; set; }
        private Business business;
        public int likes;
        public DateTime date { get; set; }

        public void AddLike() { likes++; }
    }//End class
}
