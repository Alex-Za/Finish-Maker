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

namespace Finish_Maker
{
    public partial class ConsoleWindow : Window
    {
        public string ConsoleText { get; set; }
        public SolidColorBrush Color { get; set; }
        public ConsoleWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
