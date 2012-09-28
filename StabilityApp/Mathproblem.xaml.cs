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
        private Random randomNumber;
        private int problemNumber;
        

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

        private void displayProblem()
        {
            this.MathProblem.Text = mathProblems[problemNumber];
        }

        private void submit_Button_Click(object sender, RoutedEventArgs e)
        {
            string userSolution;
            userSolution = this.Solution.Text;

            if (userSolution.Equals(mathSolutions[problemNumber]))
            {
                MessageBox.Show("You were correct " + problemNumber);
            }
            else
            {
                MessageBox.Show("You are to drunk for this");
            }
        }

    }
}