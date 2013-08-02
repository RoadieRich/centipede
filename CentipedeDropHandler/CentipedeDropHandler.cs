using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpDropHandler;
using SharpShell.Diagnostics;

namespace CentipedeDropHandler
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".100p")]
    public class CentipedeDropHandler : SharpDropHandler
    {
        protected override void DragEnter(DragEventArgs dragEventArgs)
        {
            Logging.EnableLogging(true);
            Logging.Log("Drag enter");
            dragEventArgs.Effect = DragDropEffects.Move;
        }

        protected override void Drop(DragEventArgs dragEventArgs)
        {
            new Process
            {
                StartInfo =
                {
                    FileName = this.SelectedItemPath,
                    Verb = "Run",
                    Arguments = DragItems.AsArgumentList()
                }
            }.Start();
        }
    }

    internal static class Extensions
    {
        public static string AsArgumentList(this IEnumerable<String> list)
        {
            return String.Join(" ", new []{"/r"}.Concat(list.Select(s => String.Format("\"{0}\"", s))));
        }
    }
}
