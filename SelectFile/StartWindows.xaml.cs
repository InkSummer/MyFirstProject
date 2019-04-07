using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SelectFile
{
    /// <summary>
    /// StartWindows.xaml 的交互逻辑
    /// </summary>
    public partial class StartWindows : Window
    {
        public StartWindows()
        {
            InitializeComponent();
        }

        private void PicChange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var folder = new FolderChange();
            folder.Show();
        }

        private void ReNameFolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var open = new MainWindow();
            open.Show();
        }
    }
}
