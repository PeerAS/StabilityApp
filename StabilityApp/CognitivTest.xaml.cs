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
    public partial class CognitivTest : PhoneApplicationPage
    {       
        int[] combination;
        int[] user_input;
        int current_input = 0;
        const int MAX_INPUT = 4;
            
        public CognitivTest()
        {
            
            combination = new int[4];
            user_input = new int[4];

            for (int i = 0; i < MAX_INPUT; i++)
            {
                combination[i] = 0;
            }

                InitializeComponent();
        }

        private void Up_Button_Click(object sender, RoutedEventArgs e)
        {
            if(current_input <= MAX_INPUT)
            { 
                user_input[current_input] = 1;
                current_input++;
                display_result();
            }
        }

        void display_result()
        {
            if (current_input == 4)
            {
                MessageBox.Show("Array full");
            }
        }
    }
}