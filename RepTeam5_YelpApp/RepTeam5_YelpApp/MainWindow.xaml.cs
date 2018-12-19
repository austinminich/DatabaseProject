using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Npgsql;
/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *  Project:     RepTeam5                                                    *
 *  Programmers: Austin Minich, Ian Mclerran, Jerome Santos                  *
 *  Date:        03/22/2018                                                  *
 *  Course:      CPTS451--Introduction to Database Systems                   *
 *  Term:        Spring 2018                                                 *
 *  Assignment:  Project Milestone 2                                         *
 *  Description: 
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
namespace RepTeam5_YelpApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User loggedInUser;
        private ObservableCollection<Review> friendReviews;
        private ObservableCollection<User> friends;
        private BusinessTab businessTab;

        private UserTab userTab;

        public MainWindow()
        {
            InitializeComponent();
            businessTab = new BusinessTab();    // Create instance of business tab
            businessTabItem.IsSelected = true;  // Select business tab
            businessTab.addColumnsToGrid();                 // Add columns to the grid
            //addColumnsToGrid();
            businessTab.addStates();                        // Populate the combobox with states
            loggedInUser = null;
            grdFriendReviews.ItemsSource = friendReviews;
            grdFriendsList.ItemsSource = friends;

            userTab = new UserTab();
        }   // End constructor

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  EVENTS                                                           *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       stateComboBox_SelectionChanged                      *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: A function that populates the citylistbox based on  *
         *               the state selected in the combobox.                 *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void stateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityListBox.Items.Clear();          // Clear all items before population
            zipcodeListBox.Items.Clear();       // Clear all items before population
            categoriesListBox.Items.Clear();    // Clear categories
            dayComboBox.Items.Clear();          // Clear the days
            fromComboBox.Items.Clear();         // Clear the hours
            toComboBox.Items.Clear();           // Clear the hours
            searchButton.IsEnabled = false;     // Disable search button

            if (stateComboBox.SelectedIndex > -1)   // Check if valid input
                businessTab.addCities();    // Add cities based upon state
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       cityListBox_SelectionChanged                        *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: A function that populates the zipcodeListBox based  *
         *               on the city selected in the listbox.                *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void cityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipcodeListBox.Items.Clear();       // Clear all items before population
            categoriesListBox.Items.Clear();    // Clear categories
            dayComboBox.Items.Clear();          // Clear the days
            fromComboBox.Items.Clear();         // Clear the hours
            toComboBox.Items.Clear();           // Clear the hours
            searchButton.IsEnabled = false;     // Disable search button

            if (cityListBox.SelectedIndex > -1) // Check if valid input
                businessTab.addZipcodes();    // Add cities based upon state
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       zipcodeListBox_SelectionChanged                     *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: A function that populates the zipcodeListBox based  *
         *               on the city selected in the listbox.                *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void zipcodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoriesListBox.Items.Clear();    // Clear the categories
            dayComboBox.Items.Clear();          // Clear the days
            fromComboBox.Items.Clear();         // Clear the hours
            toComboBox.Items.Clear();           // Clear the hours
            searchButton.IsEnabled = true;      // Re-enable button
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       searchButton_Click                                  *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: A function that populates the data grid with the    *
         *               given listbox selections.                           *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            resultsDataGrid.Items.Clear(); // Clear the items when choosing new state

            using (var conn = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT name, full_address, stars, review_count, num_checkins, longitude, latitude, business_id" +
                        " FROM business" +
                        " WHERE state = '" + stateComboBox.SelectedItem.ToString() +
                        "' AND city = '" + cityListBox.SelectedItem.ToString() +
                        "' AND zipcode = '" + zipcodeListBox.SelectedItem.ToString() + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add business to the datagrid
                            resultsDataGrid.Items.Add(new Business()
                            { name = reader.GetString(0),
                                full_address = reader.GetString(1),
                                stars = Convert.ToSingle(reader.GetValue(2)),
                                review_count = Convert.ToInt32(reader.GetString(3)),
                                num_checkins = Convert.ToInt32(reader.GetString(4)),
                                longitude = Convert.ToSingle(reader.GetValue(5)),
                                latitude = Convert.ToSingle(reader.GetValue(6)),
                                business_id = reader.GetString(7)
                            });
                        }
                    }
                }
                conn.Close();   // Close the connection
            }
            businessTab.addCategories();    // Fill in the categories based upon state, city, zip
            businessTab.addHours();         // Fill in the hours of the day
            searchButton.IsEnabled = false; // Disable button
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       categoriesListBox_SelectionChanged                  *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: A function that enables the add and remove button.  *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void categoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addButton.IsEnabled = true; // Re-enable the button
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       addButton_Click                                     *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: A function that adds category selection to listbox. *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            categoryListBox.IsEnabled = true;   // Enable the listbox
            removeButton.IsEnabled = true;      // Re-enable button
            businessTab.addCategoryToList();    // Call function to add to the listbox
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       removeButton_Click                                  *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: A function that removes category from listbox.      *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (categoryListBox.Items.Count > 0)    // Check if the list contains items
                businessTab.removeCategoryFromList();
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       viewMapButton_Click                                 *
         *  Input:       object, RoutedEventArgs                             *
         *  Output:      void                                                *
         *  Description: A event that displays the map window with           *
         *               the businesses matching the query will be displayed *
         *               on the map                                          *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void viewMapButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultsDataGrid.Items.Count > 0)
            {
                if (resultsDataGrid.SelectedItems.Count == 0)
                    PopupManager.Instance.ShowMapWindow(resultsDataGrid.Items);
                else
                    PopupManager.Instance.ShowMapWindow(resultsDataGrid.SelectedItems);//Error here with casting
            }
            else
                PopupManager.Instance.ShowErrorWindow("There are no businesses to view on the map.");
        }   //End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       resultsDataGrid_SelectionChanged                    *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: An event that is primarily used for updating the    *
         *               view checkin button.                                *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void resultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsDataGrid.SelectedItems.Count == 1)
            {
                showCheckinButton.IsEnabled = true;
                showTipsButton.IsEnabled = true;
            }
            else
            {
                showCheckinButton.IsEnabled = false;
                showTipsButton.IsEnabled = false;
            }
        }   //End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       resultsDataGrid_ResultsChanged                      *
         *  Input:       object, SelectionChangedEventArgs                   *
         *  Output:      void                                                *
         *  Description: An event that is primarily used for updating the    *
         *               viewMapButton.                                      *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
         private void resultsDataGrid_ResultsChanged(object sender, DependencyPropertyChangedEventArgs e)
         {
            if (resultsDataGrid.Items.Count > 0)
                viewMapButton.IsEnabled = true;
            else
                viewMapButton.IsEnabled = false;
         }  

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       showCheckinButton_Click                             *
         *  Input:       object, RoutedEventArgs                             *
         *  Output:      void                                                *
         *  Description: A event that displays the checkin window for a      *
         *               specific selected business.                         *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void showCheckinButton_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.Instance.ShowChartWindow(resultsDataGrid.SelectedItems);
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       dayComboBox_SelectionChanged                        *
         *  Input:       object, RoutedEventArgs                             *
         *  Output:      void                                                *
         *  Description: A event that updates the query                      *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void dayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dayComboBox.Items.Count != 0)
                businessTab.updateQuery();
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       fromComboBox_SelectionChanged                       *
         *  Input:       object, RoutedEventArgs                             *
         *  Output:      void                                                *
         *  Description: A event that updates the query                      *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void fromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fromComboBox.Items.Count != 0)
                businessTab.updateQuery();
        }   // End event

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       toComboBox_SelectionChanged                         *
         *  Input:       object, RoutedEventArgs                             *
         *  Output:      void                                                *
         *  Description: A event that updates the query                      *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void toComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (toComboBox.Items.Count != 0)
                businessTab.updateQuery();
        }   // End event
        

        private void btnLoginSearch_Click(object sender, RoutedEventArgs e)
        {
            userTab.SearchForUserID();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            userTab.Logout();
            userTab.tryLogin();
            userTab.populateFriendsList();
            userTab.populateReviews();
            userTab.populateVotes();
        }
        
        private void btnUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            userTab.updatePassword();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       showTipsButton_Click                                *
         *  Input:       object, RoutedEventArgs                             *
         *  Output:      void                                                *
         *  Description: A event that opens a new window the the             *
         *               corresponding business's tips.                      *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void showTipsButton_Click(object sender, RoutedEventArgs e)
        {
            PopupManager.Instance.ShowTipsWindow(resultsDataGrid.SelectedItems);
        }
    }   // End class
}
