using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
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
    public partial class CognitivTest : PhoneApplicationPage, INotifyPropertyChanged
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

        private DatabaseContext cognitiveDB;
        private ObservableCollection<Cognitiv_Test_Info> _Cognitive_Info;

        public event PropertyChangedEventHandler PropertyChanged;

        public CognitivTest()
        {
            _current_input = 0;
            combination = new int[4];
            user_input = new int[4];
            rand = new Random();
            timer = DateTime.Now;
            time_start = new TimeSpan(0, 0, 0);

            for (int i = 0; i < MAX_INPUT; i++)
            {
                combination[i] = rand.Next(4) ;
            }

            InitializeComponent();
            cognitiveDB = new DatabaseContext(DatabaseContext.Database_Connection);
            this.DataContext = this;
        }

        void CognitivTest_Loaded(object sender, RoutedEventArgs e)
        {
            display_combination();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.Loaded += new RoutedEventHandler(CognitivTest_Loaded);
            base.OnNavigatedTo(e);

            _current_input = 0;
            NavigationContext.QueryString.TryGetValue("mode", out mode);
            
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
                Cognitiv_Test_Info newCognitiveInfo = new Cognitiv_Test_Info{cognitiv_calibrate = time_stamp,
                                                                              cognitiv_time = time_stamp};
                Cognitiv_Info_Items.Add(newCognitiveInfo);
                cognitiveDB.Cognitiv_Test_Information.InsertOnSubmit(newCognitiveInfo);
                
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
          
        }

        void button_flash(Storyboard sb, TimeSpan begin)
        {   
            sb.BeginTime = begin;
            sb.Begin();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            cognitiveDB.SubmitChanges();
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Cognitiv_Test_Info> Cognitiv_Info_Items
        {
            get
            {
                return _Cognitive_Info;
            }
            set
            {
                if (_Cognitive_Info != value)
                {
                    _Cognitive_Info = value;
                    NotifyPropertyChanged("Cognitiv_Info_Items");
                }
            }

        }
    }
}