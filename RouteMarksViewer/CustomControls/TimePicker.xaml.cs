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
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace RouteMarksViewer.CustomControls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        static bool WaitForCurrentTimeUpdate = false;

        

        public TimePicker()
        {
            InitializeComponent();
            HoursCB = new List<int>();
            MinutesCB = new List<int>();
            SecondsCB = new List<int>();
            for (int i = 0; i < 24; i++)
            {
                HoursCB.Add(i);
            }
            for (int i = 0; i < 60; i+=5)
            {
                MinutesCB.Add(i);
                SecondsCB.Add(i);
            }
        }



        public List<int> HoursCB
        {
            get { return (List<int>)GetValue(HoursCBProperty); }
            set { SetValue(HoursCBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoursCBProperty =
            DependencyProperty.Register("HoursCB", typeof(List<int>), typeof(TimePicker), new UIPropertyMetadata(null));

        public List<int> MinutesCB
        {
            get { return (List<int>)GetValue(MinutesCBProperty); }
            set { SetValue(MinutesCBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinutesCBProperty =
            DependencyProperty.Register("MinutesCB", typeof(List<int>), typeof(TimePicker), new UIPropertyMetadata(null));

        public List<int> SecondsCB
        {
            get { return (List<int>)GetValue(SecondsCBProperty); }
            set { SetValue(SecondsCBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondsCBProperty =
            DependencyProperty.Register("SecondsCB", typeof(List<int>), typeof(TimePicker), new UIPropertyMetadata(null));

        public int CurrentTime
        {
            get
            {
                return ((int)GetValue(CurrentTimeProperty));
            }
            set
            {
                SetValue(CurrentTimeProperty, value);
            }
        }
        public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register("CurrentTime", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnCurrentTimeChanged)));

        private static void OnCurrentTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            TimeSpan ts = TimeSpan.FromSeconds(control.CurrentTime);
            WaitForCurrentTimeUpdate = true;
            control.Seconds = ts.Seconds;
            control.Minutes = ts.Minutes;
            WaitForCurrentTimeUpdate = false;
            control.Hours = ts.Hours;
        }


        public int Hours
        {
            get
            {
                return ((int)GetValue(HoursProperty));
            }
            set
            {
                SetValue(HoursProperty, value);
            }
        }
        public static readonly DependencyProperty HoursProperty = DependencyProperty.Register("Hours", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Minutes
        {
            get
            {
                return ((int)GetValue(MinutesProperty));
            }
            set
            {
                SetValue(MinutesProperty, value);
            }
        }
        public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register("Minutes", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Seconds
        {
            get
            {
                return ((int)GetValue(SecondsProperty));
            }
            set
            {
                SetValue(SecondsProperty, value);
            }
        }
        public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register("Seconds", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            if (control.Seconds >= 60)
            {
                control.CurrentTime = control.Hours * 60 * 60 +
                    (control.Minutes + control.Seconds / 60) * 60 +
                    control.Seconds % 60;
            }
            if (control.Minutes >= 60)
            {
                control.CurrentTime = (control.Hours + control.Minutes / 60) * 60 * 60 +
                    (control.Minutes % 60) * 60 +
                    control.Seconds;
            }
            if (!WaitForCurrentTimeUpdate)
            {
                control.CurrentTime = control.Hours * 60 * 60 +
                    control.Minutes * 60 +
                    control.Seconds;
            }
        }
    }
}
