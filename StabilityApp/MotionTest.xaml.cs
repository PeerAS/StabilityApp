using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
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
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace StabilityApp
{
    public partial class MotionTest : PhoneApplicationPage, INotifyPropertyChanged
    {
        Accelerometer stabilityReading;
        Vector3 current_acceleration;
        DispatcherTimer timer;
        DispatcherTimer movement_time;
        float peakRotationX;
        float peakRotationY;
        float peakRotationZ;
        double current_movement_tick;
        bool validData;

        private DatabaseContext motionDB;
        private ObservableCollection<Motion_Test_Info> _Motion_Info;

        public event PropertyChangedEventHandler PropertyChanged;

        public MotionTest()
        {
            timer = new DispatcherTimer();  //creates a DispatcherTimer to handle event triggers
            timer.Interval = TimeSpan.FromMilliseconds(30); //triggers every 30 milliseconds
            timer.Tick += new EventHandler(timer_Tick);

            movement_time = new DispatcherTimer();  
            movement_time.Interval = TimeSpan.FromSeconds(1);
            movement_time.Tick += new EventHandler(movement_time_Tick);

            motionDB = new DatabaseContext(DatabaseContext.Database_Connection);    //creates a link to the database
            this.DataContext = this;
            

            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.start_Accelerometer.Visibility = Visibility.Visible;
            this.Accelerometer_output.Text = "";
            peakRotationX = 0;
            peakRotationY = 0;
            peakRotationZ = 0;
            current_movement_tick = 0;

            var motionItemsInDB = from Motion_Test_Info motion in motionDB.Motion_Test_Information  //select query from the database
                                  where motion.motionID == 0
                                  select motion;

            Motion_Info = new ObservableCollection<Motion_Test_Info>(motionItemsInDB);  //moves the selected items into an ObservableCollection

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {   //this might be moved to the event handler for the movement_time.Tick
            stabilityReading.Stop();    //stops the reading from the accelerometer
            timer.Stop();   //stops the timers that handles event changes
            movement_time.Stop();

            base.OnNavigatedFrom(e);

            motionDB.SubmitChanges(); //crashes while trying to submit data
        }
        private void start_Accelerometer_Click(object sender, RoutedEventArgs e)
        {
            stabilityReading = new Accelerometer();
            stabilityReading.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);    //sets the time for when readings will be taken
            stabilityReading.CurrentValueChanged +=new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(stabilityReading_CurrentValueChanged);

            try
            {
                stabilityReading.Start(); //starts the reading from the accelerometer
                timer.Start();  //starts the timers that handle event changes
                movement_time.Start();
                this.start_Accelerometer.Visibility = Visibility.Collapsed; //removes the button
            }
            catch (InvalidOperationException)
            {
                this.Accelerometer_output.Text = "It didn't work";
            }
        }

        void stabilityReading_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            validData = stabilityReading.IsDataValid; //check if the data is valid
            current_acceleration = e.SensorReading.Acceleration; 
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (validData)
            {   //checks for new spikes in rotation
                if (current_acceleration.X > peakRotationX)
                {
                    peakRotationX = current_acceleration.X;
                }
                if (current_acceleration.Y > peakRotationY)
                {
                    peakRotationY = current_acceleration.Y;
                }
                if (current_acceleration.Z > peakRotationY)
                {
                    peakRotationZ = current_acceleration.Z;
                }
            }

            
        }


        void movement_time_Tick(object sender, EventArgs e)
        {   //displays current movement gesture to screen
            //could implement voice and animation screen for this in the future
            if(current_movement_tick == 0)
            {
                this.Accelerometer_output.Text = "Move up";
            }
            else if (current_movement_tick == 1)
            {
                this.Accelerometer_output.Text = "Move down";
            }
            else if (current_movement_tick == 2)
            {
                this.Accelerometer_output.Text = "Move left";
            }
            else if (current_movement_tick == 3)
            {
                this.Accelerometer_output.Text = "Move right";
            }
            else if (current_movement_tick == 4)
            {
                //adds the info to the database
                Motion_Test_Info newMotionInfo = new Motion_Test_Info { calibratedX = peakRotationX, calibratedY = peakRotationY, calibratedZ = peakRotationZ };
                Motion_Info.Add(newMotionInfo);
                motionDB.Motion_Test_Information.InsertOnSubmit(newMotionInfo);

                //in this section I would also run a check to see if the user has rotated the phone
                //to much by checking his peakRotation's with the one calibrated and in the database
                //of course including a slight margin of error. This part however would require extensive
                //testing both by sober and non-sober persons and are as not yet implemented, including the
                //problem of accessing the database items which I still haven't fixed.

                
                this.NavigationService.Navigate(new Uri("/ResultPage.xaml?result=True", UriKind.Relative));
            }

            current_movement_tick++;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Motion_Test_Info> Motion_Info
        {
            get
            {
                return _Motion_Info;
            }
            set
            {
                if (_Motion_Info != value)
                {
                    _Motion_Info = value;
                    NotifyPropertyChanged("Motion_Info");
                }
            }
        }
      
    }
}