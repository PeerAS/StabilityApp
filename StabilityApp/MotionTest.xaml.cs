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
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace StabilityApp
{
    public partial class MotionTest : PhoneApplicationPage
    {
        Accelerometer stabilityReading;
        Vector3 current_acceleration;
        DispatcherTimer timer;
        float peakRotationX;
        float peakRotationY;
        float peakRotationZ;
        bool validData;

        public MotionTest()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += new EventHandler(timer_Tick);
            peakRotationX = 0;
            peakRotationY = 0;
            peakRotationZ = 0;
            
            InitializeComponent();
        }

        

        private void start_Accelerometer_Click(object sender, RoutedEventArgs e)
        {
            stabilityReading = new Accelerometer();
            stabilityReading.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
            stabilityReading.CurrentValueChanged +=new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(stabilityReading_CurrentValueChanged);

            try
            {
                stabilityReading.Start();
                timer.Start();
            }
            catch (InvalidOperationException)
            {
                this.Accelerometer_output.Text = "It didn't work";
            }
        }

        void stabilityReading_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            validData = stabilityReading.IsDataValid;
            current_acceleration = e.SensorReading.Acceleration;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (validData)
            {
                this.Accelerometer_output.Text = "X: " + current_acceleration.X.ToString("0.00") +
                                                 "Y: " + current_acceleration.Y.ToString("0.00") + 
                                                 "Z: " + current_acceleration.Z.ToString("0.00");

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
      
    }
}