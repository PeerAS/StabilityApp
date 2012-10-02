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
using System.Windows.Threading;

namespace StabilityApp
{
    public partial class CognitivTest : PhoneApplicationPage
    {
        private int[] combination;
        private int[] user_input;
        private string time_stamp;
        private string mode;
        private int _current_input;
        private const int MAX_INPUT = 4;


        private Random rand;
        DateTime timer;
        private TimeSpan time_start;

        public CognitivTest()
        {
            _current_input = 0;
            combination = new int[4];
            user_input = new int[4];
            rand = new Random();
            timer = DateTime.Now;
            time_start = new TimeSpan(0, 0, 3);

            for (int i = 0; i < MAX_INPUT; i++)
            {
                combination[i] = rand.Next(4) ;
            }

                InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationContext.QueryString.TryGetValue("mode", out mode);
            display_combination(); //must be moved since it starts running before page is displayed
        }

        private void display_combination()
        {
            TimeSpan delay = new TimeSpan(0,0,1);
            for (int i = 0; i < MAX_INPUT; i++)
            {
                switch(combination[i])
                {
                    case 0: button_flash(this.Up_Button_Animation, time_start); break;
                    case 1: button_flash(this.Down_Button_Animaton, time_start); break;
                    case 2: button_flash(this.Left_Button_Animation, time_start); break;
                    case 3: button_flash(this.Right_Button_Animation, time_start); break;                    
                }

               time_start = time_start.Add(delay);
                
               
            }
        }

        private int Current_Input
        {
            get
            {
                return _current_input;
            }
            set
            {
                _current_input = value;
                if (_current_input == MAX_INPUT)
                    display_result();
            }
        }
        
        #region Button Click

        private void Up_Button_Click(object sender, RoutedEventArgs e)
        {
                user_input[_current_input] = 0;
                Current_Input = Current_Input + 1;
        }

        private void Down_Button_Click(object sender, RoutedEventArgs e)
        {
                user_input[_current_input] = 1;
                Current_Input = Current_Input + 1;
        }

        private void Left_Button_Click(object sender, RoutedEventArgs e)
        {
                user_input[_current_input] = 2;
                Current_Input = Current_Input + 1;
        }

        private void Right_Button_Click(object sender, RoutedEventArgs e)
        {
                user_input[_current_input] = 3;
                Current_Input = Current_Input + 1;
        }

        #endregion

        void display_result()
        {
            
            DateTime time_end = DateTime.Now;
            TimeSpan difference = time_end.Subtract(timer);
            time_stamp = difference.ToString("c");
            bool combination_error = true;

            for (int i = 0; i < MAX_INPUT; i++)
            {
                if (user_input[i] != combination[i])
                {
                    combination_error = false;
                    break;
                }
            }

            if (mode.Equals("calibrate") && combination_error == true)
            {
                this.NavigationService.Navigate(new Uri("/MotionTest.xaml?mode=" + mode, UriKind.Relative));
            }
            else if (mode.Equals("calibrate") && combination_error == false)
            {
                MessageBox.Show("Wrong combination input. Please try again");
                
                this.NavigationService.Navigate(new Uri("/CognitivTest.xaml?mode=" + mode, UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/ResultPage.xaml?mode=" + mode + "&result=" + combination_error, UriKind.Relative));
            }
            //this will be moved to the next page
            //if (combination_error)
            //{
            //    MessageBox.Show("Correct combination The timer is " + time_stamp);
            //}
            //else
            //{
            //    MessageBox.Show("Wrong combination");
            //}
        }

        void button_flash(Storyboard sb, TimeSpan begin)
        {   
            sb.BeginTime = begin;
            sb.Begin();
        }
    }
}