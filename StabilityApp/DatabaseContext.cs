using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

//Most of this code has been shamefully taken from http://msdn.microsoft.com/en-us/library/hh202876(v=vs.92).aspx since I
//had no idea of how to build a database for the WP7

namespace StabilityApp
{
    public class DatabaseContext: DataContext
    {
        public static string Database_Connection = "Data Source=isostore:/Database.sdf";

        public DatabaseContext(string connection_string): base(connection_string)
        {

        }

        public Table<Math_Info> Math_Information;
        public Table<Cognitiv_Test_Info> Cognitiv_Test_Information;
    }

    [Table]
    public class Math_Info : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _mathID;

        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert)]
        public int mathID
        {
            get
            {
                return _mathID;
            }
            set
            {
                if (_mathID != value)
                {
                    NotifyPropertyChanging("mathID");
                    _mathID = value;
                    NotifyPropertyChanged("mathID");
                }
            }
        }


        private string _math_problem;

        [Column]
        public string math_problem
        {
            get
            {
                return _math_problem;
            }
            set
            {
                if (_math_problem != value)
                {
                    NotifyPropertyChanging("math_problem");
                    _math_problem = value;
                    NotifyPropertyChanged("math_problem");
                }
            }
        }

        private string _math_solution;

        [Column]
        public string math_solution
        {
            get
            {
                return _math_solution;
            }
            set
            {
                if (_math_solution != value)
                {
                    NotifyPropertyChanging("math_solution");
                    _math_solution = value;
                    NotifyPropertyChanged("math_solution");
                }
            }
        }

        private string _solution_time;

        [Column]
        public string solution_time
        {
            get
            {
                return _solution_time;
            }
            set
            {
                if (_solution_time != value)
                {
                    NotifyPropertyChanging("solution_time");
                    _solution_time = value;
                    NotifyPropertyChanged("solution_time");
                }
            }
        }

        private string _solution_calibrate;

        [Column]
        public string solution_calibrate
        {
            get
            {
                return _solution_calibrate;
            }
            set
            {
                if (_solution_calibrate != value)
                {
                    NotifyPropertyChanging("solution_calibrate");
                    _solution_calibrate = value;
                    NotifyPropertyChanged("solution_calibrate");
                }
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
    }

    [Table]
    public class Cognitiv_Test_Info: INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _cognitivID;

        [Column(IsPrimaryKey=true, IsDbGenerated=true)]
        public int cognitivID
        {
            get
            {
                return _cognitivID;
            }
            set
            {
                if (_cognitivID != value)
                {
                    NotifyPropertyChanging("cognitivID");
                    _cognitivID = value;
                    NotifyPropertyChanged("cognitivID");
                }
            }
        }

        private string _cognitiv_time;

        [Column]
        public string cognitiv_time
        {
            get
            {
                return _cognitiv_time;
            }
            set
            {
                if (_cognitiv_time != value)
                {
                    NotifyPropertyChanging("cognitiv_time");
                    _cognitiv_time = value;
                    NotifyPropertyChanged("cognitiv_time");
                }
            }
        }

        private string _cognitiv_calibrate;

        [Column]
        public string cognitiv_calibrate
        {
            get
            {
                return _cognitiv_calibrate;
            }
            set
            {
                if (_cognitiv_calibrate != value)
                {
                    NotifyPropertyChanging("cognitiv_calibrate");
                    _cognitiv_calibrate = value;
                    NotifyPropertyChanged("cognitiv_calibrate");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
    
    }

}
