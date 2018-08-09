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

namespace RouteMarksViewer.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            //RefreshComs();
        }

        //private void RefreshComs()
        //{
        //    ChooseComPortСomboBox.Items.Clear();
        //    foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
        //    {
        //        ChooseComPortСomboBox.Items.Add(s); 
        //    }
        //    if (ChooseComPortСomboBox.Items.Count != 0)
        //        ChooseComPortСomboBox.SelectedItem = ChooseComPortСomboBox.Items[ChooseComPortСomboBox.Items.Count - 1];
        //}

        //private void RefreshCOM_Click(object sender, RoutedEventArgs e)
        //{
        //    RefreshComs();
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ChooseComPortСomboBox.SelectedItem != null && ChooseComPortСomboBox.Text.Contains("COM"))
        //    {
        //        Views.StartWindow _workWindow = new Views.StartWindow();
        //        _workWindow.Show();
        //        this.Close();
        //    }
        //}

        public static void WriteToConsole(string str)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item is SettingsWindow)
                {
                    (item as SettingsWindow).OneWireOutPut.Text += str + "\n";
                    (item as SettingsWindow).ScrollOutput.ScrollToEnd();
                    break;
                }
            }
        }
    }
}
