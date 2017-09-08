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

namespace FrontFramework.Common.Views
{
    /// <summary>
    /// Alert.xaml 的交互逻辑
    /// </summary>
    public partial class Alert
    {
        public Alert(String info, Window father)
        {
            InitializeComponent();
            shower.Content = info + "\n";
            this.Left = father.Left + father.ActualWidth / 2 - this.ActualWidth / 2;
            this.Top = father.Top + father.ActualHeight / 2 - this.ActualHeight / 2;
        }
    }
}
