using FrontFramework.Enums;
using FrontFramework.Interfaces;
using FrontFramework.Language;
using FrontFramework.Station;
using FrontFramework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace FrontFramework
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow: Window, ComponentDynamicTranslate
    {
        // 单例变量
        private PropertyUtilInterface propUtil = XmlBasedPropUtil.getInstance();
        private Translator translator = TranslatorBasedOnXML.getTranslator();

        public MainWindow()
        {
            InitializeComponent();

            propUtil.loadProperty("resources/configs/config.xml");
            initializeComponentContents();
            this.Width = SystemParameters.WorkArea.Size.Width / 1.25;
            this.Height = SystemParameters.WorkArea.Size.Height / 1.25;
            LanguageChangedNotifier.getInstance().addListener(this);

            AlarmModuleInit();
            SysMenuInit();
        }

        ~MainWindow() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        public void initializeComponentContents() 
        {
            this.Title = translator.getComponentTranslation("MAIN_WINDOW_NAME");
            menuSystem.Header = translator.getComponentTranslation("System");
            menuUser.Header = translator.getComponentTranslation("User");
            menuLogout.Header = translator.getComponentTranslation("Logout");
            menuLanguage.Header = translator.getComponentTranslation("Language");
            menuChinese.Header = translator.getComponentTranslation("Chinese");
            menuEnglish.Header = translator.getComponentTranslation("English");
            menuTools.Header = translator.getComponentTranslation("Tools");
            menuPrint.Header = translator.getComponentTranslation("Print");
            menuPrintScr.Header = translator.getComponentTranslation("PrintScr");
            menuDicManager.Header = translator.getComponentTranslation(new String[]{"Dictionary","Manage"});
            menuPlugin.Header = translator.getComponentTranslation("Plugin");
            menuPluginManager.Header = translator.getComponentTranslation(new String[]{"Plugin","Manage"});
            menuView.Header = translator.getComponentTranslation("View");
            normalScreenMenu.Header = translator.getComponentTranslation(new String[]{"Normal","View"});
            fullScreenMenu.Header = translator.getComponentTranslation("Full Screen");
            fullScreenMenu.ToolTip = "Esc " + translator.getComponentTranslation("Exit");
            floatScreenMenu.Header = translator.getComponentTranslation("Float Screen");
            menuHelp.Header = translator.getComponentTranslation("Help");
            menuHelpDoc.Header = translator.getComponentTranslation(new String[]{"Help","Document"});
            menuHelpOnline.Header = translator.getComponentTranslation(new String[]{"Help","Online"});
            menuAbout.Header = translator.getComponentTranslation("About");
            labelWaitingSolved.Content = translator.getComponentTranslation("Unsolved");
            labelWaitingConfirmed.Content = translator.getComponentTranslation("Unconfirmed");
            buttonMute.Content = translator.getComponentTranslation("Mute");
            buttonDetailedAlarmInfo.Content = translator.getComponentTranslation(new String[] { "Detailed", "Info." });
            alarmDataGrid.Columns[0].Header = translator.getComponentTranslation("Type");
            alarmDataGrid.Columns[1].Header = translator.getComponentTranslation("Level");
            alarmDataGrid.Columns[2].Header = translator.getComponentTranslation("Date Time");
            alarmDataGrid.Columns[3].Header = translator.getComponentTranslation("Location");
            alarmDataGrid.Columns[4].Header = translator.getComponentTranslation("System");
            alarmDataGrid.Columns[5].Header = translator.getComponentTranslation("Description");
            StationMenuInit();
        }
        public void setTitle(String title) 
        {
            this.Title = title;
        }

        #region 菜单对应动作



            # region 工具菜单
                private void dicManagerOnClick(object sender, RoutedEventArgs e)
                {
                    new FrontFramework.Language.Views.DictionaryEditor(this).ShowDialog();
                }
                private void printOnClick(object sender, RoutedEventArgs e)
                {
                    System.Windows.Controls.PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
                    if ((bool)printDlg.ShowDialog().GetValueOrDefault())
                    {
                        // 为了进行深拷贝而序列化
                        string mainViewXaml = System.Windows.Markup.XamlWriter.Save(MainView);
                        StringReader stringReader = new StringReader(mainViewXaml);
                        XmlReader xmlReader = XmlReader.Create(stringReader);
                        UIElement clonedMainView = (UIElement)XamlReader.Load(xmlReader);

                        Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                        clonedMainView.Measure(pageSize);
                        clonedMainView.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                        printDlg.PrintVisual(clonedMainView, Title);
                    }
                }
            #endregion

            # region 插件菜单
            #endregion

            # region 视图菜单
                private void normalScreenOnClick(object sender, RoutedEventArgs e)
                {
                    setWindowState(screenState, ScreenStateEnum.NORMAL);
                }
                private void fullScreenOnClick(object sender, RoutedEventArgs e)
                {
                    setWindowState(screenState, ScreenStateEnum.FULLSCREEN);
                }
                private void floatScreenOnClick(object sender, RoutedEventArgs e)
                {
                    setWindowState(screenState, ScreenStateEnum.FLOAT);
                }

                private double widthBeforeFullScreen = 0;
                private double heightBeforeFullScreen = 0;
                private double leftBeforeFullScreen = 0;
                private double topBeforeFullScreen = 0;
                private ScreenStateEnum screenState = ScreenStateEnum.NORMAL;
                private void setWindowState(ScreenStateEnum from, ScreenStateEnum to)
                {
                    if (from == to)
                        return;
                    // 操作 View 区域
                    if (to == ScreenStateEnum.NORMAL)
                    {
                        MenuArea.Width = double.NaN;
                        MenuArea.Height = double.NaN;
                        ViewSwitchArea.Width = double.NaN;
                        ViewSwitchArea.Height = double.NaN;
                        MainView.Width = double.NaN;
                        MainView.Height = double.NaN;
                        AlarmArea.Width = double.NaN;
                        AlarmArea.Height = double.NaN;
                    }
                    else if (to == ScreenStateEnum.FULLSCREEN)
                    {
                        MenuArea.Width = 0;
                        MenuArea.Height = 0;
                        ViewSwitchArea.Width = double.NaN;
                        ViewSwitchArea.Height = double.NaN;
                        MainView.Width = double.NaN;
                        MainView.Height = double.NaN;
                        AlarmArea.Width = 0;
                        AlarmArea.Height = 0;
                    }
                    else if (to == ScreenStateEnum.FLOAT)
                    {
                        MenuArea.Width = double.NaN;
                        MenuArea.Height = double.NaN;
                        ViewSwitchArea.Width = 0;
                        ViewSwitchArea.Height = 0;
                        MainView.Width = 0;
                        MainView.Height = 0;
                        AlarmArea.Width = double.NaN;
                        AlarmArea.Height = double.NaN;
                    }
                    // Window title
                    if (to == ScreenStateEnum.FULLSCREEN)
                        this.WindowStyle = WindowStyle.None;
                    else
                        this.WindowStyle = WindowStyle.SingleBorderWindow;
                    // resizable
                    if (to == ScreenStateEnum.FULLSCREEN)
                        this.ResizeMode = ResizeMode.NoResize;
                    else
                        this.ResizeMode = ResizeMode.CanResize;
                    // 全屏
                    if (to == ScreenStateEnum.FULLSCREEN)
                        this.WindowState = WindowState.Maximized;
                    else
                        this.WindowState = WindowState.Normal;
                    // 覆盖开始菜单
                    if (to == ScreenStateEnum.NORMAL)
                        this.Topmost = false;
                    else
                        this.Topmost = true;
                    // Min size
                    if (to == ScreenStateEnum.FLOAT)
                    {
                        this.MinHeight = 0.0;
                        this.MinWidth = 0.0;
                    }
                    else
                    {
                        this.MinHeight = 600.0;
                        this.MinWidth = 800.0;
                    }
                    // 数据存储
                    if (from == ScreenStateEnum.NORMAL)
                    {
                        widthBeforeFullScreen = this.Width;
                        heightBeforeFullScreen = this.Height;
                        leftBeforeFullScreen = this.Left;
                        topBeforeFullScreen = this.Top;
                    }
                    // 数据恢复
                    if (to != ScreenStateEnum.NORMAL)
                    {
                        this.Width = widthBeforeFullScreen;
                        this.Height = heightBeforeFullScreen;
                        this.Left = leftBeforeFullScreen;
                        this.Top = topBeforeFullScreen;
                    }
                    if (to == ScreenStateEnum.FLOAT)
                    {
                        this.SizeToContent = SizeToContent.Height;
                    }
                    else
                    {
                        this.SizeToContent = SizeToContent.Manual;
                    }
                    // Max size
                    if (to == ScreenStateEnum.FLOAT)
                    {
                        fullScreenMenu.IsEnabled = false;
                        this.MaxHeight = this.Height;
                        this.MinHeight = this.Height;
                    }
                    else
                    {
                        fullScreenMenu.IsEnabled = true;
                        this.MaxHeight = 5000.0;
                    }
                    // menu enabled
                    if (to == ScreenStateEnum.NORMAL)
                    {
                        normalScreenMenu.IsEnabled = false;
                        fullScreenMenu.IsEnabled = true;
                        floatScreenMenu.IsEnabled = true;
                    }
                    else if (to == ScreenStateEnum.FLOAT)
                    {
                        normalScreenMenu.IsEnabled = true;
                        fullScreenMenu.IsEnabled = false;
                        floatScreenMenu.IsEnabled = false;
                    }
                    // 状态改变
                    screenState = to;
                }
            #endregion

            # region 帮助菜单
            #endregion

        #endregion

        /// <summary>
        /// 键盘监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void mainScreenKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && screenState == ScreenStateEnum.FULLSCREEN)
            {
                setWindowState(screenState, ScreenStateEnum.NORMAL);
            }
        }

    }

    #region 报警声音操作
    public partial class MainWindow : Window, ComponentDynamicTranslate
    {
        public void AlarmModuleInit()
        {
            double volume = 0.25;
            try
            {
                volume = double.Parse(propUtil.getStrProp("historyVolume").info.Trim());
            }
            catch (Exception er) 
            {
                Console.WriteLine(er.ToString());
            }
            sliderVolume.Value = volume / (VolumnEnum.MAX_VOL - VolumnEnum.MIN_VOL) * 10.0;
            alarmPlayer.Volume = volume;
        }

        private Boolean muteState = false; // true -> mute, false -> not mute
        public void startAlarm()
        {
            alarmPlayer.Stop();
            alarmPlayer.Play();
        }
        public void stopAlarm()
        {
            alarmPlayer.Stop();
        }
        private void muteButtonClick(object sender, RoutedEventArgs e)
        {
            alarmPlayer.IsMuted = !alarmPlayer.IsMuted;
            sliderVolume.IsEnabled = !sliderVolume.IsEnabled;
            muteState = !muteState;
        }

        private void sliderVolumeMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double volume = ((Slider)sender).Value * (VolumnEnum.MAX_VOL - VolumnEnum.MIN_VOL) / 10.0;
            alarmPlayer.Volume = volume;
            propUtil.setStrProp("historyVolume", volume.ToString());
        }

        private void alarmPlayerMediaEnd(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Stop();
            (sender as MediaElement).Play();
        }

        private void testMenuClick(object sender, RoutedEventArgs e)
        {
            switch (((System.Windows.Controls.MenuItem)sender).Name)
            {
                case "testStartAlarm":
                    startAlarm();
                    break;
                case "testStopAlarm":
                    stopAlarm();
                    break;
            }
        }
    }
    #endregion

    #region 系统菜单
    public partial class MainWindow : Window, ComponentDynamicTranslate
    {
        public void SysMenuInit() 
        {
            switch (translator.getLanguageTo())
            {
                case LanguageEnum.CHINESE:
                    chCheckedImg.Visibility = Visibility.Visible;
                    break;
                case LanguageEnum.ENGLISH:
                    enCheckedImg.Visibility = Visibility.Visible; 
                    break;
            }
        }
        /// <summary>
        /// 登出操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logoutOnClick(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }

        /// <summary>
        /// 切换中文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chMenuOnClick(object sender, RoutedEventArgs e)
        {
            chCheckedImg.Visibility = Visibility.Visible;
            enCheckedImg.Visibility = Visibility.Hidden;
            translator.setLanguage(LanguageEnum.ENGLISH, LanguageEnum.CHINESE);
            LanguageChangedNotifier.getInstance().notifyAll();
        }
        /// <summary>
        /// 切换英文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enMenuOnClick(object sender, RoutedEventArgs e)
        {
            enCheckedImg.Visibility = Visibility.Visible;
            chCheckedImg.Visibility = Visibility.Hidden;
            translator.setLanguage(LanguageEnum.CHINESE, LanguageEnum.ENGLISH);
            LanguageChangedNotifier.getInstance().notifyAll();
        }
    } 
    #endregion

    #region 车站菜单
    public partial class MainWindow : Window, ComponentDynamicTranslate
    {
        private int stationCode;
        public int getStationCode() { return this.stationCode; }
        private void StationMenuInit()
        {
            List<StationInfo> stationInfoList = StationInfoManager.getInstance().getStationInfoList();
            List<StationComboboxModel> newItems = new List<StationComboboxModel>();
            foreach (StationInfo info in stationInfoList) 
            {
                StationComboboxModel temp = new StationComboboxModel();
                temp.stationCode = info.stationCode;
                if (info.isTransfer == true)
                {
                    temp.imgPath = "resources/images/transfer.png";
                    String tip = "| ";
                    foreach (String line in info.transLines)
                    {
                        tip += translator.getComponentTranslation(line) + " | ";
                    }
                    temp.tipText = tip;
                }
                else
                {
                    temp.imgPath = "resources/images/white.png";
                    temp.tipText = null;
                }
                temp.stationName = translator.getComponentTranslation(info.id);
                newItems.Add(temp);
            }
            comboBoxStation.SelectedValuePath = "stationCode";
            comboBoxStation.ItemsSource = null;
            comboBoxStation.ItemsSource = newItems;
            // 从历史中恢复数据
            try
            {
                stationCode = int.Parse(propUtil.getStrProp("historyStationCode").info.Trim());
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
            }
            comboBoxStation.SelectedValue = stationCode;
        }
        private void stationSelected(object sender, RoutedEventArgs e)
        {
            if (comboBoxStation.SelectedValue != null)
            {
                stationCode = (int)comboBoxStation.SelectedValue;
                propUtil.setStrProp("historyStationCode", stationCode.ToString());
            }
        }
    }
    class StationComboboxModel
    {
        public int stationCode { get; set; }
        public String stationName { get; set; }
        public String imgPath { get; set; }
        public String tipText { get; set; }
    }
    #endregion

}
