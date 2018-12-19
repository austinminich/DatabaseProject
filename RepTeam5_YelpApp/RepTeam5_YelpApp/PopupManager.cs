using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RepTeam5_YelpApp
{
    //Singleton class dealing with the popup windows
    public class PopupManager
    {
        private static PopupManager instance;
        private CheckinChartWindow checkinChartWindow;
        private MapWindow mapWindow;
        private ErrorWindow errorWindow;
        private TipWindow tipWindow;

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    Instance()                                          *
         *  Input:       None                                                *
         *  Output:      PopupManager                                        *
         *  Description: Returns the singleton class instance of this class. *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public static PopupManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PopupManager();
                return instance;
            }
        }//End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    ShowMapWindow()                                     *
         *  Input:       ItemCollection                                      *
         *  Output:      void                                                *
         *  Description: Based on the given businesses as a ItemCollection,  *
         *               creates a map with their corresponding locations on *
         *               the map.                                            *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void ShowMapWindow(IList items)
        {
            //Checks to see if the window has not been made or has been closed
            if(mapWindow == null || !mapWindow.IsLoaded)
                mapWindow = new MapWindow();
            mapWindow.PutLocationsOnMap(items);

            mapWindow.ShowDialog();
        }//End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    ShowChartWindow()                                   *
         *  Input:       Business                                            *
         *  Output:      void                                                *
         *  Description: Based on the given business, shows a weekly checkin *
         *               chart showing how many checkins per day.            *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void ShowChartWindow(IList items)//---------------------------------EDIT THE PARAMETERS HERE to show checkins for a specific business
        {
            //Checks to see if the window has not been made or has been closed
            if (checkinChartWindow == null || !checkinChartWindow.IsLoaded)
                checkinChartWindow = new CheckinChartWindow();
            checkinChartWindow.Business = (Business)items[0];//This should always be 1
            checkinChartWindow.EnterDataToChart();

            checkinChartWindow.ShowDialog();
        }//End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    ShowErrorWindow()                                   *
         *  Input:       string                                              *
         *  Output:      void                                                *
         *  Description: Displays a window with the given string to present  *
         *               to the user as to what the error is.                *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void ShowErrorWindow(string error)
        {
            //Checks to see if the window has not been made or has been closed
            if (errorWindow == null || !errorWindow.IsLoaded)
                errorWindow = new ErrorWindow(error);

            errorWindow.ShowDialog();
        }//End function

        internal void ShowTipsWindow(IList items)
        {
            if (tipWindow == null || !tipWindow.IsLoaded)
                tipWindow = new TipWindow();
            tipWindow.Business = (Business)items[0];
            tipWindow.AddTips();

            tipWindow.ShowDialog();
        }
    }//End class
}
