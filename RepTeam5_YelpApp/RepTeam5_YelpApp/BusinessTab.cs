using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Npgsql;

namespace RepTeam5_YelpApp
{
    public class BusinessTab
    {
        private MainWindow mainWindow;

        public BusinessTab()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }   // End constructor

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    addColumnsToGrid                                    *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that creates three columns for the       *
         *               datagrid, sets the header, width, and binds them to *
         *               corresponding database columns.                     *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void addColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn(); // Create new column
            col1.Header = "Business Name";                      // Create new column header
            col1.Binding = new Binding("name");                 // Binding
            col1.Width = 256;                                   // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col1);                  // Add column to data grid

            DataGridTextColumn col2 = new DataGridTextColumn(); // Create new column
            col2.Header = "Address";                            // Create new column header
            col2.Binding = new Binding("full_address");         // Binding
            col2.Width = 256;                                   // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col2);                  // Add column to data grid

            DataGridTextColumn col3 = new DataGridTextColumn(); // Create new column
            col3.Header = "Stars";                            // Create new column header
            col3.Binding = new Binding("stars");         // Binding
            col3.Width = 64;                                   // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col3);                  // Add column to data grid

            DataGridTextColumn col4 = new DataGridTextColumn(); // Create new column
            col4.Header = "# Of Reviews";                       // Create new column header
            col4.Binding = new Binding("review_count");         // Binding
            col4.Width = 96;                                    // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col4);                  // Add column to data grid

            DataGridTextColumn col5 = new DataGridTextColumn(); // Create new column
            col5.Header = "# Of Tips";                          // Create new column header
            col5.Binding = new Binding("num_tips");             // Binding
            col5.Width = 64;                                    // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col5);                  // Add column to data grid

            DataGridTextColumn col6 = new DataGridTextColumn(); // Create new column
            col6.Header = "Total Checkins";                     // Create new column header
            col6.Binding = new Binding("num_checkins");         // Binding
            col6.Width = 96;                                    // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col6);                  // Add column to data grid

            DataGridTextColumn col7 = new DataGridTextColumn(); // Create new column
            col7.Header = "Longitude";                          // Create new column header
            col7.Binding = new Binding("longitude");            // Binding
            col7.Width = 192;                                   // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col7);                  // Add column to data grid

            DataGridTextColumn col8 = new DataGridTextColumn(); // Create new column
            col8.Header = "Latitude";                           // Create new column header
            col8.Binding = new Binding("latitude");             // Binding
            col8.Width = 192;                                   // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col8);                  // Add column to data grid

            DataGridTextColumn col9 = new DataGridTextColumn(); // Create new column
            col9.Header = "Business ID";                        // Create new column header
            col9.Binding = new Binding("business_id");          // Binding
            col9.Width = 192;                                   // Set column width
            mainWindow.resultsDataGrid.Columns.Add(col9);                  // Add column to data grid
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    addStates                                           *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that connects to a database and populates*
         *               the state combobox using an SQL query.              *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void addStates()
        {
            mainWindow.stateComboBox.Items.Clear();    // Clear all items before population
            using (var conn = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT state FROM business ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add the state to the combobox
                            mainWindow.stateComboBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();   // Close the connection
            }
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    addCities                                           *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that connects to a database and populates*
         *               the city listbox using an SQL query.                *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void addCities()
        {
            mainWindow.cityListBox.Items.Clear();      // Clear all items before populating
            using (var conn = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT city" +
                        " FROM business WHERE state = '" + mainWindow.stateComboBox.SelectedItem.ToString() +
                        "' ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add the city to the combobox
                            mainWindow.cityListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();   // Close the connection
            }
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    addZipcodes                                         *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that connects to a database and populates*
         *               the zipcodes listbox using an SQL query.            *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void addZipcodes()
        {
            mainWindow.zipcodeListBox.Items.Clear();       // Clear all items before populating
            using (var conn = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT zipcode" +
                        " FROM business" +
                        " WHERE state = '" + mainWindow.stateComboBox.SelectedItem.ToString() +
                        "' AND city = '" + mainWindow.cityListBox.SelectedItem.ToString() +
                        "' ORDER BY zipcode;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add the city to the combobox
                            mainWindow.zipcodeListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();   // Close the connection
            }
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    addCategories                                       *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that connects to a database and populates*
         *               the categories listbox using an SQL query.          *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void addCategories()
        {
            mainWindow.categoriesListBox.Items.Clear();    // Clear all items before populating
            using (var conn = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT text " +
                        " FROM business, categories " +
                        " WHERE business.business_id = categories.business_id" +
                        " AND state = '" + mainWindow.stateComboBox.SelectedItem.ToString() +
                        "' AND city = '" + mainWindow.cityListBox.SelectedItem.ToString() +
                        "' AND zipcode = '" + mainWindow.zipcodeListBox.SelectedItem.ToString() + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add the city to the combobox
                            mainWindow.categoriesListBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();   // Close the connection
            }
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    addHours                                            *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that connects to a database and populates*
         *               the hours comboboxes.                               *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void addHours()
        {
            mainWindow.dayComboBox.Items.Clear();  // Clear all items before populating
            mainWindow.fromComboBox.Items.Clear(); // Clear all items before populating
            mainWindow.toComboBox.Items.Clear();   // Clear all items before populating

            // Fill in the days
            mainWindow.dayComboBox.Items.Add("Sunday");
            mainWindow.dayComboBox.Items.Add("Monday");
            mainWindow.dayComboBox.Items.Add("Tuesday");
            mainWindow.dayComboBox.Items.Add("Wednesday");
            mainWindow.dayComboBox.Items.Add("Thursday");
            mainWindow.dayComboBox.Items.Add("Friday");
            mainWindow.dayComboBox.Items.Add("Saturday");

            // Fill in the hours
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    mainWindow.fromComboBox.Items.Add("0" + i + ":00");
                    mainWindow.toComboBox.Items.Add("0" + i + ":00");
                }
                else
                {
                    mainWindow.fromComboBox.Items.Add(i + ":00");
                    mainWindow.toComboBox.Items.Add(i + ":00");
                }
            }
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    addCategoryToList                                   *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that adds a category to the list.        *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void addCategoryToList()
        {
            if (!mainWindow.categoryListBox.Items.Contains(mainWindow.categoriesListBox.SelectedItem))
            {
                mainWindow.categoryListBox.Items.Add(mainWindow.categoriesListBox.SelectedItem);    // Adds selection to listbox
                updateQuery();  // Revise the results datagrid
            }
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    removeCategoryFromList                              *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that removes a category from list.       *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void removeCategoryFromList()
        {
            mainWindow.categoryListBox.Items.Remove(mainWindow.categoryListBox.SelectedItem);   // Remove selection from list
            updateQuery();
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    buildQueryString                                    *
         *  Input:       none                                                *
         *  Output:      string                                              *
         *  Description: A function returns a query string.                  *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public string buildQueryString()
        {
            string str = "";    // Start with an empty string
            if (mainWindow.categoryListBox.Items.Count > 0)
            {
                str += " INNER JOIN (SELECT business_id" +
                    " FROM categories" +
                    " WHERE categories.text = '" + mainWindow.categoryListBox.Items.GetItemAt(0) + "'";

                int i = 1;
                while (i < mainWindow.categoryListBox.Items.Count)
                {
                    str += " INTERSECT SELECT business_id" +
                        " FROM categories" +
                        " WHERE categories.text = '" + mainWindow.categoryListBox.Items.GetItemAt(i) + "'";
                    i++;
                }
                str += ") AS tempTable ON business.business_id = tempTable.business_id"; // End of the string
            }
            return str;
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    buildHoursString                                    *
         *  Input:       none                                                *
         *  Output:      string                                              *
         *  Description: A function returns a query string.                  *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public string buildHoursString()
        {
            string str = "";    // Start with an empty string
            if (mainWindow.dayComboBox.SelectedIndex > -1) // Check if an item is selected
            {
                str += " AND hours.day = '" + mainWindow.dayComboBox.SelectedItem + "'";
            }
            if (mainWindow.fromComboBox.SelectedIndex > -1)
            {
                str += " AND hours.open <= '" + mainWindow.fromComboBox.SelectedItem + "'";
            }
            if (mainWindow.toComboBox.SelectedIndex > -1)
            {
                str += " AND hours.close > '" + mainWindow.toComboBox.SelectedItem + "'";
            }
            return str;
        }   // End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    updateQuery                                         *
         *  Input:       none                                                *
         *  Output:      void                                                *
         *  Description: A function that queries the mainWindow.resultsDataGrid based on*
         *               the added categories.                               *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void updateQuery()
        {
            mainWindow.resultsDataGrid.Items.Clear(); // Clear the items when choosing new state
            string categories = buildQueryString();
            using (var conn = new NpgsqlConnection(DatabaseManager.Instance.BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT name, full_address, stars, review_count, num_checkins, longitude, latitude, business.business_id" +
                        " FROM hours, business" + categories +
                        " WHERE hours.business_id = business.business_id" +
                        " AND state = '" + mainWindow.stateComboBox.SelectedItem.ToString() +
                        "' AND city = '" + mainWindow.cityListBox.SelectedItem.ToString() +
                        "' AND zipcode = '" + mainWindow.zipcodeListBox.SelectedItem.ToString() + "'" +
                        buildHoursString() + ";";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Add business to the datagrid
                            mainWindow.resultsDataGrid.Items.Add(new Business()
                            {
                                name = reader.GetString(0),
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
            mainWindow.searchButton.IsEnabled = false; // Disable button
        }   // End function
    }   // End class
}
