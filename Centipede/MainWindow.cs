using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Centipede.Actions;
using Centipede.Properties;

//     \,,/
//     (..)
// ''=={##}==''
//      ##
// ''=={##}==''
//      ##
// ''=={##}==''
//      ##
// ''=={##}==''
//      ##
// ''=={##}==''
//      ##
// ''=={##}==''
//      ##
// ''=={##}==''
//      ##
// ''=={##}==''
//      \/

namespace Centipede
{
    public partial class MainWindow : Form
    {
        public static MainWindow Instance;
        private readonly JobDataSet _dataSet = new JobDataSet();

        private readonly FavouriteJobs _favouriteJobsDataStore = new FavouriteJobs();
        private bool _dirty;

        public MainWindow()
        {
            InitializeComponent();

            Instance = this;
        }

        public override String Text
        {
            set
            {
                String dirtyMarker = String.Empty;
                if (Dirty)
                {
                    dirtyMarker = @" *";
                }
                base.Text = String.Format(@"{2} - {0}{1}", value, dirtyMarker, Resources.MainWindow_Text_Centipede);
            }
        }

        private bool Dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
                Text = Program.Instance.JobName;
            }
        }

        private Boolean ErrorHandler(ActionException e, out Action nextAction)
        {
            var messageBuilder = new StringBuilder();

            if (e.ErrorAction != null)
            {
                //UpdateHandlerDone(e.ErrorAction);
                var adc = (ActionDisplayControl)e.ErrorAction.Tag;

                SetState(adc, ActionState.Error, e.Message);

                if (ActionContainer.InvokeRequired)
                {
                    ActionContainer.Invoke(new Action<Control>(ActionContainer.ScrollControlIntoView), adc);
                }
                else
                {
                    ActionContainer.ScrollControlIntoView(adc);
                }

                messageBuilder.AppendLine(Resources.MainWindow_ErrorHandler_Error_occurred_in_);
                messageBuilder.Append(e.ErrorAction.Name).Append(@" (");
                messageBuilder.Append(e.ErrorAction.Comment).AppendLine(@")");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(Resources.MainWindow_ErrorHandler_Message_was_);
                messageBuilder.AppendLine(e.Message);
            }
            else
            {
                messageBuilder.AppendLine(Resources.MainWindow_ErrorHandler_Error_);
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(e.Message);
            }

            DialogResult result = MessageBox.Show(
                                                  messageBuilder.ToString(),
                                                  Resources.MainWindow_ErrorHandler_Error,
                                                  MessageBoxButtons.AbortRetryIgnore,
                                                  MessageBoxIcon.Exclamation
                    );

            if (e.ErrorAction == null)
            {
                nextAction = null;
                return false;
            }
            switch (result)
            {
            case DialogResult.Abort:
                nextAction = null;
                return false;
            case DialogResult.Retry:
                nextAction = e.ErrorAction;
                return true;
            case DialogResult.Ignore:
                nextAction = e.ErrorAction.GetNext();
                return true;
            default:
                nextAction = null;
                return false;
            }
        }

        private void UpdateHandlerDone(Action currentAction)
        {
            foreach (var v in Program.Instance.Variables.ToArray())
            {
                if (v.Key.StartsWith(@"_"))
                {
                    continue;
                }
                JobDataSet.VariablesRow row = _dataSet.Variables.FindByName(v.Key);
                if (row != null)
                {
                    row.Value = v.Value;
                    //row.SetModified();
                }
                else
                {
                    _dataSet.Variables.AddVariablesRow(v.Key, v.Value, 0);
                }
            }
            var adc = (ActionDisplayControl)currentAction.Tag;
            SetState(adc, ActionState.Completed, Resources.MainWindow_UpdateHandlerDone_Completed);

            if (ActionContainer.InvokeRequired)
            {
                ActionContainer.Invoke(new Action<Control>(ActionContainer.ScrollControlIntoView), adc);
            }
            else
            {
                ActionContainer.ScrollControlIntoView(adc);
            }

            //this is supposed to be a percentage, but this works.
            backgroundWorker1.ReportProgress(currentAction.Complexity);
        }

        private static void SetState([ResharperAnnotations.NotNull] ActionDisplayControl adc, ActionState state,
                                     String message)
        {
            if (adc.InvokeRequired)
            {
                adc.Invoke(new Action<ActionDisplayControl, ActionState, string>(_setActionDisplayedState), adc, state,
                           message);
            }
            else
            {
                _setActionDisplayedState(adc, state, message);
            }
        }

        private static void CompletedHandler(Boolean success)
        {
            String message;
            MessageBoxIcon icon;
            if (success)
            {
                message = Resources.MainWindow_CompletedHandler_Job_finished_successfully_;
                icon = MessageBoxIcon.Information;
            }
            else
            {
                message = Resources.MainWindow_CompletedHandler_Job_did_not_finish_;
                icon = MessageBoxIcon.Error;
            }
            MessageBox.Show(message, Resources.MainWindow_CompletedHandler_Finished, MessageBoxButtons.OK, icon);
        }

        private void ItemActivate(object sender, EventArgs e)
        {
            var sendingListView = (ListView)sender;

            var sendingActionFactory = (ActionFactory)sendingListView.SelectedItems[0];

            Program.Instance.AddAction(sendingActionFactory.Generate());
        }

        private static void _setActionDisplayedState([ResharperAnnotations.NotNull] ActionDisplayControl adc,
                                                     ActionState state, String message)
        {
            lock (adc)
            {
                adc.State = state;
                if (message != String.Empty)
                {
                    adc.StatusMessage = message;
                }
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            UIActListBox.LargeImageList.Images.Add(@"generic", Resources.generic);

            var af = new ActionFactory(Resources.MainWindow_MainWindow_ActionName_Demo_Action, typeof (DemoAction));

            UIActListBox.Items.Add(af);

            af = new ActionFactory(Resources.MainWindow_MainWindow_ActionName_Get_Filename, typeof (GetFileNameAction));

            UIActListBox.Items.Add(af);

            af = new ActionFactory(Resources.MainWindow_MainWindow_ActionName_Show_Messagebox, typeof (ShowMessageBox));

            UIActListBox.Items.Add(af);

            af = new ActionFactory(Resources.MainWindow_MainWindow_ActionName_Ask_For_Values, typeof (AskValues));

            UIActListBox.Items.Add(af);

            GetActionPlugins();

            VarDataGridView.DataSource = _dataSet.Variables;
            _dataSet.Variables.VariablesRowChanged += Variables_VariablesRowChanged;
            _dataSet.Variables.RowDeleted += DataStore_Variables_RowDeleted;

            foreach (RowStyle s in ActionContainer.RowStyles)
            {
                s.Height = 20f;
                s.SizeType = SizeType.AutoSize;
            }

            Program.Instance.ActionCompleted += UpdateHandlerDone;
            Program.Instance.BeforeAction += Program_BeforeAction;
            Program.Instance.JobCompleted += CompletedHandler;
            Program.Instance.ActionErrorOccurred += ErrorHandler;
            Program.Instance.ActionAdded += Program_ActionAdded;
            Program.Instance.ActionRemoved += Program_ActionRemoved;
            Program.Instance.AfterLoad += Program_AfterLoad;
            using (var file = File.Open(EditFavourites.GetFaveFilename(), FileMode.OpenOrCreate))
                {
                _favouriteJobsDataStore.Favourites.ReadXml(file);
            }
            UpdateFavourites();

            Dirty = false;

            ActionDisplayControl.SetDirty = delegate { Dirty = true; };
        }

/*
        private class FaveMenuItems
        {
            public ToolStripDropDownItem EditFavourites;
            public ToolStripSeparator Separator;
        }
*/

        private void UpdateFavourites()
        {   
            FavouritesMenu.DropDownItems.Clear();

            foreach (
                    ToolStripMenuItem item in
                            _favouriteJobsDataStore.Favourites.Select(job => new ToolStripMenuItem(job.Name)
                                                                             {
                                                                                     Tag = job.Filename
                                                                             }))
            {
                item.Click += ItemOnClick;
                FavouritesMenu.DropDownItems.Add(item);
            }

            FavouritesMenu.DropDownItems.Add(FaveMenuSeparator);
            FavouritesMenu.DropDownItems.Add(EditFavouritesMenuItem);
        }

        private void ItemOnClick(object sender, EventArgs eventArgs)
        {
            ToolStripDropDownItem item = (ToolStripDropDownItem)sender;
            AskSave();
            Program.Instance.LoadJob((String)item.Tag);
        }

        private void GetActionPlugins()
        {
            var di = new DirectoryInfo(Path.Combine(Application.StartupPath, Settings.Default.PluginFolder));

            IEnumerable<FileInfo> dlls = di.EnumerateFiles(@"*.dll", SearchOption.AllDirectories);

            foreach (FileInfo fi in dlls)
            {
                var evidence = new Evidence();
                var appDir = new ApplicationDirectory(Assembly.GetEntryAssembly().CodeBase);
                evidence.AddAssemblyEvidence(appDir);
                Assembly asm;
                try
                {
                    asm = Assembly.LoadFrom(fi.FullName);
                }
                catch (BadImageFormatException)
                {
                    continue;
                }
                Type[] pluginsInFile = asm.GetExportedTypes();
                foreach (Type pluginType in from type in pluginsInFile
                                            where !type.IsAbstract
                                            where type.GetCustomAttributes(typeof (ActionCategoryAttribute), true).Any()
                                            select type)
                {
                    AddToActionTab(pluginType);
                }
            }
        }

        private void AddToActionTab(Type pluginType)
        {
            Assembly asm = pluginType.Assembly;
            Object[] customAttributes = pluginType.GetCustomAttributes(typeof (ActionCategoryAttribute), true);

            ActionCategoryAttribute catAttribute = customAttributes.Cast<ActionCategoryAttribute>().First();

            TabPage tabPage =
                    AddActionTabs.TabPages.Cast<TabPage>().SingleOrDefault(tp => tp.Text == catAttribute.category);

            ListView catListView;
            if (tabPage != null)
            {
                catListView = (ListView)tabPage.Tag;
            }
            else
            {
                catListView = GenerateNewTabPage(catAttribute.category);
            }

            var af = new ActionFactory(catAttribute, pluginType);

            if (!string.IsNullOrEmpty(catAttribute.iconName))
            {
                Type t = asm.GetType(pluginType.Namespace + @".Properties.Resources");
                if (t != null)
                {
                    var icon = t.GetProperty(catAttribute.iconName, typeof (Icon)).GetValue(t, null) as Icon;
                    if (icon != null)
                    {
                        catListView.LargeImageList.Images.Add(pluginType.Name, icon);
                        af.ImageKey = pluginType.Name;
                    }
                }
            }

            catListView.Items.Add(af);
        }

        private void Program_AfterLoad(object sender, EventArgs e)
        {
            Dirty = false;
            Text = Program.Instance.JobName;
        }

        private void Program_ActionRemoved(Action action)
        {
            ActionDisplayControl adc = ActionContainer.Controls
                                                      .Cast<ActionDisplayControl>()
                                                      .FirstOrDefault(a => a.ThisAction == action);
            if (adc == null)
            {
                return;
            }
            ActionContainer.Controls.Remove(adc);
            Dirty = true;
        }

        private ListView GenerateNewTabPage(String category)
        {
            var lv = new ListView { LargeImageList = ActionIcons, SmallImageList = ActionIcons };

            var tabPage = new TabPage(category);
            tabPage.Controls.Add(lv);
            lv.Dock = DockStyle.Fill;
            tabPage.Tag = lv;
            lv.ItemDrag += BeginDrag;
            lv.ItemActivate += ItemActivate;
            AddActionTabs.TabPages.Add(tabPage);

            return lv;
        }

        private static void Program_BeforeAction(Action action)
        {
            var adc = (ActionDisplayControl)action.Tag;
            SetState(adc, ActionState.Running, Resources.MainWindow_Program_ActionStatus_Running);
        }

        private void Program_ActionAdded(Action action, int index)
        {
            Type actionType = action.GetType();
            var actionAttribute = actionType.GetCustomAttributes(true)[0] as ActionCategoryAttribute;
            ActionDisplayControl adc;
            if (actionAttribute != null && actionAttribute.displayControl != null)
            {
                Type customADCType =
                        actionType.Assembly.GetType(string.Format(@"{0}.{1}", actionType.Namespace,
                                                                  actionAttribute.displayControl));
                ConstructorInfo constructor = customADCType.GetConstructor(new[] { actionType });

                if (constructor != null)
                {
                    adc = (ActionDisplayControl)constructor.Invoke(new object[] { action });
                }
                else
                {
                    adc = new ActionDisplayControl(action);
                }
            }
            else
            {
                adc = new ActionDisplayControl(action);
            }
            adc.Deleted += adc_Deleted;
            adc.DragEnter += ActionContainer_DragEnter;
            adc.DragDrop += ActionContainer_DragDrop;
            ActionContainer.Controls.Add(adc, 0, index);
            ActionContainer.SetRow(adc, index);
            Dirty = true;
        }

        private static void adc_Deleted(object sender, CentipedeEventArgs e)
        {
            var adc = (ActionDisplayControl)sender;
            Program.Instance.RemoveAction(adc.ThisAction);
        }

        private void DataStore_Variables_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            IEnumerable<string> varsToDelete = from kvp in Program.Instance.Variables
                                               where !_dataSet.Variables.Rows.Contains(kvp.Key)
                                               select kvp.Key
                                               into k
                                               where !k.StartsWith(@"_")
                                               select k;

            IList<string> toDelete = varsToDelete as IList<string> ?? varsToDelete.ToList();
            if (toDelete.Any())
            {
                Program.Instance.Variables.Remove(toDelete.First());
            }
        }

        private void Variables_VariablesRowChanged(object sender, JobDataSet.VariablesRowChangeEvent e)
        {
            JobDataSet.VariablesRow row = e.Row;

            if (!Program.Instance.Variables.ContainsKey(row.Name))
            {
                IEnumerable<string> it = from kvp in Program.Instance.Variables
                                         where !_dataSet.Variables.Rows.Contains(kvp.Key)
                                         select kvp.Key
                                         into k
                                         where !k.StartsWith(@"_")
                                         select k;

                foreach (String key in it)
                {
                    Program.Instance.Variables.Remove(key);
                    break;
                }
            }

            Program.Instance.Variables[row.Name] = row.Value;
        }

        private void VarMenuDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in VarDataGridView.SelectedRows)
            {
                _dataSet.Variables.RemoveVariablesRow(((DataRowView)r.DataBoundItem).Row as JobDataSet.VariablesRow);
            }
        }

        private void VarDataGridView_CellContextMenuStripNeeded(object sender,
                                                                DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            var view = (DataGridView)sender;
            view.ClearSelection();
            view.Rows[e.RowIndex].Selected = true;
            e.ContextMenuStrip = VarsContextMenu;
            e.ContextMenuStrip.Show();
        }
        
        private void DoLoad()
        {
            try
            {
                AskSave();
            }
            catch (AbortOperationException)
            { }

            
            DialogResult result = OpenFileDialog.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }


            Program.Instance.LoadJob(OpenFileDialog.FileName);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            foreach (ActionDisplayControl adc in ActionContainer.Controls)
            {
                SetState(adc, ActionState.None, String.Empty);
            }
            progressBar1.Value = 0;
            progressBar1.Maximum = Program.Instance.JobComplexity;
            backgroundWorker1.RunWorkerAsync();
        }

        [STAThread]
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.Instance.RunJob();
        }

        private void MainWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                RunButton.PerformClick();
            }
        }

        private void ActionContainer_DragDrop(object sender, DragEventArgs e)
        {
            Int32 index = -1;
            if (sender is ActionDisplayControl)
            {
                index = ActionContainer.GetPositionFromControl(sender as ActionDisplayControl).Row;
            }
            object data = e.Data.GetData(@"WindowsForms10PersistentObject");
            Program.Instance.AddAction(((ActionFactory)data).Generate(), index);
        }

        private void BeginDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ActionContainer_DragEnter(object sender, DragEventArgs e)
        {
            //Fixme: Should type be checked in here?
            e.Effect = DragDropEffects.Move;
        }

        private void SaveJob()
        {
            if (String.IsNullOrEmpty(Program.Instance.JobName))
            {
                var askJobTitle = new AskJobTitle();
                DialogResult result = askJobTitle.ShowDialog(this);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                Program.Instance.JobName = askJobTitle.JobName;
            }
            saveFileDialog1.FileName = Program.Instance.JobFileName;
            saveFileDialog1.ShowDialog(this);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Program.Instance.SaveJob(saveFileDialog1.FileName);
            Dirty = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //It's not really a percentage, but it serves the purpose
            progressBar1.Increment(e.ProgressPercentage);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Program.Instance.Dispose();
        }

        private void AskSave()
        {
            if (!Dirty)
            {
                return;
            }

            DialogResult dialogResult = MessageBox.Show(Resources.MainWindow_AskSave_Do_you_wish_to_save,
                                                        Resources.MainWindow_AskSave_Unsaved_Changes,
                                                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                SaveJob();
                return;
            }

            if (dialogResult == DialogResult.No)
            {
                return;
            }
            throw new AbortOperationException();
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                AskSave();
            }
            catch (AbortOperationException)
            {
                return;
            }
            ActionContainer.Controls.Clear();
            Program.Instance.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoLoad();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Program.Instance.JobFileName))
            {
                SaveJob();
            }
            else
            {
                Program.Instance.SaveJob(Program.Instance.JobFileName);
            }
            Dirty = false;
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveJob();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                AskSave();
            }
            catch (AbortOperationException)
            {
                return;
            }

            Close();
        }

        private void EditFavouritesMenuItem_Click(object sender, EventArgs e)
        {
            EditFavourites editFavourites = new EditFavourites(_favouriteJobsDataStore);
            
            editFavourites.ShowDialog();

            UpdateFavourites();

        }

        private MessageLevel _messageLevelsShown = MessageLevel.All;

        private void ErrorsToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton)sender;
            _messageLevelsShown = _messageLevelsShown ^ MessageLevel.Error;

            UpdateOutputWindow();
        }

        private void UpdateOutputWindow()
        {
            foreach (var row in MessageDataGridView.Rows.Cast<DataGridViewRow>())
            {
                JobDataSet.MessagesRow messageRow = (JobDataSet.MessagesRow)row.DataBoundItem;
                row.Visible = _messageLevelsShown.HasFlag(messageRow.Level);
            }
        }

        private void MessageDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = MessageDataGridView.Rows[e.RowIndex];
            JobDataSet.MessagesRow messageRow = (JobDataSet.MessagesRow)jobDataSet1.Messages.Rows[e.RowIndex];
            row.Visible = _messageLevelsShown.HasFlag(messageRow.Level);
        }

    }

    [Serializable]
    internal class AbortOperationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public AbortOperationException()
        { }

        [ResharperAnnotations.UsedImplicitly]
        public AbortOperationException(string message)
                : base(message)
        { }

        [ResharperAnnotations.UsedImplicitly]
        public AbortOperationException(string message, Exception inner)
                : base(message, inner)
        { }

        protected AbortOperationException(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
        { }
    }

    [ResharperAnnotations.UsedImplicitly(ResharperAnnotations.ImplicitUseTargetFlags.WithMembers)]
    public static partial class Extensions
    {
        public static void RemoveRange(this IList list, IEnumerable items)
        {
            foreach (var item in items)
            {
                list.Remove(item);
            }
        }

        public static bool RemoveRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            return items.All(list.Remove);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        public static void AddRange(this IList list, IEnumerable items)
        {
            foreach (var item in items)
            {
                list.Remove(item);
            }
        }

        public static IList<T> Synchronise<T>(this IList<T> left, IList<T> right) where T: IComparable, IEquatable<T>
        {

            List<MyStruct<T>> leftDecorated = new List<MyStruct<T>>(left.Count);
            int[] i = { 0 };
            leftDecorated.AddRange(left.Select(v => new MyStruct<T>(v) { Index = i[0]++ }));

            List<MyStruct<T>> rightDecorated = new List<MyStruct<T>>(right.Count);
            i[0] = 0;
            rightDecorated.AddRange(right.Select(v => new MyStruct<T>(v) { Index = i[0]++ }));

            //var leftDecorated = left.Select(item => new MyStruct<T>(item)).OrderBy(item => item.Item);
            //var rightDecorated = right.Select(item => new MyStruct<T>(item)).OrderBy(item => item.Item);

            var leftEnum = leftDecorated.GetEnumerator();
            var rightEnum = rightDecorated.GetEnumerator();
            
            IList<MyStruct<T>> result = new List<MyStruct<T>>();

            bool hasItems = leftEnum.MoveNext() && rightEnum.MoveNext();

            //while both lists have items
            while (hasItems)
            {
                //if left item == right item
                if (leftEnum.Current.Equals(rightEnum.Current))
                {
                    //append left item to result
                    result.Add(leftEnum.Current);
                    //advance both
                    hasItems = leftEnum.MoveNext() && rightEnum.MoveNext();
                }
                //else 
                else
                {
                    if (leftEnum.Current.CompareTo(rightEnum.Current) > 0)
                    {
                        //append right item to result
                        result.Add(rightEnum.Current);
                        //advance right
                        hasItems = rightEnum.MoveNext();
                    }
                    else
                    {
                        //advance left
                        hasItems = leftEnum.MoveNext();
                    }
                }
            }
            //if left is empty
            if (!leftEnum.MoveNext())
            {
                //append rest of right to result
                while (rightEnum.MoveNext())
                {
                    result.Add(rightEnum.Current);
                }
            }
            //
            //return result
            return result.OrderBy(item => item.Index).Select(item => item.Item) as IList<T>;
            
        }
        
        class MyStruct<T> : IComparable<MyStruct<T>>, IEquatable<MyStruct<T>> where T: IComparable, IEquatable<T>
        {
            private readonly T _item;

            public T Item
            {
                get
                {
                    return _item;
                }
            }

            public int Index
            {
                get;
                set;
            }

            public MyStruct(T item)
            {
                _item = item;
            }

            
            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <returns>
            /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public int CompareTo(MyStruct<T> other)
            {
                return ((IComparable<T>)Item).CompareTo(other.Item);
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(MyStruct<T> other)
            {
                return Item.CompareTo(other.Item) == 0;
            }
        }
    }
}
