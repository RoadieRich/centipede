using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    [Serializable]
    public class CentipedeJob : INotifyPropertyChanged
    {
        private System.Xml.XPath.XPathNavigator nav;
        private string _infoUrl;
        private string _author;
        private string _authorContact;
        private string _fileName;
        private string _authorContact1;
        private string _infoUrl1;
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

        public CentipedeJob(string filename, XPathNavigator nav) : this()
        {
            Name = nav.SelectSingleNode("//Metadata/Name").Value;
            Author = nav.SelectSingleNode("//Metadata/Author/Name").Value;
            AuthorContact = nav.SelectSingleNode("//Metadata/Author/Contact").Value;
            InfoUrl = nav.SelectSingleNode("//Metadata/Info/@Url").Value;
            FileName = filename;
        }
        
        
        public Int32 Complexity
        {
            get
            {
                return this.Actions.Sum(action => action.Complexity);
            }
        }

        public string InfoUrl
        {
            get
            {
                return this._infoUrl1;
            }
            set
            {
                if (value == this._infoUrl1)
                {
                    return;
                }
                this._infoUrl1 = value;
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

        public IList<IAction> Actions { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Serializable]
    [UsedImplicitly]
    public class FavouriteJobsList : List<CentipedeJob>
    { }
}
