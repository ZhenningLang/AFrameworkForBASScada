using FrontFramework.Common.Views;
using FrontFramework.Interfaces;
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

namespace FrontFramework.Language.Views
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : Window, ComponentDynamicTranslate
    {
        DictionaryEditor father = null;
        private String id = null;
        public Edit(DictionaryEditor father)
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
                    translator.getComponentTranslation(new String[]{"Please","Choose","One","Row"}) + "!", 
                    father).ShowDialog();
                this.Close();
            }
        }
        ~Edit() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        private Translator translator = TranslatorBasedOnXML.getTranslator();
        public void initializeComponentContents()
        {
            translator.recoveryFromHistory();
            this.Title = translator.getComponentTranslation("Edit");

            labelChinese.Content = translator.getComponentTranslation("Chinese");
            labelEnglish.Content = translator.getComponentTranslation("English");

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
            String ch = chInput.Text;
            String en = enInput.Text;
            if (ch.Length == 0)
            {
                chInput.Focus();
                return;
            }
            else if (en.Length == 0)
            {
                enInput.Focus();
                return;
            }
            father.editWord(id, ch, en);
            this.Close();
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
