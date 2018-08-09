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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RouteMark : UserControl
    {
        public int MarkId { get; set; }

        public RouteMark()
        {
            InitializeComponent();
        }



        public double MarkWidth
        {
            get { return (int)GetValue(MarkWidthProperty); }
            set { SetValue(MarkWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkWidthProperty =
            DependencyProperty.Register("MarkWidth", typeof(double), typeof(RouteMark), new PropertyMetadata(double.NaN));



        public double MarkHeight
        {
            get { return (int)GetValue(MarkHeightProperty); }
            set { SetValue(MarkHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarkHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkHeightProperty =
            DependencyProperty.Register("MarkHeight", typeof(double), typeof(RouteMark), new PropertyMetadata(double.NaN));



        public Brush FillOfEllipse
        {
            get { return (Brush)GetValue(FillOfEllipseProperty); }
            set { SetValue(FillOfEllipseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FillOfEllipse.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillOfEllipseProperty =
            DependencyProperty.Register("FillOfEllipse", typeof(Brush), typeof(RouteMark), new PropertyMetadata(null));




        public bool ShowMarkLabels
        {
            get { return (bool)GetValue(ShowMarkLabelsProperty); }
            set { SetValue(ShowMarkLabelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowMarkLabels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMarkLabelsProperty =
            DependencyProperty.Register("ShowMarkLabels", typeof(bool), typeof(RouteMark), new PropertyMetadata(false,
                new PropertyChangedCallback(ShowMarkLabels_PropertyChanged)));

        private static void ShowMarkLabels_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            RouteMark control = obj as RouteMark;
            foreach (UIElement item in control.MarkPanel.Children)
            {
                if (item is TextBlock)
                {
                    (item as TextBlock).Visibility = control.ShowMarkLabels ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }


        public string LabelMarkText
        {
            get { return (string)GetValue(LabelMarkTextProperty); }
            set { SetValue(LabelMarkTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelMarkText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelMarkTextProperty =
            DependencyProperty.Register("LabelMarkText", typeof(string), typeof(RouteMark), new PropertyMetadata("",
                new PropertyChangedCallback(LabelMarkText_PropertyChanged)));

        private static void LabelMarkText_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            RouteMark control = obj as RouteMark;
            bool has_t = false;
            foreach (UIElement item in control.MarkPanel.Children)
            {
                if (item is TextBlock)
                {
                    has_t = true;
                    (item as TextBlock).Text = control.LabelMarkText;
                }
            }
            if(!has_t)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Background = Brushes.White;   
                textBlock.Text = control.LabelMarkText;
                textBlock.Foreground = Brushes.Black;
                control.MarkPanel.Children.Add(textBlock);
            }
            foreach (UIElement item in control.MarkPanel.Children)
            {
                if (item is TextBlock)
                {
                    (item as TextBlock).Visibility = control.ShowMarkLabels ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }


    }
}
