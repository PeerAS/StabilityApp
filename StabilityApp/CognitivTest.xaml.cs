﻿using System;
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
        DateTime timer = DateTime.Now;

        int[] combination;
        int[] user_input;
        string time_stamp;
        int current_input = 0;
        private Random rand;
        private const int MAX_INPUT = 4;
        
        public CognitivTest()
        {
            
            combination = new int[4];
            user_input = new int[4];
            rand = new Random();

            for (int i = 0; i < MAX_INPUT; i++)
            {
                combination[i] = rand.Next(4) ;
            }

                InitializeComponent();
        }

        #region Button Click
        
        private void Up_Button_Click(object sender, RoutedEventArgs e)
        {
           
            if(current_input <= MAX_INPUT)
            { 
                user_input[current_input] = 0;
                current_input++;
                display_result();
            }
        }

        private void Down_Button_Click(object sender, RoutedEventArgs e)
        {
            if (current_input <= MAX_INPUT)
            {
                user_input[current_input] = 1;
                current_input++;
                display_result();
            }
        }

        private void Left_Button_Click(object sender, RoutedEventArgs e)
        {
            if (current_input <= MAX_INPUT)
            {
                user_input[current_input] = 2;
                current_input++;
                display_result();
            }
        }

        private void Right_Button_Click(object sender, RoutedEventArgs e)
        {
            if (current_input <= MAX_INPUT)
            {
                user_input[current_input] = 3;
                current_input++;
                display_result();
            }

        }

        #endregion

        void display_result()
        {
            //will move alot of this to the a result page
            DateTime time_end = DateTime.Now;
            TimeSpan difference = time_end.Subtract(timer);
            time_stamp = difference.ToString();
            bool combination_error = true;
            if (current_input == 4)
            {
                for (int i = 0; i < MAX_INPUT; i++)
                {
                    if (user_input[i] != combination[i])
                    {
                        combination_error = false;
                        break;
                    }
                }

                if (combination_error)
                {
                    MessageBox.Show("Correct combination The timer is " + time_stamp);
                }
                else
                {
                    MessageBox.Show("Wrong combination");
                }
            }
        }

        

        

        
    }
}