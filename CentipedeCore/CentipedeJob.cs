using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    [Serializable]
    public sealed class CentipedeJob : INotifyPropertyChanged
    {
        private string _infoUrl;
        private string _author;
        private string _authorContact;
        private string _fileName;
        private string _name;

        public CentipedeJob ()
        {
            Actions = new List<IAction>();
            InfoUrl = "";
            Author = "";
            AuthorContact = "";
            FileName = "";
            Name = "";
        }

        public CentipedeJob(string fileName)
            : this(fileName, new XPathDocument(fileName).CreateNavigator())
        {
            
        }

        public ToolStripItem CreateToolStripItem()
        {
            return new ToolStripMenuItem(this.Name)
                   {
                       Tag = this.FileName
                   };
        }

        public CentipedeJob(string filename, XPathNavigator nav) : this()
        {
            try
            {
                var nameNode = nav.SelectSingleNode("//Metadata/Name");
                if (nameNode != null)
                {
                    this.Name = nameNode.Value;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                var authorNameNode = nav.SelectSingleNode("//Metadata/Author/Name");
                if (authorNameNode != null)
                {
                    this.Author = authorNameNode.Value;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                var authorContactNode = nav.SelectSingleNode("//Metadata/Author/Contact");
                if (authorContactNode != null)
                {
                    this.AuthorContact = authorContactNode.Value;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                var urlNode = nav.SelectSingleNode("//Metadata/Info/@Url");
                if (urlNode != null)
                {
                    this.InfoUrl = urlNode.Value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Invalid or missing metadata in job file");
            }
            
            FileName = filename;
        }

        public CentipedeJob(string jobFileName, XmlDocument xmlDocument) : this()
        {
            XmlElement metaElement = (XmlElement)xmlDocument.GetElementsByTagName("Metadata")[0];
            Name = metaElement.GetFirstElementByName("Name").InnerText;
            XmlElement authorElement = metaElement.GetFirstElementByName("Author");
            Author = authorElement.GetFirstElementByName("Name").InnerText;
            AuthorContact = authorElement.GetFirstElementByName("Contact").InnerText;
            InfoUrl = metaElement.GetFirstElementByName("Info").GetAttribute("Url");
            FileName = jobFileName;
        }


        public Int32 Complexity
        {
            get
            {
                return Actions.Sum(action => action.Complexity);
            }
        }

        public string InfoUrl
        {
            get
            {
                return this._infoUrl;
            }
            set
            {
                if (value == this._infoUrl)
                {
                    return;
                }
                this._infoUrl = value;
                OnPropertyChanged("InfoUrl");
            }
        }

        public string Author
        {
            get
            {
                return this._author;
            }
            set
            {
                this._author = value;
                OnPropertyChanged("Author");
            }
        }

        public string AuthorContact
        {
            get
            {
                return this._authorContact;
            }
            set
            {
                if (value == this._authorContact)
                {
                    return;
                }
                this._authorContact = value;
                OnPropertyChanged("AuthorContact");
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                if (value == this._fileName)
                {
                    return;
                }
                this._fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (value == this._name)
                {
                    return;
                }
                this._name = value;
                OnPropertyChanged("Name");
            }
        }

        public IList<IAction> Actions { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public static ToolStripItem ToolStripItemFromFilename(string jobFilename)
        {
            CentipedeJob job = new CentipedeJob(jobFilename);
            return job.CreateToolStripItem();
        }

        public bool HasBeenSaved
        {
            get { return String.IsNullOrEmpty(FileName) || String.IsNullOrEmpty(Name); }
        }
    }

    [Serializable]
    [UsedImplicitly]
    public class FavouriteJobsList : List<CentipedeJob>
    { }
}
