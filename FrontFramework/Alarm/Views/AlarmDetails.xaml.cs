using FrontFramework.Utils;
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

namespace FrontFramework.Alarm.Views
{
    /// <summary>
    /// AlarmDetails.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmDetails : ComponentDynamicTranslate
    {
        private static ITranslator translator = TranslatorBasedOnXML.getTranslator();
        private static PropertyUtilInterface propUtil = PropUtilFactory.getPropUtil();
        public AlarmDetails()
        {
            InitializeComponent();
            initializeComponentContents();
            LanguageChangedNotifier.getInstance().addListener(this);
        }
        ~AlarmDetails() 
        {
            LanguageChangedNotifier.getInstance().removeListener(this);
        }
        public void initializeComponentContents()
        {
            this.Title = translator.getComponentTranslation(new String[] { "Alarm", "Info." });
            menuOperation.Header = translator.getComponentTranslation("Operation");
            menuPrint.Header = translator.getComponentTranslation("Print");
            alarmDataGrid.Columns[0].Header = translator.getComponentTranslation("Type");
            alarmDataGrid.Columns[1].Header = translator.getComponentTranslation("Level");
            alarmDataGrid.Columns[2].Header = translator.getComponentTranslation("Date Time");
            alarmDataGrid.Columns[3].Header = translator.getComponentTranslation("Location");
            alarmDataGrid.Columns[4].Header = translator.getComponentTranslation("System");
            alarmDataGrid.Columns[5].Header = translator.getComponentTranslation("Description");
            alarmDataGrid.Columns[6].Header = translator.getComponentTranslation("State");
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

            /*
            // 深拷贝
            String viewXaml = System.Windows.Markup.XamlWriter.Save(MainViewFrameA);
            StringReader stringReader = new StringReader(viewXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            FrameworkElement clonedView = (FrameworkElement)XamlReader.Load(xmlReader);
            PrintHelper.Print(clonedView);
             */
        }
    }
}
