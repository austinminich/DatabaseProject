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
using Microsoft.Maps.MapControl.WPF;
using System.Collections;

namespace RepTeam5_YelpApp
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    { 
        public MapWindow()
        {
            InitializeComponent();
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    PutLocationsOnMap                                   *
         *  Input:       ItemCollection                                      *
         *  Output:      void                                                *
         *  Description: Attempts to put a collection of items' locations    *
         *  onto the map.                                                    *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public void PutLocationsOnMap(IList items)
        {
            List<Pushpin> pins = new List<Pushpin>();//Used for scaling initial map state
            foreach (var item in items)
            {
                //Need to make sure it's a business
                if(item.GetType().Name == "Business")
                {
                    Location loc = new Location(((Business)item).latitude, ((Business)item).longitude);
                    Pushpin pin = new Pushpin();
                    pin.Location = loc;
                    pins.Add(pin);//Add the new location to the list
                    myMap.Children.Add(pin);
                }
            }
            RescaleMap(pins);
        }//End function

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  Function:    RescaleMap                                          *
         *  Input:       List<Pushpin>                                       *
         *  Output:      void                                                *
         *  Description: Private function that rescales the map to encompass *
         *               all business locations when the window is opened.   *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void RescaleMap(List<Pushpin> pins)
        {
            double north = pins.Max(x => x.Location.Latitude);
            double south = pins.Min(x => x.Location.Latitude);
            double east = pins.Max(x => x.Location.Longitude);
            double west = pins.Min(x => x.Location.Longitude);
            LocationRect box = new LocationRect();
            if (pins.Count > 1)
            {
                box.North = north;
                box.West = west;
                box.South = south;
                box.East = east;
                myMap.SetView(box.Center, 15.0);//SetView(box) throws exception
            }
            else if (pins.Count == 1)//double check
            {
                box.North = north;
                box.West = west;
                box.South = south;
                box.East = east;
                myMap.SetView(box.Center, 15.0);
            }
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         *  event:       changeView_Click                                    *
         *  Input:       object, RoutedEventArgs                             *
         *  Output:      void                                                *
         *  Description: A event that changes the view of the map            *
         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        private void changeView_Click(object sender, RoutedEventArgs e)
        {
            if (myMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.RoadMode")
            {
                //Set the map mode to Aerial with labels
                myMap.Mode = new AerialMode(true);
            }
            else if (myMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.AerialMode")
            {
                //Set the map mode to RoadMode
                myMap.Mode = new RoadMode();
            }
        }//End event
    }
}
