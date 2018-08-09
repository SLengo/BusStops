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
    /// Interaction logic for DayOfWeekPicker.xaml
    /// </summary>
    public partial class DayOfWeekPicker : UserControl
    {
        public DayOfWeekPicker()
        {
            InitializeComponent();
        }

        bool selected_date_init = false;

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(DayOfWeekPicker), new UIPropertyMetadata("Выберите день (дни) недели",
                new PropertyChangedCallback(LabelText_PropertyChanged)));

        private static void LabelText_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DayOfWeekPicker control = obj as DayOfWeekPicker;
            control.Sign.Content = control.LabelText;
        }



        public int SelectedDays
        {
            get { return (int)GetValue(SelectedDaysProperty); }
            set { SetValue(SelectedDaysProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDays.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDaysProperty =
            DependencyProperty.Register("SelectedDays", typeof(int), typeof(DayOfWeekPicker), new UIPropertyMetadata(0,
                new PropertyChangedCallback(SelectedDays_PropertyChanged)));

        private static void SelectedDays_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DayOfWeekPicker control = obj as DayOfWeekPicker;
            control.selected_date_init = true;
            string SelectedDaysStr = control.SelectedDays.ToString();
            for (int i = 0; i < SelectedDaysStr.Length; i++)
            {
                switch (SelectedDaysStr[i])
                {
                    case '1':
                        {
                            control.Mon.IsChecked = true;
                            break;
                        }
                    case '2':
                        {
                            control.Tue.IsChecked = true;
                            break;
                        }
                    case '3':
                        {
                            control.Wed.IsChecked = true;
                            break;
                        }
                    case '4':
                        {
                            control.Thu.IsChecked = true;
                            break;
                        }
                    case '5':
                        {
                            control.Fri.IsChecked = true;
                            break;
                        }
                    case '6':
                        {
                            control.Sat.IsChecked = true;
                            break;
                        }
                    case '7':
                        {
                            control.Sun.IsChecked = true;
                            break;
                        }
                }
            }
            control.selected_date_init = false;
        }


        // events
        private void Day_Checked(object sender, RoutedEventArgs e)
        {
            if (!selected_date_init)
            {
                string SelectedDaysStr = SelectedDays.ToString();
                string day_checked = "";
                switch ((sender as System.Windows.Controls.Primitives.ToggleButton).Name)
                {
                    case "Mon":
                        {
                            day_checked = "1";
                            break;
                        }
                    case "Tue":
                        {
                            day_checked = "2";
                            break;
                        }
                    case "Wed":
                        {
                            day_checked = "3";
                            break;
                        }
                    case "Thu":
                        {
                            day_checked = "4";
                            break;
                        }
                    case "Fri":
                        {
                            day_checked = "5";
                            break;
                        }
                    case "Sat":
                        {
                            day_checked = "6";
                            break;
                        }
                    case "Sun":
                        {
                            day_checked = "7";
                            break;
                        }
                }
                if ((bool)((sender as System.Windows.Controls.Primitives.ToggleButton).IsChecked))
                {
                    SelectedDaysStr += day_checked;
                }
                else
                {
                    if (SelectedDaysStr.Contains(day_checked))
                    {
                        SelectedDaysStr = SelectedDaysStr.Replace(day_checked, "");
                    }
                }
                if (SelectedDaysStr != "")
                    SelectedDays = Convert.ToInt32(SelectedDaysStr);
                else SelectedDays = 0;
            }
        }
    }
}
