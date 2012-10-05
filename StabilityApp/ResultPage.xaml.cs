using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class ResultPage : PhoneApplicationPage
    {
        private string mode;
        private string result;
        private string time;

        public ResultPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //does a get-style request to check data that is passed
            NavigationContext.QueryString.TryGetValue("mode", out mode);
            NavigationContext.QueryString.TryGetValue("result", out result);
            NavigationContext.QueryString.TryGetValue("time", out time);

            if (result.Equals("True"))  //
            {
                this.Result_Text.Text = "Congratulations \n you are free to drive";
            }
            else
            {
                this.Result_Text.Text = "I'm sorry \n You are to drunk or tired to drive";
            }
        }
        private void main_menu_button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

    }
}