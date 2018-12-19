using System;
using System.Collections.ObjectModel;
using System.Windows;
using Npgsql;

namespace RepTeam5_YelpApp
{
    class UserTab
    {
        private User loggedInUser;
        private ObservableCollection<Review> friendReviews;
        private ObservableCollection<User> friends;
        private MainWindow mainWindow;

        public UserTab() {
            mainWindow = (MainWindow)Application.Current.MainWindow;
            loggedInUser = null;
            mainWindow.grdFriendReviews.ItemsSource = friendReviews;
            mainWindow.grdFriendsList.ItemsSource = friends;
        }

        /// <summary>
        /// Find a list of user_ids matching the user name entered in the login box, and display them to the user
        /// </summary>
        public void SearchForUserID() {
            mainWindow.lstLoginUserID.Items.Clear();
            using (var connection = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString())) {
                connection.Open();
                using (var cmd = new NpgsqlCommand()) {
                    cmd.Connection = connection;
                    var searchName = mainWindow.txtLoginName.Text;
                    cmd.CommandText = "SELECT user_id,name FROM YelpUser WHERE name ILIKE '%" + searchName + "%'  ORDER BY user_id;";
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            mainWindow.lstLoginUserID.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to login using the selected user_id
        /// </summary>
        public void tryLogin() {
            string testPass = mainWindow.pwdLoginPassword.Password;

            using (var connection = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString())) {
                if (mainWindow.lstLoginUserID.SelectedItems.Count < 1) {
                    mainWindow.lblLoginMessage.Content = "Please Select a user ID";
                }
                else {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand()) {
                        cmd.Connection = connection;
                        var loginID = mainWindow.lstLoginUserID.SelectedItems[0].ToString();
                        cmd.CommandText = "SELECT * FROM YelpUser WHERE user_id = '" + loginID + "';";
                        using (var reader = cmd.ExecuteReader()) {
                            reader.Read();
                            if (!reader.IsDBNull(2)) {
                                string passwd = reader.GetString(2);

                                if (passwd == testPass) {
                                    Console.WriteLine("login successful");
                                    mainWindow.lblLoginMessage.Content = "";
                                    loggedInUser = new User(reader.GetString(0), reader.GetString(1));

                                    mainWindow.txtInfoName.Text = reader.GetString(1);
                                    mainWindow.txtInfoYelpSince.Text = reader.GetDate(6).ToString();
                                    mainWindow.txtInfoRevCount.Text = reader.GetInt32(3).ToString();
                                    mainWindow.txtInfoAvgStars.Text = reader.GetDouble(5).ToString();

                                }
                                else {
                                    mainWindow.lblLoginMessage.Content = "Login failed!";
                                    Logout();
                                }
                            }
                            else {
                                if (String.IsNullOrEmpty(testPass)) {
                                    Console.WriteLine("login successful");
                                    mainWindow.lblLoginMessage.Content = "";
                                    loggedInUser = new User(reader.GetString(0), reader.GetString(1));

                                    mainWindow.txtInfoName.Text = reader.GetString(1);
                                    mainWindow.txtInfoYelpSince.Text = reader.GetDate(6).ToString();
                                    mainWindow.txtInfoRevCount.Text = reader.GetInt32(3).ToString();
                                    mainWindow.txtInfoAvgStars.Text = reader.GetDouble(5).ToString();

                                }
                                else {
                                    mainWindow.lblLoginMessage.Content = "Login failed!";
                                    Logout();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// show the logged in user's votes
        /// </summary>
        public void populateVotes() {
            if (loggedInUser != null) {
                using (var connection = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString())) {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand()) {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT * FROM Votes WHERE user_id = '" + loggedInUser.userID +
                                          "'  ORDER BY user_id;";
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                mainWindow.txtInfoVoteCool.Text = reader.GetInt32(0).ToString();
                                mainWindow.txtInfoVoteFunny.Text = reader.GetInt32(0).ToString();
                                mainWindow.txtInfoVoteUseful.Text = reader.GetInt32(0).ToString();

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show the logged in user's friends list
        /// </summary>
        public void populateFriendsList() {
            if (loggedInUser != null) {
                using (var connection = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString())) {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand()) {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT friend_id FROM Friends WHERE user_id = '" + loggedInUser.userID +
                                          "'  ORDER BY user_id;";
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                loggedInUser.friends.Add(reader.GetString(0));

                            }
                        }

                        friends = new ObservableCollection<User>();
                        foreach (var friend_id in loggedInUser.friends) {
                            cmd.CommandText = "SELECT name,average_stars,yelping_since FROM YelpUser WHERE user_id = '" + friend_id +
                                              "' ORDER BY user_id;";
                            using (var reader = cmd.ExecuteReader()) {
                                while (reader.Read()) {
                                    friends.Add(new User() { name = reader.GetString(0), averageStars = (float)reader.GetDouble(1), yelpingSince = reader.GetDateTime(2) });
                                }
                            }
                        }

                        mainWindow.grdFriendsList.ItemsSource = friends;
                    }
                }
            }
        }

        /// <summary>
        /// show the logged in user's friend's reviews
        /// </summary>
        public void populateReviews() {
            if (loggedInUser != null) {
                using (var connection = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString())) {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand()) {
                        cmd.Connection = connection;

                        friendReviews = new ObservableCollection<Review>();
                        foreach (var user_id in loggedInUser.friends) {
                            cmd.CommandText = "SELECT DISTINCT Review.text, Review.date, Business.name, Business.city, YelpUser.name " +
                                              "FROM Review " +
                                              "LEFT JOIN Business ON Review.business_id = Business.business_id " +
                                              "LEFT JOIN YelpUser ON Review.user_id = YelpUser.user_id " +
                                              "WHERE Review.user_id = '" + user_id + "' ORDER BY Review.date DESC;";
                            using (var reader = cmd.ExecuteReader()) {
                                while (reader.Read()) {
                                    friendReviews.Add(new Review() {
                                        text = reader.GetString(0),
                                        date = reader.GetDateTime(1),
                                        bname = reader.GetString(2),
                                        city = reader.GetString(3),
                                        uname = reader.GetString(4)
                                    });
                                }
                            }
                        }
                        mainWindow.grdFriendReviews.ItemsSource = friendReviews;
                    }
                }
            }
        }

        /// <summary>
        /// update the password of the logged in user
        /// </summary>
        public void updatePassword() {
            if (loggedInUser != null) {
                using (var connection = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString())) {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand()) {
                        cmd.Connection = connection;

                        string password = null;
                        cmd.CommandText = "SELECT password " +
                                          "FROM YelpUser " +
                                          "WHERE user_id = '" + loggedInUser.name + "';";
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                password = reader.GetString(0);
                            }
                        }

                        if (password == null) password = "";
                        if (mainWindow.pwdCurrentPassword.Password == password &&
                            mainWindow.pwdNewPassword.Password == mainWindow.pwdRepeatPassword.Password) {
                            cmd.CommandText = "UPDATE YelpUser SET password = '" + mainWindow.pwdNewPassword.Password +
                                              "' WHERE user_id = '" + loggedInUser.userID + "';";
                            if (0 < cmd.ExecuteNonQuery())
                                mainWindow.lblLoginMessage.Content = "Password updated";
                        }

                    }
                }
            }

            mainWindow.pwdNewPassword.Password = "";
            mainWindow.pwdCurrentPassword.Password = "";
            mainWindow.pwdRepeatPassword.Password = "";
        }

        /// <summary>
        /// logout the current user, and clear data from the GUI
        /// </summary>
        public void Logout() {
            loggedInUser = null;
            mainWindow.lblLoginMessage.Content = "";
            mainWindow.txtInfoName.Text = "";
            mainWindow.txtInfoAvgStars.Text = "";
            mainWindow.txtInfoYelpSince.Text = "";
            mainWindow.txtInfoRevCount.Text = "";
            mainWindow.pwdNewPassword.Password = "";
            mainWindow.pwdRepeatPassword.Password = "";
            mainWindow.pwdCurrentPassword.Password = "";
            friendReviews = null;
            friends = null;
        }
    }
}
