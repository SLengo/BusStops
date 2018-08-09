using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace RouteMarksViewer.CustomControls
{
    /// <summary>
    /// Interaction logic for RouteMap.xaml
    /// </summary>
    public partial class RouteMap : UserControl
    {

        double MARK_ELLIPSE_DEFAULT_WIDTH = 20;
        double MARK_ELLIPSE_DEFAULT_HEIGHT = 20;

        public RouteMap()
        {
            InitializeComponent();
            this.Loaded += MapLoaded;
        }

        void MapLoaded(object sender, EventArgs e)
        {
            
        }

        public ObservableCollection<Models.Map> CurrentMap
        {
            get
            {
                return ((ObservableCollection<Models.Map>)GetValue(CurrentMapProperty));
            }
            set
            {
                SetValue(CurrentMapProperty, value);
            }
        }
        public static readonly DependencyProperty CurrentMapProperty = DependencyProperty.Register("CurrentMap", typeof(ObservableCollection<Models.Map>), typeof(RouteMap),
        new UIPropertyMetadata(null, new PropertyChangedCallback(CurrentMap_PropertyChanged)));

        private static void CurrentMap_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            RouteMap control = obj as RouteMap;
            UpdateUI(control);
        }



        public Models.User CurrentUser
        {
            get { return (Models.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(Models.User), typeof(RouteMap), new UIPropertyMetadata(null));



        public static void UpdateUI(RouteMap control)
        {
            if (control.MarksEllipseList == null) control.MarksEllipseList = new List<CustomControls.RouteMark>();


            // set map image
            control.MapImage = new BitmapImage(new Uri("pack://application:,,,/RouteMarksViewer;component/assets/icons/map.png"));
            if (control.CurrentMap.Count > 0)
            {
                if (control.CurrentMap[0].MapImage != "" && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "maps/" + control.CurrentMap[0].MapImage))
                    control.MapImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "maps/" + control.CurrentMap[0].MapImage));
            }

            // remove deleted
            for (int i = control.MarksEllipseList.Count - 1; i >= 0; i--)
            {
                if(control.CurrentMap.FirstOrDefault(o => o.MarkId == control.MarksEllipseList[i].MarkId) == null)
                {
                    foreach (UIElement mark in control.ImageCanvas.Children)
                    {
                        if(mark is RouteMark && (mark as RouteMark).MarkId == control.MarksEllipseList[i].MarkId)
                        {
                            control.ImageCanvas.Children.Remove(mark);
                            break;
                        }
                    }
                    control.MarksEllipseList.Remove(control.MarksEllipseList[i]);
                }
            }


            //set marks in stack (if without coords) or on map
            int skip_mark_count = 0;
            for (int i = 0; i < control.CurrentMap.Count; i++)
            {
                CustomControls.RouteMark mark_ellipse = new RouteMark();

                mark_ellipse.LabelMarkText = control.CurrentMap[i].Mark != null ? control.CurrentMap[i].Mark.Name : "Mark " + (i + 1);

                mark_ellipse.Name = "Mark_" + Convert.ToString(i);
                mark_ellipse.MarkId = control.CurrentMap[i].MarkId;
                try
                {
                    mark_ellipse.FillOfEllipse = control.CurrentMap[i].MarkColor == null ? Brushes.White : new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(control.CurrentMap[i].MarkColor));
                }
                catch
                {
                    mark_ellipse.FillOfEllipse = Brushes.White;
                }
                mark_ellipse.MarkWidth = control.MARK_ELLIPSE_DEFAULT_WIDTH;
                mark_ellipse.MarkHeight = control.MARK_ELLIPSE_DEFAULT_HEIGHT;
                TextBlock textBlock_tooltip = new TextBlock();
                textBlock_tooltip.Foreground = Brushes.Black;
                textBlock_tooltip.Text = control.CurrentMap[i].Mark != null ? control.CurrentMap[i].Mark.Name : "Mark " + (i + 1);
                mark_ellipse.ToolTip = textBlock_tooltip;

                bool not_has_in_mark_list = false;
                if (control.MarksEllipseList.FirstOrDefault(o => o.MarkId == control.CurrentMap[i].MarkId) == null)
                {
                    control.MarksEllipseList.Add(mark_ellipse);
                    not_has_in_mark_list = true;
                }
                else skip_mark_count++;

                if (control.CurrentMap[i].CoordX == 0 && control.CurrentMap[i].CoordY == 0)
                {
                    mark_ellipse.Margin = new Thickness(0, 20 * (i - skip_mark_count), 0, 0);
                    control.ImageCanvas.Children.Add(mark_ellipse);
                }
                else if (not_has_in_mark_list)
                {
                    control.ImageCanvas.Children.Add(mark_ellipse);
                }
            }
        }

        public BitmapImage MapImage
        {
            get { return (BitmapImage)GetValue(MapImageProperty); }
            set { SetValue(MapImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapImageProperty =
            DependencyProperty.Register("MapImage", typeof(BitmapImage), typeof(RouteMap), new PropertyMetadata(null, new PropertyChangedCallback(MapImage_PropertyChanged)));
        private static void MapImage_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }



        public List<CustomControls.RouteMark> MarksEllipseList
        {
            get { return (List<CustomControls.RouteMark>)GetValue(MarksEllipseListProperty); }
            set { SetValue(MarksEllipseListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MarksEllipseList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarksEllipseListProperty =
            DependencyProperty.Register("MarksEllipseList", typeof(List<CustomControls.RouteMark>), typeof(RouteMap), new PropertyMetadata(null, new PropertyChangedCallback(MarksEllipseList_PropertyChanged)));

        private static void MarksEllipseList_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        /// events

        private void ReMarks_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = System.Windows.MessageBox.Show("Вы действительно хотите сбросить все метки?\n(Метки будут помещены в левый верхний угол)", "Сброс меток",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                for (int i = 0; i < MarksEllipseList.Count; i++)
                {
                    MarksEllipseList[i].Margin = new Thickness(0, 20 * i, 0, 0);
                    MarksEllipseList[i].FillOfEllipse = Brushes.White;
                }
            }
        }

        #region color picker by rigth click on mark
        int selected_mark_color_pick = -1;
        private void ImageCanvasWorkGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentUser != null && (CurrentUser.UserRoleId != 1 && CurrentUser.UserRoleId != 2)) return;
            CurrentPoint = e.GetPosition(ImageCanvas);
            if (selected_mark_color_pick < 0)
            {
                selected_mark_color_pick = GetCurrentMark(e.GetPosition);
                if (selected_mark_color_pick < 0)
                {
                    selected_mark_color_pick = -1;
                    return;
                }
                ColorPicker _colorPicker = new ColorPicker();
                _colorPicker.ColorMode = ColorMode.ColorPalette;
                _colorPicker.ShowTabHeaders = false;
                _colorPicker.SelectedColorChanged += SelectedColorChanged_colorPicker;
                _colorPicker.Margin = new Thickness(CurrentPoint.X,
                    CurrentPoint.Y, 0, 0);
                Panel.SetZIndex(_colorPicker, 4);
                ImageCanvas.Children.Remove(_colorPicker);
                ImageCanvas.Children.Add(_colorPicker);
            }
        }
        private void SelectedColorChanged_colorPicker(object sender, EventArgs e)
        {
            if (selected_mark_color_pick < 0) return;
            Color picked_color = (Color)(sender as ColorPicker).SelectedColor;
            MarksEllipseList[selected_mark_color_pick].FillOfEllipse = new SolidColorBrush(picked_color);
            CurrentMap[selected_mark_color_pick].MarkColor = picked_color.ToString();
            selected_mark_color_pick = -1;
            ImageCanvas.Children.Remove(sender as ColorPicker);
        }
        private void ImageCanvasWorkGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (UIElement item in ImageCanvas.Children)

            {
                if (item is ColorPicker)
                {
                    ImageCanvas.Children.Remove(item);
                    break;
                }
            }
            selected_mark_color_pick = -1;
        }
        #endregion

        #region toolbar buttons
        private void ShowMarkLabels_Click(object sender, RoutedEventArgs e)
        {
            foreach (RouteMark item in MarksEllipseList)
            {
                item.ShowMarkLabels = item.ShowMarkLabels ? false : true;
            }
        }

        private void ImagePlace_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.LeftCtrl))
                ZoomImage(e.Delta);
        }
        private void ImagePlace_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.Key == Key.OemPlus && Keyboard.IsKeyDown(Key.LeftCtrl))
                || (e.Key == Key.Add && Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                ZoomImage(1);
            }
            else if (e.Key == Key.Subtract && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                ZoomImage(-1);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomImage(1);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomImage(-1);
        }
        private void ZoomImage(int delta)
        {
            double prev_w = ImagePlace.ActualWidth;
            double prev_h = ImagePlace.ActualHeight;
            if (delta > 0)
            {
                ImagePlace.Width = ImagePlace.ActualWidth + 100 > 1500 ? ImagePlace.Width : ImagePlace.ActualWidth + 100;
            }
            else
            {
                ImagePlace.Width = ImagePlace.ActualWidth - 100 <= 0 ? ImagePlace.Width : ImagePlace.ActualWidth - 100;
            }
            ImagePlace.Height *= (1 / (prev_w / ImagePlace.ActualWidth));
            ImageCanvas_SizeChangedMethod(ImagePlace, prev_w, prev_h);
        }

        private void ChooseMap_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg; *.jpeg; *.JPG; *.JPEG; *.png; *.PNG) | *.jpg; *.jpeg; *.JPG; *.JPEG; *.png; *.PNG|" +
                "AllFiles (*.*) | *.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string map_location = AppDomain.CurrentDomain.BaseDirectory + "maps";
                if (new[] { "jpg", "jpeg", "png", "JPG", "JPEG", "PNG" }
                        .Any(o => System.IO.Path.GetExtension(openFileDialog.FileName).Contains(o)))
                {
                    if (!Directory.Exists(map_location)) Directory.CreateDirectory(map_location);

                    bool overwrite = false;
                    if (File.Exists(map_location + "/" + System.IO.Path.GetFileName(openFileDialog.FileName)))
                    {
                        MessageBoxResult res =
                            System.Windows.MessageBox.Show("Файл с таким именем уже добавлен, продолжить?\n(файл перезапишется)", "Совпадение файлов", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.Yes)
                            overwrite = true;
                        else return;
                    }
                    else
                        File.Copy(openFileDialog.FileName, map_location + "/" + System.IO.Path.GetFileName(openFileDialog.FileName), overwrite);

                    foreach (Models.Map map in CurrentMap)
                    {
                        map.MapImage = System.IO.Path.GetFileName(openFileDialog.FileName);
                    }
                    MapImage = new BitmapImage(new Uri(map_location + "/" + System.IO.Path.GetFileName(openFileDialog.FileName)));
                }
                else
                    System.Windows.MessageBox.Show("ВЫбранный файл не картинка!");
            }
        }

        #endregion

        private void ImageCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageCanvas_SizeChangedMethod(sender, e.PreviousSize.Width, e.PreviousSize.Height);
        }
        private void ImageCanvas_SizeChangedMethod(object sender, double PreviousSize_Width, double PreviousSize_Height)
        {
            for (int i = 0; i < MarksEllipseList.Count; i++)
            {
                if (PreviousSize_Width == 0 && PreviousSize_Height == 0)
                {
                    if (CurrentMap[i].CoordX != 0 && CurrentMap[i].CoordY != 0)
                    {
                        MarksEllipseList[i].Margin = new Thickness(
                                            CurrentMap[i].CoordX * (1 / (CurrentMap[i].DefaultWidth / (sender as Image).ActualWidth)),
                                            CurrentMap[i].CoordY * (1 / (CurrentMap[i].DefaultHeight / (sender as Image).ActualHeight)),
                                            0, 0
                                            );
                    }
                }

                else if (MarksEllipseList[i].Margin.Left != 0 && MarksEllipseList[i].Margin.Top != 0)
                {
                    MarksEllipseList[i].Margin = new Thickness(
                        MarksEllipseList[i].Margin.Left * (1 / (PreviousSize_Width / (sender as Image).ActualWidth)),
                        MarksEllipseList[i].Margin.Top * (1 / (PreviousSize_Height / (sender as Image).ActualHeight)),
                        0, 0
                        );

                    CurrentMap[i].CoordX = (int)MarksEllipseList[i].Margin.Left;
                    CurrentMap[i].CoordY = (int)MarksEllipseList[i].Margin.Top;
                    CurrentMap[i].DefaultWidth = (int)(sender as Image).ActualWidth;
                    CurrentMap[i].DefaultHeight = (int)(sender as Image).ActualHeight;
                }
            }
        }

        #region drag n drop marks on map
        public delegate Point GetPosition(IInputElement element);

        int selected_mark = -1;

        public Point CurrentPoint
        {
            get { return (Point)GetValue(CurrentPointProperty); }
            set { SetValue(CurrentPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPointProperty =
            DependencyProperty.Register("CurrentPoint", typeof(Point), typeof(RouteMap), new PropertyMetadata(null));


        private void WorkGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentUser != null && (CurrentUser.UserRoleId != 1 && CurrentUser.UserRoleId != 2)) return;
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                CurrentPoint = e.GetPosition(ImageCanvas);
                if (selected_mark < 0)
                {
                    selected_mark = GetCurrentMark(e.GetPosition);
                    if (selected_mark < 0) return;
                    ImageCanvas.Children.Remove(MarksEllipseList[selected_mark]);
                    ImageCanvas.Children.Add(MarksEllipseList[selected_mark]);
                }

                System.Windows.Input.Mouse.SetCursor(System.Windows.Input.Cursors.Hand);
                
                Panel.SetZIndex(MarksEllipseList[selected_mark], 3);
                
                MarksEllipseList[selected_mark].Margin = new Thickness(
                    CurrentPoint.X - (int)(MARK_ELLIPSE_DEFAULT_WIDTH / 2),
                    CurrentPoint.Y - (int)(MARK_ELLIPSE_DEFAULT_HEIGHT / 2),
                    0,0);

                CurrentMap[selected_mark].CoordX = (int)CurrentPoint.X - (int)(MARK_ELLIPSE_DEFAULT_WIDTH / 2);
                CurrentMap[selected_mark].CoordY = (int)CurrentPoint.Y - (int)(MARK_ELLIPSE_DEFAULT_HEIGHT / 2);
                CurrentMap[selected_mark].DefaultWidth = (int)ImagePlace.ActualWidth;
                CurrentMap[selected_mark].DefaultHeight = (int)ImagePlace.ActualHeight;
            }
            else
            {
                System.Windows.Input.Mouse.SetCursor(System.Windows.Input.Cursors.Arrow);
                selected_mark = -1;
            }
        }
        private bool GetMouseTargetMark(Visual theTarget, GetPosition position)
        {
            if (theTarget == null) return false;
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);

            return rect.Contains(point);
        }
        private int GetCurrentMark(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < MarksEllipseList.Count; i++)
            {
                if (GetMouseTargetMark(MarksEllipseList[i], pos))
                {
                    curIndex = Convert.ToInt32(MarksEllipseList[i].Name.Split('_').Last());
                    break;
                }
            }
            return curIndex;
        }


        #endregion

        
    }
}
