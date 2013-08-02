using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Centipede.Actions;
using Centipede.Properties;
using CentipedeInterfaces;
using PythonEngine;
using ResharperAnnotations;

//      \,,/
//      (..)
//  ''=={##}==''
//       ##
//  ''=={##}==''
//       ##
//  ''=={##}==''
//       ##
//  ''=={##}==''
//       ##
//  ''=={##}==''
//       ##
//  ''=={##}==''
//       ##
//  ''=={##}==''
//       ##
//  ''=={##}==''
//       \/

namespace Centipede
{
    [ComVisible(true)]
    public partial class MainWindow : Form
    {
        private readonly FavouriteJobs _favouriteJobsDataStore = new FavouriteJobs();
        
        [UsedImplicitly]
        private List<string> _arguments;

        private bool _dirty;
        private MessageLevel _displayedLevels;
        private ActionEventArgs _pendingUpdate;
        private bool _stepping;
        private ManualResetEvent _steppingMutex;
        private bool _unloadablePlugins;
        private ToolStripSpringTextBox _urlTextbox;

        public MainWindow(ICentipedeCore centipedeCore, List<string> arguments = null)
        {
            this._pendingUpdate = null;
            //Visible = false;

            this.InitializeComponent();
            this.SetUserProperties();

            this.WebBrowser.DocumentText = Resources.WelcomeScreen;

            this.WebBrowser.ObjectForScripting = new ShowTutorialObject(WebBrowser);

            this.WebBrowser.AllowNavigation = true;


            //Visible = true;

            this.Core = centipedeCore;

            ActionFactory.MessageHandler = this.Action_MessageHandler;

            this._arguments = arguments;

            this.Core.Variables.PropertyChanged += this.Core_Variables_PropertyChanged;

            this._dataSet.Messages.RowChanged += delegate
                                                     {
                                                         DataGridView msgDGV = this.MessageDataGridView;
                                                         msgDGV.Invoke(new System.Action(msgDGV.Invalidate));
                                                         msgDGV.Invoke(new System.Action(msgDGV.Update));
                                                     };

            this.MessageDataGridView.CellFormatting += delegate(object sender, DataGridViewCellFormattingEventArgs args)
                                                           {
                                                               if (args.Value is IAction)
                                                               {
                                                                   args.Value = args.Value.ToString();
                                                               }
                                                           };

            this.Core.Tag = this;
        }

        [ComVisible(true)]
        public class ShowTutorialObject
        {
            [NotNull]
            private readonly WebBrowser _webBrowser;

            public ShowTutorialObject(WebBrowser webBrowser)
            {
                _webBrowser = webBrowser;
            }

            public void ShowTutorial()
            {
                string appDir = Path.GetDirectoryName(Application.ExecutablePath);
                _webBrowser.Navigate(Path.Combine(appDir, "Tutorial", "Tutorial.htm"));
            }
        }


        public override String Text
        {
            set
            {
                String dirtyMarker = String.Empty;
                if (this.Dirty)
                {
                    dirtyMarker = @" *";
                }
                base.Text = String.Format(@"{2} - {0}{1}", value, dirtyMarker, Resources.MainWindow_Text_Centipede);
            }
            get { return base.Text; }
        }

        private bool Dirty
        {
            get { return this._dirty; }
            set
            {
                this._dirty = value;
                this.Text = this.Core.Job.Name;
            }
        }

        private MessageLevel DisplayedLevels
        {
            get { return this._displayedLevels; }
            set
            {
                this._displayedLevels = value;
                this.UpdateOutputWindow();
            }
        }

        public ICentipedeCore Core { get; private set; }

        private void SetUserProperties()
        {
            SetAndIgnoreErrors(this, f => f.Height, Settings.Default.MainWindowHeight);
            SetAndIgnoreErrors(this, f => f.Width, Settings.Default.MainWindowWidth);
            SetAndIgnoreErrors(this, f => f.Location, Settings.Default.MainWindowLocation);
            SetAndIgnoreErrors(this, f => f.WindowState, Settings.Default.MainWindowState);
            SetAndIgnoreErrors(this, f => f.DisplayedLevels, Settings.Default.MessageFilterSetting);
            SetAndIgnoreErrors(this.SplitContainer1, s => s.SplitterDistance, Settings.Default.SplitContainer1Point);
            SetAndIgnoreErrors(this.SplitContainer2, s => s.SplitterDistance, Settings.Default.SplitContainer2Point);
            SetAndIgnoreErrors(this.SplitContainer3, s => s.SplitterDistance, Settings.Default.SplitContainer3Point);
        }

        private static void SetAndIgnoreErrors<T1, T2>(T1 arg1, Expression<Func<T1, T2>> selector, T2 arg2)
        {
            try
            {
                FieldAndPropertyWrapper.SetPropertyOnObject(arg1, selector, arg2);
            }
            catch
            {
                ;
            }
        }

        private void Core_Variables_PropertyChanged(object sender, PythonVariableChangedEventArgs args)
        {
            var scope = (PythonScope)sender;
            string name = args.PropertyName;
            dynamic value = scope.GetVariable(name);

            //DataSet1.VariablesTableRow row = (DataSet1.VariablesTableRow)args.Row;
            String message = string.Format(Resources.MainWindow_OnVariablesOnRowChanged_Message_Text,
                                           args.Action.ToString(),
                                           name,
                                           value);
            System.Action action = () => this._dataSet.Messages
                                             .AddMessagesRow(DateTime.Now,
                                                             message,
                                                             this.Core.CurrentAction,
                                                             MessageLevel.VariableChange,
                                                             this.DisplayedLevels.HasFlag(MessageLevel.VariableChange));

            try
            {
                Invoke(action);
            }
            catch (Exception e)
            { }
        }

        private void FavouriteItem_Click(object sender, EventArgs eventArgs)
        {
            var item = (ToolStripDropDownItem)sender;
            try
            {
                this.AskToSave();
            }
            catch (AbortOperationException)
            { }
            this.Core.LoadJob((String)item.Tag);
        }

        private void Action_MessageHandler(object sender, MessageEventArgs e)
        {
            //MessageBox.Show(String.Format("{0}\n\nLevel: {1}", e.Message, e.Level.AsText()), "Message");

            this.MessageDataGridView.Invoke(
                new Func<DateTime, String, IAction, MessageLevel, bool, JobDataSet.MessagesRow>(
                    this._dataSet.Messages.AddMessagesRow),
                DateTime.Now,
                e.Message,
                sender as Action,
                e.Level,
                this.DisplayedLevels.HasFlag(e.Level));
        }

        private void Core_ActionErrorOccurred(object sender, ActionErrorEventArgs e)
        {
            var messageBuilder = new StringBuilder();
            var exception = (ActionException)e.Exception;
            if (exception.ErrorAction != null)
            {
                //UpdateHandlerDone(e.ErrorAction);
                var adc = (ActionDisplayControl)exception.ErrorAction.Tag;

                SetState(adc, ActionState.Error, exception.Message);

                if (this.ActionContainer.InvokeRequired)
                {
                    this.ActionContainer.Invoke(new Action<Control>(this.ActionContainer.ScrollControlIntoView), adc);
                }
                else
                {
                    this.ActionContainer.ScrollControlIntoView(adc);
                }

                messageBuilder.AppendLine(Resources.MainWindow_ErrorHandler_Error_occurred_in_);
                messageBuilder.Append(exception.ErrorAction.Name).Append(@" (");
                messageBuilder.Append(exception.ErrorAction.Comment).AppendLine(@")");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(Resources.MainWindow_ErrorHandler_Message_was_);
                messageBuilder.AppendLine(exception.Message);
            }
            else
            {
                messageBuilder.AppendLine(Resources.MainWindow_ErrorHandler_Error_);
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(exception.Message);
            }

            DialogResult result = MessageBox.Show(messageBuilder.ToString(),
                                                  Resources.MainWindow_ErrorHandler_Error,
                                                  e.Fatal ? MessageBoxButtons.OK : MessageBoxButtons.AbortRetryIgnore,
                                                  MessageBoxIcon.Exclamation);

            this.MessageDataGridView.Invoke(
                new Func<DateTime, string, IAction, MessageLevel, bool, JobDataSet.MessagesRow>(
                    this._dataSet.Messages.AddMessagesRow),
                DateTime.Now,
                messageBuilder.ToString(),
                e.Action,
                MessageLevel.Error,
                this.DisplayedLevels.HasFlag(MessageLevel.Error));

            SetErrorReturnState(e, result);
        }

        private static void SetErrorReturnState(ActionErrorEventArgs e, DialogResult result)
        {
            var exception = ((ActionException)e.Exception);

            if (exception.ErrorAction == null)
            {
                e.NextAction = null;
                e.Continue = ContinueState.Abort;
                return;
            }

            switch (result)
            {
                case DialogResult.Abort:
                    e.NextAction = null;
                    e.Continue = ContinueState.Abort;
                    break;
                case DialogResult.Retry:
                    e.NextAction = exception.ErrorAction;
                    e.Continue = ContinueState.Retry;
                    break;
                case DialogResult.Ignore:
                    e.NextAction = exception.ErrorAction.GetNext();
                    e.Continue = ContinueState.Continue;
                    break;
                default:
                    e.NextAction = null;
                    e.Continue = ContinueState.Abort;
                    break;
            }
        }

        private void Core_ActionCompleted(object sender, ActionEventArgs actionEventArgs)
        {
            if (actionEventArgs.Stepping)
            {
                this._pendingUpdate = actionEventArgs;
            }

            this.DoAfterAction(actionEventArgs);
        }

        private void DoAfterAction(ActionEventArgs actionEventArgs)
        {
            IAction action = actionEventArgs.Action;

            var adc = (ActionDisplayControl)action.Tag;
            SetState(adc, ActionState.Completed, Resources.MainWindow_UpdateHandlerDone_Completed);

            //this is supposed to be a percentage, but this works.
            this.BackgroundWorker.ReportProgress(action.Complexity);
        }

        private static void SetState([NotNull] ActionDisplayControl adc,
                                     ActionState state,
                                     String message)
        {
            if (adc.InvokeRequired)
            {
                adc.Invoke(new Action<ActionDisplayControl, ActionState, string>(SetActionDisplayedState),
                           adc,
                           state,
                           message);
            }
            else
            {
                SetActionDisplayedState(adc, state, message);
            }
        }

        private void Core_JobCompleted(object sender, JobCompletedEventArgs e)
        {
            String message;
            MessageBoxIcon icon;
            if (e.Completed)
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
            this._stepping = false;
            this._steppingMutex = null;
            this.RunButton.Text = Resources.MainWindow_CompletedHandler_Run;
        }

        private void TabPageListView_ItemActivate(object sender, EventArgs e)
        {
            var sendingListView = (ListView)sender;

            var sendingActionFactory = (ActionFactory)sendingListView.SelectedItems[0];

            this.Core.AddAction(sendingActionFactory.Generate());
        }

        private static void SetActionDisplayedState([NotNull] ActionDisplayControl adc,
                                                    ActionState state,
                                                    String message)
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
            foreach (ToolStripButton button in from MessageLevel value in Enum.GetValues(typeof(MessageLevel))
                                               where (value != MessageLevel.All) && (value != MessageLevel.Default)
                                               select new ToolStripButton(value.AsText())
                                                      {
                                                          CheckOnClick = true,
                                                          Checked = this.DisplayedLevels.HasFlag(value),
                                                          Tag = value
                                                      })
            {
                button.Click += delegate(object sender1, EventArgs e1)
                                    {
                                        var btn = (ToolStripButton)sender1;
                                        this.DisplayedLevels = this.DisplayedLevels.SetFlag((MessageLevel)btn.Tag,
                                                                                            btn.Checked);
                                        this.UpdateOutputWindow();
                                    };
                this.MessageFilterToolStrip.Items.Add(button);
            }

            this.ActionIcons.Images.Add(@"generic", Resources.generic);

            //this.UIActTab.Tag = this.UIActListBox;

            this.AddToActionTab(typeof(DemoAction));
            this.AddToActionTab(typeof(GetFileNameAction));
            this.AddToActionTab(typeof(ShowMessageBox));
            this.AddToActionTab(typeof(AskValues));
            this.AddToActionTab(typeof(ExitAction));
            this.AddToActionTab(typeof(SubJobAction));
            this.AddToActionTab(typeof(MultipleChoice));
            this.AddToActionTab(typeof(AskBooleans));
            this.AddToActionTab(typeof(TestSerialize));
            this.AddToActionTab(typeof(TestDeserialize));
            this.AddToActionTab(typeof(SubJobEntry));
            this.AddToActionTab(typeof(SubJobExitPoint));
            
            this._urlTextbox = new ToolStripSpringTextBox();
            this._urlTextbox.KeyUp += this.UrlTextbox_KeyUp;
            this._urlTextbox.MergeIndex = this.NavigationToolbar.Items.Count - 2;

            this.NavigationToolbar.Items.Add(this._urlTextbox);

            this.NavigationToolbar.Stretch = true;

            this.GetActionPlugins();

            this.VarDataGridView.DataSource = (this.Core.Variables);

            //this.VarDataGridView.Columns.Add

            //this._dataSet.Variables.VariablesRowChanged += Variables_VariablesRowChanged;
            //this._dataSet.Variables.RowDeleted += DataStore_Variables_RowDeleted;

            foreach (RowStyle s in this.ActionContainer.RowStyles)
            {
                s.Height = 20f;
                s.SizeType = SizeType.AutoSize;
            }

            this.MessageDataGridView.DataSource = this.messagesBindingSource; //this._messageFilterBindingSource;

            this.Core.ActionCompleted += this.Core_ActionCompleted;
            this.Core.BeforeAction += this.Core_BeforeAction;
            this.Core.JobCompleted += this.Core_JobCompleted;
            this.Core.ActionErrorOccurred += this.Core_ActionErrorOccurred;
            this.Core.ActionAdded += this.Core_ActionAdded;
            this.Core.ActionRemoved += this.Core_ActionRemoved;
            this.Core.AfterLoad += this.Core_AfterLoad;
            this.Core.StartRun += this.Core_StartRun;

            //Core.Variables.OnUpdate += VariablesOnOnUpdate;

            Action.PluginFolder = Settings.Default.PluginFolder;

            if (Settings.Default.ListOfFavouriteJobs == null)
            {
                Settings.Default.ListOfFavouriteJobs = new StringCollection();
            }

            this.UpdateFromFaveFile();

            this.UpdateFavourites();

            this.Dirty = false;

            ActionDisplayControl.SetDirty = delegate { this.Dirty = true; };
        }

        private void UpdateFromFaveFile()
        {
            string faveFilename = EditFavourites.GetFaveFilename();
            string directoryName = Path.GetDirectoryName(faveFilename);
            if (directoryName == null)
            {
                throw new InvalidOperationException(@"Favourite file is set to root of a drive");
            }

            if (!Directory.Exists(directoryName) || !File.Exists(faveFilename))
            {
                return;
            }
            try
            {
                using (FileStream file = File.Open(faveFilename, FileMode.OpenOrCreate))
                {
                    this._favouriteJobsDataStore.Favourites.ReadXml(file);
                }

                Settings.Default.ListOfFavouriteJobs.AddRange(
                    from job in this._favouriteJobsDataStore.Favourites
                    select job.Filename);

                try
                {
                    Directory.Delete(directoryName, true);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
            }
            catch (XmlException)
            { }
        }

        private void Core_Job_PropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var job = (CentipedeJob)sender;

            switch (propertyChangedEventArgs.PropertyName)
            {
                case "InfoUrl":
                    this.WebBrowser.Navigate(job.InfoUrl);
                    break;
                case "Name":
                    this.Text = job.Name;
                    break;
            }
        }

        private void Core_StartRun(object sender, StartRunEventArgs startRunEventArgs)
        {
            this._steppingMutex = startRunEventArgs.ResetEvent;
            //this._stepping = true;
        }

        private void UpdateFavourites()
        {
            this.FavouritesMenu.DropDownItems.Clear();

            bool itemsAdded = false;

            foreach (string jobFilename in Settings.Default.ListOfFavouriteJobs)
            {
                ToolStripItem item;
                if (File.Exists(jobFilename))
                {
                    try
                    {
                        item = CentipedeJob.ToolStripItemFromFilename(jobFilename);
                        item.Click += this.FavouriteItem_Click;
                    }
                    catch (InvalidOperationException)
                    {
                        item = new ToolStripMenuItem
                               {
                                   Text = "<Invalid file format>",
                                   Enabled = false
                               };
                    }
                }
                else
                {
                    item = new ToolStripMenuItem
                           {
                               Text = "<Favourite item file not found>",
                               Enabled = false
                           };
                }
                this.FavouritesMenu.DropDownItems.Add(item);
                itemsAdded = true;
            }

            if (!itemsAdded)
            {
                this.FavouritesMenu.DropDownItems.Add(new ToolStripMenuItem
                                                      {
                                                          Text = "<No favourites set>",
                                                          Enabled = false
                                                      });
            }

            this.FavouritesMenu.DropDownItems.Add(this.FavoutiesMenuSeparator);
            this.FavouritesMenu.DropDownItems.Add(this.FavouritesAddCurrentMenuItem);
            this.FavouritesMenu.DropDownItems.Add(this.FavouritesEditFavouritesMenuItem);
        }

        private void GetActionPlugins()
        {
            Core.LoadActionPlugins();

            foreach (var action in Core.PluginFiles.SelectMany(kvp=>kvp.Value))
            {
                this.AddToActionTab(action);
            }
        }

        private void AddToActionTab(Type pluginType)
        {
            Assembly asm = pluginType.Assembly;
            Object[] customAttributes = pluginType.GetCustomAttributes(typeof(ActionCategoryAttribute), true);

            ActionCategoryAttribute catAttribute = customAttributes.OfType<ActionCategoryAttribute>().First();

            TabPage tabPage = this.AddActionTabs.TabPages
                                  .Cast<TabPage>().SingleOrDefault(tp => tp.Text == catAttribute.category);

            ListView catListView;
            if (tabPage != null)
            {
                catListView = (ListView)tabPage.Tag;
            }
            else
            {
                catListView = this.GenerateNewTabPage(catAttribute.category);
                catListView.ShowItemToolTips = true;
            }

            var af = new ActionFactory(catAttribute, pluginType, this.Core);

            if (!string.IsNullOrEmpty(catAttribute.IconName))
            {
                Type t = asm.GetType(string.Format(@"{0}.Properties.Resources", pluginType.Namespace));

                if (t != null)
                {
                    PropertyInfo iconProperty = t.GetProperty(catAttribute.IconName, typeof(Icon));
                    if (iconProperty != null)
                    {
                        var icon = iconProperty.GetValue(t, null) as Icon;
                        if (icon != null)
                        {
                            catListView.LargeImageList.Images.Add(pluginType.Name, icon);
                            af.ImageKey = pluginType.Name;
                        }
                    }
                }
            }

            catListView.Items.Add(af);
        }

        private void Core_AfterLoad(object sender, EventArgs e)
        {
            this.Dirty = false;
            this.Core.Job.PropertyChanged += this.Core_Job_PropertyChanged;
            this.Text = this.Core.Job.Name;
            this.ProgressBar.Value = 0;

            this.WebBrowser.Navigate(this.Core.Job.InfoUrl);
            this.ActionContainer.VerticalScroll.Maximum = 0;
        }

        private void Core_ActionRemoved(object sender, ActionEventArgs e)
        {
            IAction action = e.Action;
            var adc = (ActionDisplayControl)action.Tag;
            if (adc == null)
            {
                return;
            }
            this.ActionContainer.Controls.Remove(adc);
            this.Dirty = true;
        }

        private ListView GenerateNewTabPage(String category)
        {
            var lv = new ListView
                     {
                         LargeImageList = this.ActionIcons,
                         SmallImageList = this.ActionIcons,
                         View = View.List,
                         ListViewItemSorter = new ActionFactoryComparer()
                     };

            var tabPage = new TabPage(category);
            tabPage.Controls.Add(lv);
            lv.Dock = DockStyle.Fill;
            tabPage.Tag = lv;
            lv.ItemDrag += this.TabPageListView_BeginDrag;
            lv.ItemActivate += this.TabPageListView_ItemActivate;
            this.AddActionTabs.TabPages.Add(tabPage);

            return lv;
        }

        private void Core_BeforeAction(object sender, ActionEventArgs e)
        {
            if (this._pendingUpdate != null)
            {
                this.DoAfterAction(this._pendingUpdate);
                this._pendingUpdate = null;
            }

            IAction action = e.Action;
            var adc = (ActionDisplayControl)action.Tag;
            SetState(adc, ActionState.Running, Resources.MainWindow_Program_ActionStatus_Running);

            if (this.ActionContainer.InvokeRequired)
            {
                this.ActionContainer.Invoke(new Action<Control>(this.ActionContainer.ScrollControlIntoView), adc);
            }
            else
            {
                this.ActionContainer.ScrollControlIntoView(adc);
            }
        }

        private void Core_ActionAdded(object sender, ActionEventArgs e)
        {
            IAction action = e.Action;
            Type actionType = action.GetType();
            var actionAttribute = actionType.GetCustomAttributes(true)[0] as ActionCategoryAttribute;
            ActionDisplayControl adc;
            if (actionAttribute != null && actionAttribute.DisplayControl != null)
            {
                Type customADCType = actionType.Assembly.GetType(string.Format(@"{0}.{1}",
                                                                               actionType.Namespace,
                                                                               actionAttribute.DisplayControl));
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
            action.MessageHandler += this.Action_MessageHandler;
            adc.Deleted += this.ActionDisplayControl_Deleted;
            adc.DragEnter += this.ActionContainer_DragEnter;
            adc.DragDrop += this.ActionContainer_DragDrop;
            this.ActionContainer.Controls.Add(adc, 0, e.Index);
            this.ActionContainer.ScrollControlIntoView(adc);
            this.ActionContainer.SetRow(adc, e.Index);
            this.Dirty = true;

            if (!e.LoadedSuccessfully)
            {
                this._unloadablePlugins = true;
            }
        }

        private void ActionDisplayControl_Deleted(object sender, CentipedeEventArgs e)
        {
            IAction action = ((ActionDisplayControl)sender).ThisAction;

            if (MessageBox.Show(string.Format("Are you sure you want to delete {0}?", action),
                                "Delete Action",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Core.RemoveAction(action);
            }
        }

        private void DoLoad(string fileName)
        {
            this._unloadablePlugins = false;
            try
            {
                this.Core.LoadJob(fileName);
                this.Text = this.Core.Job.Name;
            }
            catch (PluginNotFoundException e)
            {
                MessageBox.Show(string.Format("Cannot load action {0}. {1}\nAttempting to run the job will fail.", e.ActionName, e.Message),
                                "Error loading job",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }

            if (this._unloadablePlugins)
            {
                MessageBox.Show("Some plugins could not be loaded.\nAttempting to run the job will fail.",
                                "Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (!this._stepping)
            {
                this.StartRunning(false);
            }
            else
            {
                this.DoStep();
            }
        }

        private void StartRunning(bool step)
        {
            this._stepping = step;
            this.ResetDisplay();
            this.BackgroundWorker.RunWorkerAsync(step);
        }

        private void ResetDisplay()
        {
            foreach (ActionDisplayControl adc in this.ActionContainer.Controls)
            {
                SetState(adc, ActionState.None, String.Empty);
            }
            this.ProgressBar.Value = 0;
            this.ProgressBar.Maximum = this.Core.Job.Complexity;
        }

        [STAThread]
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Core.RunJob(this._stepping);
        }

        private void MainWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                this.RunButton.PerformClick();
            }
            e.IsInputKey = true;
        }

        private void ActionContainer_DragDrop(object sender, DragEventArgs e)
        {
            Int32 index = -1;
            if (sender is ActionDisplayControl)
            {
                index = this.ActionContainer.GetPositionFromControl(sender as ActionDisplayControl).Row;
            }
            object data = e.Data.GetData(@"WindowsForms10PersistentObject");
            this.Core.AddAction(((ActionFactory)data).Generate(), index);
        }

        private void TabPageListView_BeginDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ActionContainer_DragEnter(object sender, DragEventArgs e)
        {
            //Fixme: Should type be checked in here?
            e.Effect = DragDropEffects.Move;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //It's not really a percentage, but it serves the purpose
            this.ProgressBar.Increment(e.ProgressPercentage);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch (NullReferenceException)
            { }
            catch (InvalidOperationException)
            { }
        }

        private void FileNewMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.AskToSave();
            }
            catch (AbortOperationException)
            {
                return;
            }
            this.ActionContainer.Controls.Clear();
            this.Core.Clear();
        }

        private void FileOpenMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.AskToSave();
            }
            catch (AbortOperationException)
            {
                return;
            }

            DialogResult result = this.OpenFileDialog.ShowDialog(this);

            if (result != DialogResult.OK)
            {
                return;
            }

            this.DoLoad(this.OpenFileDialog.FileName);
        }

        private void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var dlg = (SaveFileDialog)sender;
            this.Core.SaveJob(dlg.FileName);
            this.Dirty = false;
        }

        /// <summary>
        ///     Check if save needed, ask to save, and save
        /// </summary>
        /// <exception cref="AbortOperationException">
        ///     Throws abort operation exception if cancel is clicked at any
        ///     point
        /// </exception>
        private void AskToSave()
        {
            if (!this.Dirty)
            {
                return;
            }

            DialogResult result = MessageBox.Show(Resources.MainWindow_AskSave_Do_you_wish_to_save,
                                                  Resources.MainWindow_AskSave_Unsaved_Changes,
                                                  MessageBoxButtons.YesNoCancel,
                                                  MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Yes:
                    this.Save();
                    break;
                case DialogResult.No:
                    break;
                default:
                    throw new AbortOperationException();
            }
        }

        private void FileSaveMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void Save()
        {
            if (this.Core.Job.HasBeenSaved)
            {
                try
                {
                    this.ShowSaveDialogs();
                }
                catch (AbortOperationException)
                {
                    return;
                }
            }
            else
            {
                this.Core.SaveJob(this.Core.Job.FileName);
            }
            this.Dirty = false;
        }

        private void ShowSaveDialogs(bool force=false)
        {
            if (!force && !this.Dirty)
            {
                return;
            }
            if (String.IsNullOrEmpty(this.Core.Job.Name))
            {
                this.SetJobProperties();
            }

            this.SaveFileDialog.FileName = !String.IsNullOrEmpty(this.Core.Job.FileName)
                                               ? this.Core.Job.FileName
                                               : this.Core.Job.Name;

            if (this.SaveFileDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                throw new AbortOperationException();
            }
        }

        private void FileSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowSaveDialogs(true);
            }
            catch (AbortOperationException)
            { }
        }

        private void FileExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditFavouritesMenuItem_Click(object sender, EventArgs e)
        {
            //var editFavourites = new EditFavourites(Settings.Default.FavouriteJobs);
            var editFavourites = new EditFavourites();

            editFavourites.ShowDialog(this);

            this.UpdateFavourites();
        }

        private void UpdateOutputWindow()
        {
            this.MessageDataGridView.Parent.SuspendLayout();
            this.MessageDataGridView.Hide();
            foreach (JobDataSet.MessagesRow row in this._dataSet.Messages)
            {
                row.Visible = this.DisplayedLevels.HasFlag(row.Level);
            }
            this.MessageDataGridView.Show();
            this.MessageDataGridView.Parent.ResumeLayout(true);
        }

        //private void MessageDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //DataGridViewRow row = this.MessageDataGridView.Rows[e.RowIndex];
        //var messageRow = (JobDataSet.MessagesRow)this.jobDataSet1.Messages.Rows[e.RowIndex];
        //row.Visible = this._messageLevelsShown.HasFlag(messageRow.Level);
        //}

        private void UrlTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = ((ToolStripSpringTextBox)sender);
            string url = textBox.Text;

            if (e.KeyCode == Keys.Enter)
            {
                if (e.Control)
                {
                    url = string.Format(@"http://www.{0}.com", url);
                }

                this.WebBrowser.Navigate(url, e.Shift);

                textBox.Text = url;

                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void NavigationBackButton_Click(object sender, EventArgs e)
        {
            this.WebBrowser.GoBack();
        }

        private void NavigationForwardButton_Click(object sender, EventArgs e)
        {
            this.WebBrowser.GoForward();
        }

        private void NavigationRefresh_Click(object sender, EventArgs e)
        {
            this.WebBrowser.Refresh();
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (this.WebBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                this._urlTextbox.Text = e.Url.AbsoluteUri;
            }
        }

        private void FilePropertiesMenuItem_Click(object sender, EventArgs e)
        {
            bool changed;
            try
            {
                changed = this.SetJobProperties();
            }
            catch (AbortOperationException)
            {
                return;
            }

            if (changed)
            {
                this.Dirty = true;
                this.WebBrowser.Navigate(this.Core.Job.InfoUrl);
            }
        }

        private bool SetJobProperties()
        {
            CentipedeJob centipedeJob = this.Core.Job;
            var form = new JobPropertyForm(ref centipedeJob);

            DialogResult result = form.ShowDialog(this);

            if (result == DialogResult.Cancel)
            {
                throw new AbortOperationException();
            }

            return form.Dirty;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Core.Dispose();
            Dispose();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        { }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        { }

        private void FavouritesAddCurrentMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Dirty || !this.Core.Job.HasBeenSaved)
            {
                if (MessageBox.Show("This job must be saved before it can be added as a favourite.",
                                    "Save File",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        this.Save();
                    }
                    catch (AbortOperationException)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            Settings.Default.ListOfFavouriteJobs.Add(this.Core.Job.FileName);
            this.UpdateFavourites();
        }

        private void OutputClear_Click(object sender, EventArgs e)
        {
            this._dataSet.Messages.Clear();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.AskToSave();
            }
            catch (AbortOperationException)
            {
                e.Cancel = true;
            }
            Settings.Default.MainWindowState = WindowState;
            Settings.Default.MessageFilterSetting = this.DisplayedLevels;
            Settings.Default.MainWindowHeight = Height;
            Settings.Default.MainWindowWidth = Width;
            Settings.Default.MainWindowLocation = Location;
            Settings.Default.SplitContainer1Point = this.SplitContainer1.SplitterDistance;
            Settings.Default.SplitContainer2Point = this.SplitContainer2.SplitterDistance;
            Settings.Default.SplitContainer3Point = this.SplitContainer3.SplitterDistance;

            Settings.Default.Save();
        }

        private void visitGetSatisfactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Resources.MainWindow_visitGetSatisfactionToolStripMenuItem_Click_GetSatisfaction_Url);
        }

        private void HelpAboutMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutForm(Core.PluginFiles)).Show(this);
        }

        private void RunStepThroughMenuItem_Click(object sender, EventArgs e)
        {
            if (!this._stepping)
            {
                this.RunButton.Text = Resources.MainWindow_stepThroughToolStripMenuItem_Click_Step;
                this.StartRunning(true);
            }
            else
            {
                this.DoStep();
            }
        }

        private void DoStep()
        {
            this._steppingMutex.Set();
        }

        private void RunAbortMenuItem_Click(object sender, EventArgs e)
        {
            this.Core.AbortRun();
        }

        private void RunResetJobMenuItem_Click(object sender, EventArgs e)
        {
            this.Core.AbortRun();

            this.ResetDisplay();
            this._dataSet.Messages.Clear();
        }

        private void HelpPythonReferenceMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://docs.python.org/2.7/");
        }

        public void LoadJobAfterLoad(string file)
        {
            Load += (sender, args) => this.DoLoad(file);
        }

        public void RunJobAfterLoad()
        {
            Load += (sender, args) => this.StartRunning(false);
        }

        private void HelpOpenTutorialMenuItem_Click(object sender, EventArgs e)
        {
            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            WebBrowser.Navigate(Path.Combine(appDir, "Tutorial","Tutorial.htm"));
        }

    }

    internal class ActionFactoryComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            var a = (ActionFactory)x;
            var b = (ActionFactory)y;
            return String.Compare(a.Text, b.Text, CultureInfo.CurrentCulture, CompareOptions.StringSort);
        }

        #endregion
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

        [UsedImplicitly]
        public AbortOperationException(string message)
            : base(message)
        { }

        [UsedImplicitly]
        public AbortOperationException(string message, Exception inner)
            : base(message, inner)
        { }

        protected AbortOperationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }
    }
}
