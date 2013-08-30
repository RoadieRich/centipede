using System;
using CentipedeInterfaces;

namespace Centipede.Actions
{
    [ActionCategory("User Interface",
        DisplayName = "Set Webpage",
        IconName = "setwebbrowser")]
    public class SetWebpage : Action
    {
        public SetWebpage(ICentipedeCore core)
            : base("Set Webpage", core)
        {
            Url = "#";
        }

        protected override void DoAction()
        {
            UriBuilder uriBuilder;
            if (Url.StartsWith("#"))
            {
                uriBuilder = new UriBuilder(MainWindow.Current.WebBrowser.Url)
                             {
                                 Fragment = this.Url.TrimStart('#')
                             };
            }
            else
            {
                uriBuilder = new UriBuilder(Url);
            }
            
            MainWindow.Current.WebBrowser.Navigate(uriBuilder.Uri.ToString());
        }

        [ActionArgument]
        public string Url { get; set; }
    }
}
