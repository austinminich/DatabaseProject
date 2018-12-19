using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepTeam5_YelpApp
{
    public class User
    {
        public float averageStars { get; set; }
        public Dictionary<string, int> compliments;
        public List<int> elite;
        public int numOfFans;
        public List<User> fanOf;
        public List<string> friends;
        public string name { get; set; }
        public int reviewCount;
        public string userID;
        public Dictionary<int, string> votes;
        public DateTime yelpingSince { get; set; }
        public string password;
        public List<Business> ownedBusinesses;
        public List<Review> reviews;

        public User()
        {
            friends = new List<string>();
        }

        public User(string user_id, string name)
        {
            this.name = name;
            this.userID = user_id;
            friends = new List<string>();
        }


        /*public void AddFriend(User user)
        {
            if (!friends.Contains(user))
                friends.Add(user);
        }*/

        /*public void RemoveFriend(User user)
        {
            if (friends.Contains(user))
                friends.Remove(user);
        }*/

        public void AddCompliment(string compliment) => this.compliments[compliment]++;
        public void AddFan() => numOfFans++;

        public void UpdateReviewCount()
        {
            //Gets the number of reviews by this user and updates reviewCount?
        }

    }//End class
}
