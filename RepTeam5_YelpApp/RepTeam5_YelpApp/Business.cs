using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepTeam5_YelpApp
{
    public class Business
    {
        public string business_id { get; set; }
        public string full_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public bool open { get; set; }
        public int review_count { get; set; }
        public string name { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public float stars { get; set; }
        public int num_checkins { get; set; }
    }   // End class
}
