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
    public partial class Mathproblem : PhoneApplicationPage
    {
        private string[] mathProblems;
        private string[] mathSolutions;
        private string userSolution;
        private string mode;
        private Random randomNumber;
        private int problemNumber;
        private bool result;
        

        public Mathproblem()
        {
            mathProblems = new string[10];
            mathSolutions = new string[10];
            randomNumber = new Random();
            problemNumber = randomNumber.Next(10);

            mathProblems[problemNumber] = "1+1";
            mathSolutions[problemNumber] = "2";

            InitializeComponent();

            displayProblem();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationContext.QueryString.TryGetValue("mode", out mode);

            if (mode.Equals("calibrate"))   //this checks if we are calibrating or not
            {                               //if we are calibrating the button saying continue will show, if not the button saying submit will show
                submit_Button.Visibility = Visibility.Collapsed;
                continue_Button.Visibility = Visibility.Visible;
            }
            else
            {
                submit_Button.Visibility = Visibility.Visible;
                continue_Button.Visibility = Visibility.Collapsed;
            }
        }

        private void displayProblem()
        {
            this.MathProblem.Text = mathProblems[problemNumber];    //displays the mathproblem/equation
        }

        private void submit_Button_Click(object sender, RoutedEventArgs e)
        {
            
            userSolution = this.Solution.Text;

            result = userSolution.Equals(mathSolutions[problemNumber]);
            if (result)
            {
                this.NavigationService.Navigate(new Uri("/ResultPage.xaml?mode=" + mode + "&result=" + result, UriKind.Relative));
                MessageBox.Show("You were correct " + problemNumber);
            }
            else
            {
                MessageBox.Show("You are to drunk for this");
            }
        }

        private void continue_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CognitivTest.xaml?mode=calibrate", UriKind.Relative));
        }

    }
}