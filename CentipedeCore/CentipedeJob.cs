using System;
using System.Collections.Generic;
using System.Linq;


namespace CentipedeInterfaces
{
    public class CentipedeJob
    {

        public CentipedeJob ()
        {
            Actions = new List<IAction>();
            InfoUrl = "";
            Author = "";
            AuthorContact = "";
            FileName = "";
            Name = "";
        }

        public Int32 Complexity
        {
            get
            {
                return this.Actions.Sum(action => action.Complexity);
            }
        }

        public string InfoUrl { get; set; }
        public string Author { get; set; }
        public string AuthorContact { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public IList<IAction> Actions { get; private set; }
    }
}
