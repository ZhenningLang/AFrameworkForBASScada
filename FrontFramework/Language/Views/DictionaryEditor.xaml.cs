using FrontFramework.Common;
using FrontFramework.Common.Views;
using FrontFramework.Enums;
using FrontFramework.Interfaces;
using FrontFramework.Language;
using FrontFramework.Language.Views;
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
using System.Xml;

namespace FrontFramework.Language.Views
{
    /// <summary>
    /// DictionaryEditor.xaml 的交互逻辑
    /// </summary>
    public partial class DictionaryEditor : Window, ComponentDynamicTranslate
    {
        private List<TransDataModel> gridData = null;
        private Translator translator = TranslatorBasedOnXML.getTranslator();
        public DictionaryEditor(Window father)
        {
            InitializeComponent();
            initializeComponentContents();
            loadData();
            dataGrid.ItemsSource = gridData;
            dataGrid.IsReadOnly = true;
            this.Left = father.Left + father.Width / 2 - this.Width / 2;
            this.Top = father.Top + father.Height / 2 - this.Height / 2;
            LanguageChangedNotifier.getInstance().addListener(this);
        }
        ~DictionaryEditor() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }

        public void initializeComponentContents()
        {
            this.Title = translator.getComponentTranslation("DicEditor");
            
            dataGrid.Columns[0].Header = translator.getComponentTranslation("Chinese");
            dataGrid.Columns[1].Header = translator.getComponentTranslation("English");

            buttonAdd.Content = translator.getComponentTranslation("Add");
            buttonEdit.Content = translator.getComponentTranslation("Edit");
            buttonDel.Content = translator.getComponentTranslation("Delete");
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            new Add(this).ShowDialog();
        }
        public void addNewWord(String chinese, String english) 
        {
            TransDataModel newData = new TransDataModel()
            {
                id = english.Substring(0,1).ToUpper() + english.Substring(1),
                chinese = chinese,
                english = english
            };
            OprResult r = translator.addNewWordToDict(newData.id, new Dictionary<LanguageEnum, string>()
            {
                {LanguageEnum.CHINESE, newData.chinese},
                {LanguageEnum.ENGLISH, newData.english}
            });
            if (r.result == true)
            {
                gridData.Add(newData);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = gridData;
            }
            else
            {
                new Alert(r.info, this).ShowDialog();
            }
        }
        private void editClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new Edit(this).ShowDialog();
            }
            catch(Exception er)
            {
                Console.WriteLine(er.ToString());
            }
            
        }
        public void editWord(String id, String ch, String en)
        {
            delWord(id);
            addNewWord(ch, en);
        }
        private void deleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new Delete(this).ShowDialog();
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
            }
        }
        public void delWord(String id)
        {
            TransDataModel rmData = new TransDataModel();
            foreach (TransDataModel data in gridData) 
            {
                if (data.id.Equals(id)) 
                {
                    rmData = data;
                    break;
                }
            }
            OprResult r = translator.delWordFromDict(id);
            if (r.result == true)
            {
                gridData.Remove(rmData);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = gridData;
            }
            else
            {
                new Alert(r.info, this).ShowDialog();
            }
        }
        private void loadData()
        {
            gridData = new List<TransDataModel>();
            foreach (var word in translator.getAllWords())
            {
                gridData.Add(new TransDataModel()
                {
                    id = word.Key,
                    chinese = word.Value[LanguageEnum.CHINESE],
                    english = word.Value[LanguageEnum.ENGLISH]
                });
            }
        }

    }

    public class TransDataModel
    {
        public String id { get; set; }
        public String chinese { get; set; }
        public String english { get; set; }
    }

}
