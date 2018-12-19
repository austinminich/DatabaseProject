using Npgsql;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CheckinChartWindow.xaml
    /// </summary>
    public partial class CheckinChartWindow : Window
    {
        public Business Business;

        public CheckinChartWindow()
        {
            InitializeComponent();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    EnterDataToChart                                    *
         *  Input:       Business                                            *
         *  Output:      void                                                *
         *  Description: Populates the window's chart with the corresponding *
         *  chart data.                                                      *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void EnterDataToChart()
        {
            //Simulate data for checkins
            List<KeyValuePair<string, int>> myChartData = new List<KeyValuePair<string, int>>();
            QueryCheckinData(myChartData);
            myChart.DataContext = myChartData;
            myChart.Title = "" + Business.name + " weekly checkins";
        }
        
        private void QueryCheckinData(List<KeyValuePair<string, int>> myChartData)
        {
            using (var conn = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT hour_day, amount FROM Checkin WHERE business_id = '" +
                        this.Business.business_id + "';";//Query string
                    int sun = 0, mon = 0, tue = 0, wed = 0, thu = 0, fri = 0, sat = 0;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Put the correct amount of checkins to the correct day
                            if (reader.GetString(0).Split('-')[1] == "0")//Sunday
                                sun += Convert.ToInt32(reader.GetString(1));
                            else if (reader.GetString(0).Split('-')[1] == "1")//Monday
                                mon += Convert.ToInt32(reader.GetString(1));
                            else if (reader.GetString(0).Split('-')[1] == "2")//Tuesday
                                tue += Convert.ToInt32(reader.GetString(1));
                            else if (reader.GetString(0).Split('-')[1] == "3")//Wednesday
                                wed += Convert.ToInt32(reader.GetString(1));
                            else if (reader.GetString(0).Split('-')[1] == "4")//Thursday
                                thu += Convert.ToInt32(reader.GetString(1));
                            else if (reader.GetString(0).Split('-')[1] == "5")//Friday
                                fri += Convert.ToInt32(reader.GetString(1));
                            else if (reader.GetString(0).Split('-')[1] == "6")//Saturday
                                sat += Convert.ToInt32(reader.GetString(1));
                        }
                    }
                    myChartData.Add(new KeyValuePair<string, int>("Sun", sun));
                    myChartData.Add(new KeyValuePair<string, int>("Mon", mon));
                    myChartData.Add(new KeyValuePair<string, int>("Tue", tue));
                    myChartData.Add(new KeyValuePair<string, int>("Wed", wed));
                    myChartData.Add(new KeyValuePair<string, int>("Thu", thu));
                    myChartData.Add(new KeyValuePair<string, int>("Fri", fri));
                    myChartData.Add(new KeyValuePair<string, int>("Sat", sat));
                }
                conn.Close();   // Close the connection
            }
        }
    }
}
