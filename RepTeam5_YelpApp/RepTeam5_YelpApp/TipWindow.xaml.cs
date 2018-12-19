using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RepTeam5_YelpApp
{
    /// <summary>
    /// Interaction logic for TipWindow.xaml
    /// </summary>
    public partial class TipWindow : Window
    {
        public Business Business;

        public TipWindow()
        {
            InitializeComponent();

        }

        public void AddTips()
        {
            using (var connection = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    ObservableCollection<Review> reviews = new ObservableCollection<Review>();
                    cmd.CommandText = "SELECT text, date, likes, user_id FROM Review "+
                        "WHERE business_id = '" + Business.business_id + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review()
                            {
                                text = reader.GetString(0),
                                date = reader.GetDateTime(1),
                                likes = Convert.ToInt32(reader.GetString(2)),
                                uname = reader.GetString(3)
                            });
                        }
                    }
                    tipsGrid.ItemsSource = reviews;
                }
            }
        }
    }
}
