using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace StabilityApp
{
    public class Localization
    {
        public Localization()
        {

        }

        private static StabilityApp.Language _localizedResources = new StabilityApp.Language();

        //returns the info from the resource files Language.resx
        public StabilityApp.Language localizedResources
        {
            get
            {
                return _localizedResources;
            }
        }
    }
}
