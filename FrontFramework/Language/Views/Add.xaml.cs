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
    public partial class Add : Window, ComponentDynamicTranslate
    {
        DictionaryEditor father = null;
        public Add(DictionaryEditor father)
        {
            InitializeComponent();
            initializeComponentContents();
            this.father = father;
            this.Left = father.Left + father.Width / 2 - this.Width / 2;
            this.Top = father.Top + father.Height / 2 - this.Height / 2;
            LanguageChangedNotifier.getInstance().addListener(this);
        }
        ~Add() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        private Translator translator = TranslatorBasedOnXML.getTranslator();
        public void initializeComponentContents()
        {
            translator.recoveryFromHistory();
            this.Title = translator.getComponentTranslation("Add");

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
            else if(en.Length == 0)
            {
                enInput.Focus();
                return;
            }
            father.addNewWord(ch, en);
            this.Close();
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
