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

namespace PluginDoor
{
    /// <summary>
    /// DoorPage.xaml 的交互逻辑
    /// </summary>
    public partial class DoorPage : Page, Observer
    {
        ContextMenu context = new ContextMenu();
        int doorState = 0; // 0 locked, 1 closed, 2 open
        MenuItem menuLock = new MenuItem();
        MenuItem menuOpen = new MenuItem();
        MenuItem menuClose = new MenuItem();
        public DoorPage()
        {
            InitializeComponent();
            menuLock.Header = "Lock";
            menuLock.Click += menuClicked;
            menuClose.Header = "Close";
            menuClose.Click += menuClicked;
            menuOpen.Header = "Open";
            menuOpen.Click += menuClicked;
            setState();
            context.Items.Add(menuLock);
            context.Items.Add(menuClose);
            context.Items.Add(menuOpen);
        }

        public void action(Object obj) 
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                menuClicked(menuOpen, null);
            }));
        }
        public void setState() 
        {
            switch (doorState)
            {
                case 0:
                    menuLock.IsEnabled = false;
                    menuClose.IsEnabled = true;
                    menuOpen.IsEnabled = true;
                    break;
                case 1:
                    menuLock.IsEnabled = true;
                    menuClose.IsEnabled = false;
                    menuOpen.IsEnabled = true;
                    break;
                case 2:
                    menuLock.IsEnabled = true;
                    menuClose.IsEnabled = true;
                    menuOpen.IsEnabled = false;
                    break;
            }
        }
        private void menuClicked(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Header.ToString())
            {
                case "Lock":
                    doorState = 0;
                    setState();
                    doorIcon.Source = new BitmapImage(new Uri("C:/Users/JackL/Desktop/img/门禁/上锁的门.png"));
                    break;
                case "Close":
                    doorState = 1;
                    setState();
                    doorIcon.Source = new BitmapImage(new Uri("C:/Users/JackL/Desktop/img/门禁/关闭的门.png"));
                    break;
                case "Open":
                    doorState = 2;
                    setState();
                    doorIcon.Source = new BitmapImage(new Uri("C:/Users/JackL/Desktop/img/门禁/打开的门.png"));
                    break;
            }
        }

        private void doorClicked(object sender, MouseButtonEventArgs e)
        {
            context.IsOpen = true;
        }
    }
}
