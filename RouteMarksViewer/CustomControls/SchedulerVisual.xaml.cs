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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RouteMarksViewer.CustomControls
{
    /// <summary>
    /// Interaction logic for SchedulerVisual.xaml
    /// </summary>
    public partial class SchedulerVisual : UserControl, INotifyPropertyChanged
    {
        public SchedulerVisual()
        {
            InitializeComponent();
            //(this.Content as FrameworkElement).DataContext = this;
            //var OutputDriverDPD = DependencyPropertyDescriptor.FromProperty(CurrentRouteSchedulerProperty, typeof(SchedulerVisual));
            //OutputDriverDPD.AddValueChanged(this, (sender, args) =>
            //{
            //    CurrentRouteScheduler = (ObservableCollection<Models.RouteScheduler>)GetValue(CurrentRouteSchedulerProperty);
            //});
        }



        public ObservableCollection<Models.RouteScheduler> CurrentRouteScheduler
        {
            get { return (ObservableCollection<Models.RouteScheduler>)GetValue(CurrentRouteSchedulerProperty); }
            set
            {
                SetValue(CurrentRouteSchedulerProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentRouteSchedulerProperty =
            DependencyProperty.Register("CurrentRouteScheduler", typeof(ObservableCollection<Models.RouteScheduler>), typeof(SchedulerVisual), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(CurrentRouteScheduler_PropertyChanged)));

        private static void CurrentRouteScheduler_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SchedulerVisual control = obj as SchedulerVisual;
            UpdateUI(control);
        }

        public static void UpdateUI(SchedulerVisual control)
        {
            for (int i = 1; i < 8; i++)
            {
                Helpers.UIHelper.FindChild<StackPanel>(control.CurrentGrid, "week_" + i).Children.Clear();
            }
            foreach (Models.RouteScheduler item in control.CurrentRouteScheduler)
            {
                DigitTimeView digitTimeView = new DigitTimeView();
                digitTimeView.Margin = new Thickness(2);
                DataConvertors.TimeStampConvertor timeStampConvertor = new DataConvertors.TimeStampConvertor();
                digitTimeView.CurrentTimeToShow = (string)timeStampConvertor.Convert(item.StartTime, null, "2", null);
                string day_of_week = item.DayOfWeek.ToString();

                TextBlock toolTipTextBlock = new TextBlock();
                toolTipTextBlock.Style = null;
                DataConvertors.WindowStartTimeConvertor windowStartTimeConvertor = new DataConvertors.WindowStartTimeConvertor();
                toolTipTextBlock.Text = 
                    (string)windowStartTimeConvertor.Convert(new object[] { item.StartTime, item.WindowStartTime }, null,null,null);
                digitTimeView.ToolTip = toolTipTextBlock;

                Helpers.UIHelper.FindChild<StackPanel>(control.CurrentGrid, "week_" + day_of_week).Children.Add(digitTimeView);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
