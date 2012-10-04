using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace StabilityApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string local;
        private string culture;
        // Constructor
        public MainPage()
        {
            
            InitializeComponent();
        }

        private void LocList_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cognitiv_Test_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CognitivTest.xaml?mode=cognitiv", UriKind.Relative));
        }

        private void math_Test_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Mathproblem.xaml?mode=math", UriKind.Relative));
        }

        private void motion_Test_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MotionTest.xaml?mode=motion", UriKind.Relative));
        }

        private void calibrate_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Mathproblem.xaml?mode=calibrate", UriKind.Relative));
        }
    }
}