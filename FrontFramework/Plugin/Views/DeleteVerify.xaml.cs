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
{
    /// <summary>
    /// DeleteVerify.xaml 的交互逻辑
    /// </summary>
    public partial class DeleteVerify : ComponentDynamicTranslate
    {
        PluginManageWindow father = null;
        public DeleteVerify(PluginManageWindow father)
        {
            InitializeComponent();
            initializeComponentContents();
            this.father = father;
            this.Left = father.Left + father.ActualWidth / 2 - this.ActualWidth / 2;
            this.Top = father.Top + father.ActualHeight / 2 - this.ActualHeight / 2;
            LanguageChangedNotifier.getInstance().addListener(this);
        }
        ~DeleteVerify()
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        private ITranslator translator = TranslatorBasedOnXML.getTranslator();
        public void initializeComponentContents()
        {
            translator.recoveryFromHistory();
            displayLabel.Content = translator.getComponentTranslation(new String[] { "Sure" }) + "?";
            okButton.Content = translator.getComponentTranslation("Ok");
            cancelButton.Content = translator.getComponentTranslation("Cancel");
        }

        private void okOnClick(object sender, RoutedEventArgs e)
        {
            father.deletePath();
            this.Close();
        }
    }
}
