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

namespace PluginFAS
{
    /// <summary>
    /// TempPage.xaml 的交互逻辑
    /// </summary>
    public partial class TempPage : Page, Observer
    {
        public TempPage()
        {
            InitializeComponent();
        }

        public void action(Object obj) 
        {
            switch (((FakeTemperature)obj).pos)
            {
                case "LeftTop":
                    labelTemp1.Dispatcher.Invoke(new Action(() =>
                    {
                        labelTemp1.Content = ((FakeTemperature)obj).showTemp;
                    }));
                    break;
                case "RightBottom":
                    labelTemp2.Dispatcher.Invoke(new Action(() =>
                    {
                        labelTemp2.Content = ((FakeTemperature)obj).showTemp;
                    }));
                    break;
            }
        }

    }
}
