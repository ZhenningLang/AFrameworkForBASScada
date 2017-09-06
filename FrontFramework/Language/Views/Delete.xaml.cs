using FrontFramework.Common.Views;
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

namespace FrontFramework.Language.Views
{
    public partial class Delete : Window, ComponentDynamicTranslate
    {
        DictionaryEditor father = null;
        private String id = null;
        public Delete(DictionaryEditor father)
        {
            InitializeComponent();
            initializeComponentContents();
            this.father = father;
            this.Left = father.Left + father.Width / 2 - this.Width / 2;
            this.Top = father.Top + father.Height / 2 - this.Height / 2;
            LanguageChangedNotifier.getInstance().addListener(this);
            if (father.dataGrid.SelectedCells.Count != 0)
            {
                chInput.Text = ((TransDataModel)father.dataGrid.SelectedCells[0].Item).chinese;
                enInput.Text = ((TransDataModel)father.dataGrid.SelectedCells[0].Item).english;
                id = ((TransDataModel)father.dataGrid.SelectedCells[0].Item).id;
            }
            else
            {
                new Alert(
                    translator.getComponentTranslation(new String[] { "Please", "Choose", "One", "Row" }) + "!",
                    father).ShowDialog();
                this.Close();
            }
        }

        ~Delete() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        private ITranslator translator = TranslatorBasedOnXML.getTranslator();
        public void initializeComponentContents()
        {
            translator.recoveryFromHistory();
            this.Title = translator.getComponentTranslation("Delete");

            labelChinese.Content = translator.getComponentTranslation("Chinese");
            labelEnglish.Content = translator.getComponentTranslation("English");

            tipSpace.Content = translator.getComponentTranslation("Delete") + "?";

            buttonOK.Content = translator.getComponentTranslation("Ok");
            buttonCancel.Content = translator.getComponentTranslation("Cancel");
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                okClick(sender, null);
            }
            else if (e.Key == Key.Escape)
            {
                cancelClick(sender, null);
            }
        }

        private void okClick(object sender, RoutedEventArgs e)
        {
            father.delWord(id);
            this.Close();
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
