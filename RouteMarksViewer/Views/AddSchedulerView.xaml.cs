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
using System.Windows.Shapes;

namespace RouteMarksViewer.Views
{
    /// <summary>
    /// Interaction logic for AddSchedulerView.xaml
    /// </summary>
    public partial class AddSchedulerView : Window
    {
        public AddSchedulerView(ViewModels.AddSchedulerViewModel addSchedulerViewModel)
        {
            InitializeComponent();
            addSchedulerViewModel.CurrentAddSchedulerWindow = this;
            this.DataContext = addSchedulerViewModel;
        }
    }
}
