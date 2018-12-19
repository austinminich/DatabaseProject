using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepTeam5_YelpApp
{
    //In charge of calling queries, inserting, deleting, and updating onto the database
    //Also a singleton
    class DatabaseManager
    {
        private static DatabaseManager instance;
        private Npgsql.NpgsqlConnection db;

        public DatabaseManager()
        {

        }

        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DatabaseManager();
                return instance;
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    buildConnString                                     *
         *  Input:       None                                                *
         *  Output:      string                                              *
         *  Description: A function that returns a string.                   *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public string BuildConnString()
        {
            return "Host=localhost; Username=postgres; Password=123; Database=postgres";
        }   // End function

        public void InsertUser(User user)
        { }

        public void UpdateUser(User user)
        { }

        public void DeleteUser(User user)
        { }

        public void QueryUsers(string query, ref User[] users)
        { }

        public void InsertReview(Review review)
        { }

        public void UpdateReview(Review review)
        { }

        public void DeleteReview(Review review)
        { }

        public void QueryReview(string query, ref Review[] reviews)
        { }

        public void InsertBusiness(Business business)
        { }

        public void UpdateReview(Business business)
        { }

        public void DeleteReview(Business business)
        { }

        public void QueryBusiness(string query, ref Business[] businesses)
        { }

        public void InsertCheckin(string checkin)
        { }

        public void UpdateCheckin(string checkin)
        { }

        public void DeleteCheckin(string checkin)
        { }

        public void QueryCheckin(string query, ref Review[] reviews)//Dont think this is neccessary
        { }
    }//End class
}
