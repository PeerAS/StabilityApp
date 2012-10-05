using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace StabilityApp
{
    public partial class Mathproblem : PhoneApplicationPage, INotifyPropertyChanged
    {
        private string[] mathProblems;
        private string[] mathSolutions;
        private string userSolution;
        private string mode;
        private string time_stamp;
        private Random randomNumber;
        private int problemNumber;
        private bool result;
        private DateTime timer;
        private DateTime time_end;
        private TimeSpan difference;

        private DatabaseContext mathDB;
        private ObservableCollection<Math_Info> _Math_Info;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public ObservableCollection<Math_Info> Math_Info_Items
        {
            get
            {
                return _Math_Info;
            }
            set
            {
                if (_Math_Info != value)
                {
                    _Math_Info = value;
                    NotifyPropertyChanged("Math_Info_Items");
                }
            }
        }

        public Mathproblem()
        {
            mathProblems = new string[10];
            mathSolutions = new string[10];
            randomNumber = new Random();
            problemNumber = randomNumber.Next(10);
            
            //here i set the specified array index from random problemNumber to be a placeholder question
            //in a better version this would get its data directly from the database
            mathProblems[problemNumber] = "1+1";
            mathSolutions[problemNumber] = "2";

            InitializeComponent();

            mathDB = new DatabaseContext(DatabaseContext.Database_Connection); //creates a connection to the database
            this.DataContext = this;

            displayProblem();
            this.Loaded += new RoutedEventHandler(Mathproblem_Loaded); //creates an event handler for when the page is finished loading
        }

        void Mathproblem_Loaded(object sender, RoutedEventArgs e)
        {
            timer = DateTime.Now; //gets the exact time
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            NavigationContext.QueryString.TryGetValue("mode", out mode);    //uses a get-style request to check if im calibrating or performing an actual test

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

            var mathItemsInDB = from Math_Info math in mathDB.Math_Information  //fetches the math_info from the database 
                                where math.mathID == 0 select math;             //the 0 here should be switched to a randomgenerated number giving the user a
                                                                                //new question frequently          
            
            Math_Info_Items = new ObservableCollection<Math_Info>(mathItemsInDB); //moves the fetched items into an observable container

            
            base.OnNavigatedTo(e);
        }

        private void displayProblem()
        {   
            this.MathProblem.Text = mathProblems[problemNumber];    //displays the mathproblem/equation, must be switched to a ID from the datbase
        }

        private void submit_Button_Click(object sender, RoutedEventArgs e)
        {
            userSolution = this.Solution.Text;  //fetches the users answer
            time_end = DateTime.Now;    //gets the time
            difference = time_end.Subtract(timer);  //gets the time difference for comparison
            time_stamp = difference.ToString("c");  //and formats is
            
            result = userSolution.Equals(mathSolutions[problemNumber]);

            Math_Info newMathInfo = new Math_Info { solution_time = time_stamp };   //puts the users used time into the database
            Math_Info_Items.Add(newMathInfo);   
            mathDB.Math_Information.InsertOnSubmit(newMathInfo);

            //remember this must be switch to id and an update query

            this.NavigationService.Navigate(new Uri("/ResultPage.xaml?mode=" + mode + "&result=" + result + "&time=" + time_stamp, UriKind.Relative));
        }

        private void continue_Button_Click(object sender, RoutedEventArgs e)
        {
            time_end = DateTime.Now;
            difference = time_end.Subtract(timer);
            time_stamp = difference.ToString("c");

            //this must be switched to input it into the correct id for the math problem
            Math_Info newMathCalibrate = new Math_Info { solution_calibrate = time_stamp };
            Math_Info_Items.Add(newMathCalibrate);
            mathDB.Math_Information.InsertOnSubmit(newMathCalibrate);

            this.NavigationService.Navigate(new Uri("/CognitivTest.xaml?mode=calibrate", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            mathDB.SubmitChanges(); //submits the changes to the database
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}