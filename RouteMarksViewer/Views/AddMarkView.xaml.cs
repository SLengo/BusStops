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
    /// Interaction logic for AddMarkView.xaml
    /// </summary>
    public partial class AddMarkView : Window
    {
        public AddMarkView(ViewModels.AddMarkViewModel addMarkViewModel)
        {
            InitializeComponent();
            addMarkViewModel.CurrentAddMarkView = this;
            this.DataContext = addMarkViewModel;
        }
    }
}
