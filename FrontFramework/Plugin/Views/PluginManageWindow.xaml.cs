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

namespace FrontFramework.Plugin.Views
{    /// <summary>
    /// PluginManageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PluginManageWindow : ComponentDynamicTranslate
    {
        MainWindow father = null;
        public PluginManageWindow(MainWindow father)
        {
            InitializeComponent();
            initializeComponentContents();
            this.father = father;
            LanguageChangedNotifier.getInstance().addListener(this);
        }
        ~PluginManageWindow()
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        private ITranslator translator = TranslatorBasedOnXML.getTranslator();
        public void initializeComponentContents()
        {
            translator.recoveryFromHistory();
            this.Title = translator.getComponentTranslation(new String[] { "Plugin", "Manager" });
            addButton.Content = translator.getComponentTranslation("Add");
            delButton.Content = translator.getComponentTranslation("Delete");
            // 路径窗格显示初始化
            pathDisplayArea.Children.Clear();
            pathDisplayArea.RowDefinitions.Clear();
            for (int i = 0; i < manager.loadingPaths.Count; i++)
            {
                RowDefinition def = new RowDefinition();
                def.Height = GridLength.Auto;
                pathDisplayArea.RowDefinitions.Add(def);
                Label item = new Label();
                item.Content = manager.loadingPaths[i];
                item.SetValue(Grid.RowProperty, i);
                item.MouseLeftButtonUp += pathSelected;
                pathDisplayArea.Children.Add(item);
            }
            // 插件显示初始化
            pluginDisplayArea.Children.Clear();
            pluginDisplayArea.RowDefinitions.Clear();
            for (int i = 0; i < manager.pluginInfos.Count; i++)
            {
                RowDefinition def = new RowDefinition();
                def.Height = GridLength.Auto;
                pluginDisplayArea.RowDefinitions.Add(def);
                Label item = new Label();
                item.Content = manager.pluginInfos[i].pluginID;
                item.Foreground = manager.pluginInfos[i].isActive ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                item.SetValue(Grid.RowProperty, i);
                item.MouseLeftButtonUp += pluginSelected;
                pluginDisplayArea.Children.Add(item);
            }
        }

        private static PluginManager manager = PluginManager.getPluginManager();
        private void okButtonOnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                manager.addLoadingPath(dialog.FileName);
                seletedPathLabel = null;
                initializeComponentContents();
            }
        }
        private void delButtonOnClick(object sender, RoutedEventArgs e)
        {
            new DeleteVerify(this).ShowDialog();

        }
        public void deletePath()
        {
            manager.deleteLoadingPath(seletedPathLabel.Content.ToString());
            seletedPathLabel = null;
            initializeComponentContents();
        }

        private Label seletedPathLabel = null;
        private Label seletedPluginLabel = null;
        private void pathSelected(object sender, MouseButtonEventArgs e)
        {
            if (seletedPathLabel != null)
                seletedPathLabel.Background = new SolidColorBrush(Colors.White);
            seletedPathLabel = (Label)e.Source;
            seletedPathLabel.Background = new SolidColorBrush(Colors.LightBlue);
            e.Handled = true;
        }
        private void pluginSelected(object sender, RoutedEventArgs e)
        {
            if (seletedPluginLabel != null)
                seletedPluginLabel.Background = new SolidColorBrush(Colors.White);
            seletedPluginLabel = (Label)e.Source;
            seletedPluginLabel.Background = new SolidColorBrush(Colors.LightBlue);
            e.Handled = true;

            MahApps.Metro.IconPacks.PackIconMaterial icon = new MahApps.Metro.IconPacks.PackIconMaterial();
            if (seletedPluginLabel.Foreground.ToString().Equals(new SolidColorBrush(Colors.Green).ToString()))
            {
                icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.CheckboxMarkedOutline;
            }
            else
            {
                icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.CheckboxBlankOutline;
            }
            enableButton.Content = icon;
        }

        private void moveUpOnClick(object sender, RoutedEventArgs e)
        {
            PluginInfo info = manager.getPluginInfoByID(seletedPluginLabel.Content.ToString());
            int pluginIndex = manager.pluginInfos.IndexOf(info);
            if (pluginIndex != 0)
            {
                PluginInfo infoAbove = manager.pluginInfos[pluginIndex - 1];
                manager.setPluginInfo(info.pluginID, info.position - 1, info.isActive);
                manager.setPluginInfo(infoAbove.pluginID, infoAbove.position + 1, infoAbove.isActive);
                initializeComponentContents();
                Label oldSeletedPluginLabel = seletedPluginLabel;
                foreach (var item in pluginDisplayArea.Children)
                {
                    if (((Label)item).Content.ToString().Equals(oldSeletedPluginLabel.Content.ToString()))
                    {
                        seletedPluginLabel = (Label)item;
                        break;
                    }
                }
                seletedPluginLabel.Background = new SolidColorBrush(Colors.White);
                seletedPluginLabel.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void enableOnClick(object sender, RoutedEventArgs e)
        {
            PluginInfo info = manager.getPluginInfoByID(seletedPluginLabel.Content.ToString());
            if (info.isActive)
            {
                manager.setPluginInfo(info.pluginID, info.position, false);
                seletedPluginLabel.Foreground = new SolidColorBrush(Colors.Red);
                MahApps.Metro.IconPacks.PackIconMaterial icon = new MahApps.Metro.IconPacks.PackIconMaterial();
                icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.CheckboxBlankOutline;
                enableButton.Content = icon;
            }
            else
            {
                manager.setPluginInfo(info.pluginID, info.position, true);
                seletedPluginLabel.Foreground = new SolidColorBrush(Colors.Green);
                MahApps.Metro.IconPacks.PackIconMaterial icon = new MahApps.Metro.IconPacks.PackIconMaterial();
                icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.CheckboxMarkedOutline;
                enableButton.Content = icon;
            }
        }

        private void moveDownOnClick(object sender, RoutedEventArgs e)
        {
            PluginInfo info = manager.getPluginInfoByID(seletedPluginLabel.Content.ToString());
            int pluginIndex = manager.pluginInfos.IndexOf(info);
            if (pluginIndex < manager.pluginInfos.Count - 1)
            {
                PluginInfo infoAbove = manager.pluginInfos[pluginIndex + 1];
                manager.setPluginInfo(info.pluginID, info.position + 1, info.isActive);
                manager.setPluginInfo(infoAbove.pluginID, infoAbove.position - 1, infoAbove.isActive);
                initializeComponentContents();
                Label oldSeletedPluginLabel = seletedPluginLabel;
                foreach (var item in pluginDisplayArea.Children)
                {
                    if (((Label)item).Content.ToString().Equals(oldSeletedPluginLabel.Content.ToString()))
                    {
                        seletedPluginLabel = (Label)item;
                        break;
                    }
                }
                seletedPluginLabel.Background = new SolidColorBrush(Colors.White);
                seletedPluginLabel.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

    }
}
