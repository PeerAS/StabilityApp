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
        Math_Info[] list_array;
        private string userSolution;
        private string mode;
        private string time_stamp;
        private Random randomNumber;
        private int problemNumber;
        private bool result;
        private DateTime timer;

        private DatabaseContext mathDB;
        private ObservableCollection<Math_Info> _Math_Info;

        public event PropertyChangedEventHandler PropertyChanged;        

        public Mathproblem()
        {
            mathProblems = new string[10];
            mathSolutions = new string[10];
            list_array = new Math_Info[100];
            randomNumber = new Random();
            problemNumber = randomNumber.Next(10);
            
            
            mathProblems[problemNumber] = "1+1";
            mathSolutions[problemNumber] = "2";

            InitializeComponent();

            mathDB = new DatabaseContext(DatabaseContext.Database_Connection);
            this.DataContext = this;

            displayProblem();
            this.Loaded += new RoutedEventHandler(Mathproblem_Loaded);
        }

        void Mathproblem_Loaded(object sender, RoutedEventArgs e)
        {
            timer = DateTime.Now;
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
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

            var mathItemsInDB = from Math_Info math in mathDB.Math_Information
                                where math.mathID == 0 select math;
            

            Math_Info_Items = new ObservableCollection<Math_Info>(mathItemsInDB);

            List<Math_Info> derpe = new List<Math_Info>(Math_Info_Items);
            
            base.OnNavigatedTo(e);
        }

        private void displayProblem()
        {
            this.MathProblem.Text = mathProblems[problemNumber];    //displays the mathproblem/equation
        }

        private void submit_Button_Click(object sender, RoutedEventArgs e)
        {
            userSolution = this.Solution.Text;
            DateTime time_end = DateTime.Now;
            TimeSpan difference = time_end.Subtract(timer);
            time_stamp = difference.ToString("c");
            
            result = userSolution.Equals(mathSolutions[problemNumber]);

            Math_Info newMathInfo = new Math_Info { solution_time = time_stamp };
            Math_Info_Items.Add(newMathInfo);
            mathDB.Math_Information.InsertOnSubmit(newMathInfo);

            //remember this must be switch to id...

            this.NavigationService.Navigate(new Uri("/ResultPage.xaml?mode=" + mode + "&result=" + result + "&time=" + time_stamp, UriKind.Relative));
        }

        private void continue_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CognitivTest.xaml?mode=calibrate", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            base.OnNavigatedFrom(e);

            mathDB.SubmitChanges();
        }
        #region Database functions

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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

        #endregion
    }
}