using Generator.ViewModel;
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

namespace Generator.View
{
    /// <summary>
    /// Logika interakcji dla klasy ChooseQuizWindow.xaml
    /// </summary>
    public partial class ChooseQuizWindow : Window
    {
        public ChooseQuizWindow()
        {
            InitializeComponent();
            DataContext = new SelectQuizViewModel();
        }
    }
}
