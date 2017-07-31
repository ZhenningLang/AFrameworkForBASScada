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
    public partial class Alert : Window
    {
        public Alert(String info, Window father)
        {
            InitializeComponent();
            shower.Text = info;
            this.Left = father.Left + father.Width / 2 - this.Width / 2;
            this.Top = father.Top + father.Height / 2 - this.Height / 2;
        }
    }
}
