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
    /// AlarmPage.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmPage : Page, Observer
    {
        public AlarmPage()
        {
            InitializeComponent();
        }

        public void action(Object obj) 
        {
            if (((FasAlarm)obj).alarm)
            {
                alarmIcon.Dispatcher.Invoke(new Action(() =>
                {
                    alarmIcon.Source = new BitmapImage(new Uri("C:/Users/JackL/Desktop/img/火灾/火警.png"));
                }));
            }
            else
            {
                alarmIcon.Dispatcher.Invoke(new Action(() =>
                {
                    alarmIcon.Source = new BitmapImage(new Uri("C:/Users/JackL/Desktop/img/火灾/无火警.png"));
                }));
            }
        }

    }
}
