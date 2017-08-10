using FrontFramework.Enums;
using FrontFramework.Help;
using FrontFramework.Interfaces;
using FrontFramework.Language;
using FrontFramework.Station;
using FrontFramework.Utils;
using FrontFramework.Utils.Print;
using FrontFramework.Utils.Spring;
using PluginDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
        private PropertyUtilInterface propUtil = null;
        private Translator translator = null;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                // 辅助类初始化
                propUtil = PropUtilFactory.getPropUtil();
                translator = TranslatorFactory.getTranslator();
                // 加载插件 dll
                Compose(); 
                // 加载系统配置文件
                propUtil.loadProperty("resources/configs/config.xml");
                // 窗体所有文字初始化与监听注册
                initializeComponentContents();
                LanguageChangedNotifier.getInstance().addListener(this);
                // 报警模块初始化
                AlarmModuleInit(); 
                // “系统”菜单初始化（菜单中的系统选项）
                SysMenuInit(); 
                // 界面窗体大小设置
                this.Width = SystemParameters.WorkArea.Size.Width / 1.25;
                this.Height = SystemParameters.WorkArea.Size.Height / 1.25;
            }
            catch (Exception er) 
            {
                FrontFramework.Log.LogUtil.writeFuncErrorLog("主界面初始化异常", er);
                System.Environment.Exit(0);
            }
        }

        /// <summary>
        /// 所有需要在翻译时刷新的空间都在这里赋值
        /// </summary>
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
            menuDicManager.Header = translator.getComponentTranslation(new String[] { "Dictionary", "Manage" });
            menuAlarmTest.Header = translator.getComponentTranslation(new String[] { "Alarm", "Test" });
            menuStartAlarm.Header = translator.getComponentTranslation(new String[] { "Start", "Alarm" });
            menuStopAlarm.Header = translator.getComponentTranslation(new String[] { "Stop", "Alarm" });
            menuPlugin.Header = translator.getComponentTranslation("Plugin");
            menuPluginManager.Header = translator.getComponentTranslation(new String[]{"Plugin","Manage"});
            menuView.Header = translator.getComponentTranslation("View");
            normalScreenMenu.Header = translator.getComponentTranslation(new String[]{"Normal","View"});
            fullScreenMenu.Header = translator.getComponentTranslation("Full Screen");
            fullScreenMenu.ToolTip = "Esc " + translator.getComponentTranslation("Exit");
            floatScreenMenu.Header = translator.getComponentTranslation("Float Screen");
            multiPageMenu.Header = translator.getComponentTranslation("MultiPage");
            singlePageMenu.Header = translator.getComponentTranslation(new String[]{"Single","Page"});
            doublePageMenu.Header = translator.getComponentTranslation(new String[] { "Double", "Page" });
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
            alarmDataGrid.Columns[6].Header = translator.getComponentTranslation("State");
            PluginInit();
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
                    /*System.Windows.Controls.PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
                    //if ((bool)printDlg.ShowDialog().GetValueOrDefault())
                    {
                        // 为了进行深拷贝而序列化
                        String mainViewXaml = System.Windows.Markup.XamlWriter.Save(MainViewFrameA);
                        StringReader stringReader = new StringReader(mainViewXaml);
                        XmlReader xmlReader = XmlReader.Create(stringReader);
                        Frame clonedMainView = (Frame)XamlReader.Load(xmlReader);

                        Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                        clonedMainView.Measure(pageSize);
                        clonedMainView.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                        //printDlg.PrintVisual(MainView, Title);
                    }*/

                    // 深拷贝
                    String viewXaml = System.Windows.Markup.XamlWriter.Save(MainViewFrameA);
                    StringReader stringReader = new StringReader(viewXaml);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    FrameworkElement clonedView = (FrameworkElement)XamlReader.Load(xmlReader);
                    PrintHelper.Print(clonedView);
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

        private void WindowClosed(object sender, EventArgs e)
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
            foreach (AbstractPlugin plugin in plugins)
            {
                plugin.closeListeningThread();
            }
        }

        private void viewSwitchMenuClicked(object sender, EventArgs e)
        {
            Console.WriteLine(((MenuItem)sender).Name);
            MainViewFrameA.Content = null;
            MainViewFrameA.Content = pageDic[((MenuItem)sender).Name];
        }

        private void singlePageOnClick(object sender, RoutedEventArgs e)
        {

        }

        private void doublePageOnClick(object sender, RoutedEventArgs e)
        {

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
                case "menuStartAlarm":
                    startAlarm();
                    break;
                case "menuStopAlarm":
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

    #region 帮助菜单
    public partial class MainWindow : Window, ComponentDynamicTranslate
    {
        private void docHelpOnClick(object sender, RoutedEventArgs e)
        {
            ((IHelper)SpringUtil.getContext().GetObject("DocHelper")).openHelper();
        }

        private void onlineHelpOnClick(object sender, RoutedEventArgs e)
        {
            ((IHelper)SpringUtil.getContext().GetObject("OnlineHelper")).openHelper();
        }

        private void aboutClick(object sender, RoutedEventArgs e)
        {
            new FrontFramework.Common.Views.AboutBox(this).ShowDialog();
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
            foreach (AbstractPlugin plugin in plugins)
            {
                plugin.setStationCode(stationCode);
            }
            comboBoxStation.SelectedValue = stationCode;
        }
        private void stationSelected(object sender, RoutedEventArgs e)
        {
            if (comboBoxStation.SelectedValue != null)
            {
                stationCode = (int)comboBoxStation.SelectedValue;
                foreach (AbstractPlugin plugin in plugins)
                {
                    plugin.setStationCode(stationCode);
                }
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

    #region 插件交互
    public partial class MainWindow : Window, ComponentDynamicTranslate 
    {

        // 变量定义
        [ImportMany]
        public List<AbstractPlugin> plugins { get; set; } // 插件
        private Dictionary<String, Page> pageDic = new Dictionary<string, Page>(); // View Switch Menu ID - Page Thread

        /// <summary>
        /// 加载插件
        /// </summary>
        private void Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog("plugins"));
            var container = new CompositionContainer(catalog);
            //将部件（part）和宿主程序添加到组合容器
            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        /// <summary>
        /// 主要负责初始化插件在界面中的显示与一些点击回调函数
        /// </summary>
        private void PluginInit()
        {
            int i = 0;
            // 清空原有显示
            for (; i < MainMenu.Items.Count; i++)
            {
                if (((MenuItem)MainMenu.Items.GetItemAt(i)).Name.Equals("beginSeparator"))
                {
                    i++;
                    break;
                }
            }
            for (; i < MainMenu.Items.Count; i++)
            {
                if (!((MenuItem)MainMenu.Items.GetItemAt(i)).Name.Equals("endSeparator"))
                {
                    MainMenu.Items.Remove(MainMenu.Items.GetItemAt(i));
                    i--;
                }
                else
                {
                    break;
                }
            }
            ViewSwitchTabControl.Items.Clear();
            // 添加新显示
            pageDic.Clear();
            foreach (AbstractPlugin plugin in plugins)
            {
                // menu
                MenuItem item = new MenuItem();
                if (plugin.getMenuId() != null)
                {
                    item.Header = translator.getComponentTranslation(plugin.getMenuId().Split(' '));
                    foreach (String str in plugin.getMenuItemsId())
                    {
                        MenuItem childItem = new MenuItem();
                        childItem.Header = translator.getComponentTranslation(str.Split(' '));
                        item.Items.Add(childItem);
                    }
                    MainMenu.Items.Insert(MainMenu.Items.Count - 2, item);
                }
                // view switch menu
                TabItem tabItem = new TabItem();
                tabItem.Header = translator.getComponentTranslation(plugin.getViewSwitchMenuId().Split(' '));
                Menu switchMenu = new Menu();
                switchMenu.FontSize = 13;
                foreach (var view in plugin.getViewSwitchPages())
                {
                    MenuItem childItem = new MenuItem();
                    childItem.Header = translator.getComponentTranslation(view.Key.Split(' '));
                    childItem.Padding = new Thickness(10, 1, 10, 1);
                    childItem.Name = view.Key.Replace(" ", "");
                    childItem.Click += viewSwitchMenuClicked;
                    switchMenu.Items.Add(childItem);
                }
                tabItem.Content = switchMenu;
                ViewSwitchTabControl.Items.Add(tabItem);
                ViewSwitchTabControl.SelectedIndex = 0;
                // view switch page
                foreach (var id2page in plugin.getViewSwitchPages())
                {
                    pageDic.Add(id2page.Key.Replace(" ", ""), id2page.Value);
                }
                // event listener
                plugin.sendEvent += pluginEventHappend;
            }
        }

        private void pluginEventHappend(BasEvent e)
        {
            foreach (var plugin in plugins)
            {
                if (e.eventDestination.Contains<String>(plugin.getPluginId()))
                {
                    plugin.trigger(e);
                }
            }
        }

    }
    #endregion
    

}
