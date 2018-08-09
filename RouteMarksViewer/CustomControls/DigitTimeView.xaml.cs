using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RouteMarksViewer.CustomControls
{
    /// <summary>
    /// Interaction logic for DigitTimeView.xaml
    /// </summary>
    public partial class DigitTimeView : UserControl
    {
        public DigitTimeView()
        {
            InitializeComponent();
        }



        public string CurrentTimeToShow
        {
            get { return (string)GetValue(CurrentTimeToShowProperty); }
            set { SetValue(CurrentTimeToShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTimeToShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeToShowProperty =
            DependencyProperty.Register("CurrentTimeToShow", typeof(string), typeof(DigitTimeView), new PropertyMetadata(""));


    }
}
