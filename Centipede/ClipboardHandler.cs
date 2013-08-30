using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using CentipedeInterfaces;
using CentipedeInterfaces.Extensions;

namespace Centipede
{
    public static class ClipboardHandler
    {
        private const string ElementName = "CentipedeClipboard";

        public static void ToClipboard(IAction action)
        {
            ToClipboard(Enumerable.Repeat(action, 1));
        }
        
        public static void ToClipboard(IEnumerable<IAction> actions)
        {
            var xmlDoc = new XmlDocument();
            var xmlElement = xmlDoc.CreateChildElement(ElementName);
            foreach (var action in actions)
            {
                action.AddToXmlElement(xmlElement);
            }
            
            var sw = new StringWriter();
            xmlDoc.WriteContentTo(XmlWriter.Create(sw));
            Clipboard.SetData(DataFormats.Text, sw.ToString());
        }
        
        public static List<IAction> FromClipboard(ICentipedeCore core)
        {
            var data = (string)Clipboard.GetData(DataFormats.Text);
            var doc = new XmlDocument();
            doc.LoadXml(data);
            var clipBoardElement = doc.GetFirstElementByName(ElementName);
            if (clipBoardElement == null)
            {
                throw new ApplicationException();
            }
            return clipBoardElement.ChildNodes.OfType<XmlElement>()
                                   .Select(element => (IAction)Action.FromXml(element, core))
                                   .ToList();
        }

        public static bool DataIsValid()
        {
            try
            {
                var data = (string)Clipboard.GetData(DataFormats.Text);
                if (data == null)
                    return false;

                var doc = new XmlDocument();
                doc.LoadXml(data);

                var clipBoardElement = doc.GetFirstElementByName(ElementName);
                return clipBoardElement != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
