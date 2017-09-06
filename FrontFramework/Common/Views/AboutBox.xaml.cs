using FrontFramework.Language;
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

namespace FrontFramework.Common.Views
{
    /// <summary>
    /// AboutBox.xaml 的交互逻辑
    /// </summary>
    public partial class AboutBox : Window, ComponentDynamicTranslate
    {
        public AboutBox(Window father)
        {
            InitializeComponent();
            initializeComponentContents();
            this.Left = father.Left + father.Width / 2 - this.Width / 2;
            this.Top = father.Top + father.Height / 2 - this.Height / 2;
            LanguageChangedNotifier.getInstance().addListener(this);
        }

        ~AboutBox() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        public void initializeComponentContents()
        {
            ITranslator translator = TranslatorFactory.getTranslator();
            this.Title = translator.getComponentTranslation("About");
            okButton.Content = translator.getComponentTranslation("Ok");
        }

        private void okButtonOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void emailOnClick(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject("zhenninglang@163.com");
        }
    }
}
