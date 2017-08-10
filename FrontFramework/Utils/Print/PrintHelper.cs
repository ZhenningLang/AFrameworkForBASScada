using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace FrontFramework.Utils.Print
{
    class PrintHelper
    {
        public static void Print(FrameworkElement element)
        {
            XpsDocument xpsDoc = convertVisualToXps(element);
        }

        private static XpsDocument convertVisualToXps(FrameworkElement visual)
        {
            visual.Measure(new Size(Int32.MaxValue, Int32.MaxValue));
            Size visualSize = visual.DesiredSize;
            visual.Arrange(new Rect(new Point(0, 0), visualSize));
            MemoryStream stream = new MemoryStream();
            string pack = "pack://temp.xps";
            Uri uri = new Uri(pack);
            DocumentPaginator paginator;
            XpsDocument xpsDoc;

            using (Package container = Package.Open(stream, FileMode.Create))
            {
                PackageStore.AddPackage(uri, container);
                using (xpsDoc = new XpsDocument(container, CompressionOption.Fast, pack))
                {
                    XpsSerializationManager rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
                    rsm.SaveAsXaml(visual);
                    paginator = ((IDocumentPaginatorSource)xpsDoc.GetFixedDocumentSequence()).DocumentPaginator;
                    paginator.PageSize = visualSize;
                }
                PackageStore.RemovePackage(uri);
            }
            return xpsDoc;
        }


    }
}
