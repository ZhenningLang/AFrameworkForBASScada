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
using Translation;
using Translation.XMLBasedLanguage;

namespace FrontFramework.Utils.ViewSetting
{
    /// <summary>
    /// ViewSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ViewSetting : ComponentDynamicTranslate
    {
        private PropertyUtilInterface propUtil = PropUtilFactory.getPropUtil();
        private MainWindow father = null;
        private String multiMode = "multipage";
        private int scaleRowVal = 1;
        private int scaleColVal = 1;
        private int mainMonitorRowVal = 1;
        private int mainMonitorColVal = 1;

        public ViewSetting(MainWindow father)
        {
            InitializeComponent();
            this.father = father;
            this.Left = father.Left + father.Width / 2 - this.Width / 2;
            this.Top = father.Top + father.Height / 2 - this.Height / 2;
            initializeComponentContents();
            LanguageChangedNotifier.getInstance().addListener(this);
        }
        ~ViewSetting() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        private ITranslator translator = TranslatorBasedOnXML.getTranslator();
        public void initializeComponentContents()
        {
            translator.recoveryFromHistory();
            this.Title = translator.getComponentTranslation(new String[] { "View", "Setting" });
            labelViewMode.Content = translator.getComponentTranslation(new String[] { "View", "Mode" });
            radioMultiPage.Content = translator.getComponentTranslation(new String[] { "Multiple", "Page" });
            radioMultiMonitor.Content = translator.getComponentTranslation(new String[] { "Multiple", "Monitor" });
            // radio button initial
            var propValue = propUtil.getStrProp("multiMode");
            if (propValue.result == false)
            {
                propUtil.setStrProp("multiMode", "multipage");
            }
            this.multiMode = propUtil.getStrProp("multiMode").info;
            if(multiMode.Trim().ToLower().Equals("multipage"))
            {
                radioMultiPage.IsChecked = true;
                radioMultiPage_Checked(null, null);
            }
            else if(multiMode.Trim().ToLower().Equals("multimonitor"))
            {
                radioMultiMonitor.IsChecked = true;
                radioMultiMonitor_Checked(null, null);
            }
            // input initial
            propUtil.reloadProperty();
            propValue = propUtil.getStrProp("multiScreenInfo");
            if (propValue.result == true)
            {
                string[] infoTemp = propValue.info.Split(new char[] { ',' });
                try
                {
                    scaleRowVal = int.Parse(infoTemp[0]);
                    scaleColVal = int.Parse(infoTemp[1]);
                    mainMonitorRowVal = int.Parse(infoTemp[2]);
                    mainMonitorColVal = int.Parse(infoTemp[3]);
                }
                catch (Exception er) { }
            }
            setMonitorInfo(scaleRowVal, scaleColVal, mainMonitorRowVal, mainMonitorColVal);
        }

        private void radioMultiPage_Checked(object sender, RoutedEventArgs e)
        {
            multiMode = "multipage";
            hiddenMask.Visibility = Visibility.Visible;
        }

        private void radioMultiMonitor_Checked(object sender, RoutedEventArgs e)
        {
            multiMode = "multimonitor";
            hiddenMask.Visibility = Visibility.Hidden;
            isMultiMonitorPossible();
        }

        private void okClick(object sender, RoutedEventArgs e)
        {
            propUtil.setStrProp("multiMode", multiMode);
            propUtil.setStrProp("multiScreenInfo", 
                scaleRowVal + ", " +
                scaleColVal + ", " +
                mainMonitorRowVal + ", " +
                mainMonitorColVal);
            propUtil.reloadProperty();
            if (multiMode.Trim().ToLower().Equals("multipage")) 
            {
                father.setMultiPageMode();
            } 
            else if (multiMode.Trim().ToLower().Equals("multimonitor"))
            {
                father.setMultiMonitorMode();
            }
            this.Close();
        }

        private void setMonitorInfo(int scaleRowVal, int scaleColVal, int mainMonitorRowVal, int mainMonitorColVal)
        {
            this.scaleRowVal = scaleRowVal;
            this.scaleColVal = scaleColVal;
            this.mainMonitorRowVal = mainMonitorRowVal;
            this.mainMonitorColVal = mainMonitorColVal;
            scaleRow.Value = scaleRowVal;
            scaleCol.Value = scaleColVal;
            mainMonitorRow.Value = mainMonitorRowVal;
            mainMonitorCol.Value = mainMonitorColVal;
            mainMonitorRow.Maximum = scaleRowVal;
            mainMonitorCol.Maximum = scaleColVal;
            // set display
            displayArea.Children.Clear();
            displayArea.RowDefinitions.Clear();
            displayArea.ColumnDefinitions.Clear();
            int count = scaleRowVal;
            while (count-- > 0)
            {
                displayArea.RowDefinitions.Add(new RowDefinition());
            }
            count = scaleColVal;
            while (count-- > 0)
            {
                displayArea.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int row = 0; row < scaleRowVal; row++) 
            {
                for (int col = 0; col < scaleColVal; col++)
                {
                    Image img = new Image();
                    img.Source = ((row + 1) == mainMonitorRowVal && (col + 1) == mainMonitorColVal) ? 
                        new BitmapImage(new Uri("pack://application:,,,/resources/images/MainScreen.png")) : 
                        new BitmapImage(new Uri("pack://application:,,,/resources/images/Screen.png"));
                    img.SetValue(Grid.RowProperty, row);
                    img.SetValue(Grid.ColumnProperty, col);
                    displayArea.Children.Add(img);
                }
            }
        }

        private void scaleInput(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (scaleRow.Value != null && scaleCol.Value != null && mainMonitorRow.Value != null && mainMonitorCol.Value != null)
            {
                setMonitorInfo((int)scaleRow.Value, (int)scaleCol.Value, (int)mainMonitorRow.Value, (int)mainMonitorCol.Value);
            }
        }

        private int mainScreenIndex = 0;
        private int advisedMonitorRow = 0;
        private int advisedMonitorCol = 0;
        private int advisedMainMonitorRow = 0;
        private int advisedMainMonitorCol = 0;
        private void isMultiMonitorPossible() 
        {
            System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
            // 只有一个屏幕
            if (screens.Length == 1)
            {
                new FrontFramework.Common.Views.Alert("Only one monitor is detected!", father).ShowDialog();
                radioMultiPage.IsChecked = true;
                return;
            }
            // 测试输出
            foreach (var screen in screens)
            {
                Console.Write("Device Name: " + screen.DeviceName + ": ");
                Console.WriteLine(screen.Bounds.Width + " " + screen.Bounds.Height);
                Console.WriteLine("Primary: " + screen.Primary);
                Console.WriteLine("Top: " + screen.Bounds.Top);
                Console.WriteLine("Bottom: " + screen.Bounds.Bottom);
                Console.WriteLine("Left: " + screen.Bounds.Left);
                Console.WriteLine("Right: " + screen.Bounds.Right);
                Console.WriteLine("Location: " + screen.Bounds.Location);
                Console.WriteLine("X: " + screen.Bounds.X);
                Console.WriteLine("Y: " + screen.Bounds.Y);
                Console.WriteLine();
            }
            // 相同分辨率检测，必须保证所有的显示屏具有相同的分辨率
            for (int i = 0; i < screens.Length - 1; i++)
            {
                if (screens[i].Bounds.Height != screens[i + 1].Bounds.Height || screens[i].Bounds.Height != screens[i + 1].Bounds.Height)
                {
                    new FrontFramework.Common.Views.Alert("各屏幕分辨率不一致，请在操作系统的显示设置中修改 \n(Resolution inconsistency, please set in the operation system settings)", this).ShowDialog();
                    radioMultiPage.IsChecked = true;
                    return;
                }
            }
            // 对齐检测，必须保证所有显示屏呈矩阵状拼接，可能存在稍许误差
            // 核心是先找到屏幕外形的包络 -> 再确定每个屏幕的中心位置 -> 再逐个位置判断是否存在屏幕
            // 先判断屏幕是否都对整齐了 (顺带找到主屏幕 index)
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].Primary) 
                {
                    mainScreenIndex = i;
                }
                if (Math.Abs(screens[i].Bounds.Top % screens[i].Bounds.Height) > 5 || Math.Abs(screens[i].Bounds.Left % screens[i].Bounds.Width) > 5)
                {
                    new FrontFramework.Common.Views.Alert("存在屏幕没有对齐，请在操作系统的显示设置中修改 \n(Some screen is misaligned, please set in the operation system settings)", this).ShowDialog();
                    radioMultiPage.IsChecked = true;
                    return;
                }
            }
            // 寻找包络 -> 即 minTop, maxBottom, maxRight, minLeft
            int minTop = screens[mainScreenIndex].Bounds.Top,
                maxBottom = screens[mainScreenIndex].Bounds.Bottom,
                maxRight = screens[mainScreenIndex].Bounds.Right,
                minLeft = screens[mainScreenIndex].Bounds.Left;
            foreach (var screen in screens)
            {
                minTop = minTop < screen.Bounds.Top ? minTop : screen.Bounds.Top;
                maxBottom = maxBottom > screen.Bounds.Bottom ? maxBottom : screen.Bounds.Bottom;
                maxRight = maxRight > screen.Bounds.Right ? maxRight : screen.Bounds.Right;
                minLeft = minLeft < screen.Bounds.Left ? minLeft : screen.Bounds.Left;
            }
            advisedMonitorRow = (maxBottom - minTop) / screens[mainScreenIndex].Bounds.Height;
            advisedMonitorCol = (maxRight - minLeft) / screens[mainScreenIndex].Bounds.Width;
            MainWindow.setOffset(0 - minLeft, 0 - minTop);
            // 确定中心位置
            int[,] centersX = new int[advisedMonitorRow, advisedMonitorCol];
            int[,] centersY = new int[advisedMonitorRow, advisedMonitorCol];
            centersX[0, 0] = minLeft + screens[mainScreenIndex].Bounds.Width / 2;
            centersY[0, 0] = minTop + screens[mainScreenIndex].Bounds.Height / 2;
            for (int row = 1; row < advisedMonitorRow; row++)
            {
                centersX[row, 0] = minLeft + screens[mainScreenIndex].Bounds.Width / 2;
                centersY[row, 0] = centersY[row - 1, 0] + screens[mainScreenIndex].Bounds.Height;
            } 
            for (int col = 1; col < advisedMonitorCol; col++)
            {
                centersX[0, col] = centersX[0, col - 1] + screens[mainScreenIndex].Bounds.Width;
                centersY[0, col] = minTop + screens[mainScreenIndex].Bounds.Height / 2;
            }
            for (int row = 1; row < advisedMonitorRow; row++)
            {
                for (int col = 1; col < advisedMonitorCol; col++)
                {
                    centersX[row, col] = centersY[row, col - 1] + screens[mainScreenIndex].Bounds.Width;
                    centersY[row, col] = centersY[row - 1, col];
                }
            }
            // 判断每个中心位置是否存在屏幕
            for (int row = 0; row < advisedMonitorRow; row++)
            {
                for (int col = 0; col < advisedMonitorCol; col++)
                {
                    bool exist = false;
                    foreach (var screen in screens) 
                    {
                        if (Math.Abs(centersX[row, col] - screen.Bounds.Left - screen.Bounds.Width / 2) < 10 && 
                            Math.Abs(centersY[row, col] - screen.Bounds.Top - screen.Bounds.Height / 2) < 10) 
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        new FrontFramework.Common.Views.Alert("存在屏幕没有对齐，请在操作系统的显示设置中修改 \n(Some screen is misaligned, please set in the operation system settings)", this).ShowDialog();
                        radioMultiPage.IsChecked = true;
                        return;
                    }
                }
            }
            // 判断主屏幕位置
            for (int row = 0; row < advisedMonitorRow; row++)
            {
                for (int col = 0; col < advisedMonitorCol; col++)
                {
                    if (Math.Abs(centersX[row, col] - screens[mainScreenIndex].Bounds.Left - screens[mainScreenIndex].Bounds.Width / 2) < 10 &&
                        Math.Abs(centersY[row, col] - screens[mainScreenIndex].Bounds.Top - screens[mainScreenIndex].Bounds.Height / 2) < 10)
                    {
                        advisedMainMonitorRow = row + 1;
                        advisedMainMonitorCol = col + 1;
                        return;
                    }
                }
            }
        }
        private void autoDetectClick(object sender, RoutedEventArgs e)
        {
            new FrontFramework.Common.Views.Alert(
                "Advised Monitor Scale: \nRow " + advisedMonitorRow + " Column " + advisedMonitorCol + "\n" +
                "Advised Primary Monitor: \nRow " + advisedMainMonitorRow + " Column " + advisedMainMonitorCol, this).ShowDialog();
        }

    }
}
