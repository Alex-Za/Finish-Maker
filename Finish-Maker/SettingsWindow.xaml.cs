using Finish_Maker.Models.FileModels;
using Finish_Maker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(List<string> allExportLinksFiles)
        {
            InitializeComponent();
            DataContext = new SettingsViewModel(allExportLinksFiles);
        }
    }
}
