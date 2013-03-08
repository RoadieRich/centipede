using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Centipede.Actions;
using Centipede.Properties;

using CentipedeInterfaces;
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
    public partial class MainWindow : Form
    {
        public static MainWindow Instance;
        //private readonly JobDataSet _dataSet = new JobDataSet();

        private readonly FavouriteJobs _favouriteJobsDataStore = new FavouriteJobs();

        [UsedImplicitly]
        private Dictionary<string, string> _arguments;

        private bool _dirty;
        private ToolStripSpringTextBox _urlTextbox;
        private MessageLevel _displayedLevels;
        private ManualResetEvent _steppingMutex;
        private bool _stepping;
        private Keys _modifierState;

        public MainWindow(ICentipedeCore centipedeCore, Dictionary<string, string> arguments = null)
        {
            //Visible = false;

            InitializeComponent();

            Height = Settings.Default.MainWindowHeight;
            Width = Settings.Default.MainWindowWidth;
            Location = Settings.Default.MainWindowLocation;
            WindowState = Settings.Default.MainWindowState;

            SplitContainer1.SplitterDistance = Settings.Default.SplitContainer1Point;
            SplitContainer2.SplitterDistance = Settings.Default.SplitContainer2Point;
            SplitContainer3.SplitterDistance = Settings.Default.SplitContainer3Point;
            
            WebBrowser.DocumentText = Resources.WelcomeScreen;

            //Visible = true;

            Core = centipedeCore;
            Instance = this;
            ActionFactory.MessageHandlerDelegate = OnMessageHandlerDelegate;

            this._arguments = arguments;

            Core.Variables.RowChanged += OnVariablesOnRowChanged;



            _dataSet.Messages.RowChanged += delegate
                                            {
                                                this.MessageDataGridView.Invalidate();
                                                this.MessageDataGridView.Update();
                                            };

            MessageDataGridView.CellFormatting += delegate(object sender, DataGridViewCellFormattingEventArgs args)
                                                  {
                                                      if (args.Value is IAction)
                                                          args.Value = args.Value.ToString();
                                                  };

            DisplayedLevels = Settings.Default.MessageFilterSetting;
        }

        private void OnVariablesOnRowChanged(object sender, DataRowChangeEventArgs args)
        {
            DataSet1.VariablesTableRow row = (DataSet1.VariablesTableRow)args.Row;
            String message = string.Format("{1} variable {{{0}}} : {2}", row.Name, args.Action.ToString(), row.Value);
            System.Action action =
                    () => this._dataSet.Messages.AddMessagesRow(DateTime.Now, message, this.Core.CurrentAction,
                                                                MessageLevel.VariableChange,
                                                                DisplayedLevels.HasFlag(MessageLevel.VariableChange));

            Invoke(action);
        }

        private void OnMessageHandlerDelegate(object sender, MessageEventArgs e)
        {
            this._dataSet.Messages.AddMessagesRow(DateTime.Now, e.Message, sender as Action, e.Level,
                                                  DisplayedLevels.HasFlag(e.Level));

           
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
            get
            {
                return base.Text;
            }
        }

        private bool Dirty
        {
            get
            {
                return this._dirty;
            }
            set
            {
                this._dirty = value;
                Text = Core.Job.Name;
            }
        }

        public ICentipedeCore Core { get; private set; }

        private void ErrorHandler(object sender, ActionErrorEventArgs e)
        {
            var messageBuilder = new StringBuilder();
            ActionException exception = (ActionException)e.Exception;
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
                                                  MessageBoxButtons.AbortRetryIgnore,
                                                  MessageBoxIcon.Exclamation);

            SetErrorReturnState(e, result);
        }

        private static void SetErrorReturnState(ActionErrorEventArgs e, DialogResult result)
        {
            ActionException exception = ((ActionException)e.Exception);

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

        private void UpdateHandlerDone(object sender, ActionEventArgs actionEventArgs)
        {
            //foreach (var v in Core.Variables.ToArray())
            //{
            //    if (v.Key.StartsWith(@"_"))
            //    {
            //        continue;
            //    }
            //    KeyValuePair<string, object> v1 = v;
            //    JobDataSet.VariablesRow row = this._dataSet.Variables.Rows
            //                                      .OfType<JobDataSet.VariablesRow>()
            //                                      .First(r => r.Name == v1.Key);
            //    if (row != null)
            //    {
            //        row.Value = v.Value;
            //    }
            //    else
            //    {
            //        this._dataSet.Variables.AddVariablesRow(v.Key, v.Value);
            //    }
            //}
            var adc = (ActionDisplayControl)Core.CurrentAction.Tag;
            SetState(adc, ActionState.Completed, Resources.MainWindow_UpdateHandlerDone_Completed);

            if (this.ActionContainer.InvokeRequired)
            {
                this.ActionContainer.Invoke(new Action<Control>(this.ActionContainer.ScrollControlIntoView), adc);
            }
            else
            {
                this.ActionContainer.ScrollControlIntoView(adc);
            }

            //this is supposed to be a percentage, but this works.
            this.backgroundWorker1.ReportProgress(Core.CurrentAction.Complexity);
            //UpdateOutputWindow();
        }

        private static void SetState([NotNull] ActionDisplayControl adc, ActionState state,
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

        private void CompletedHandler(object sender, JobCompletedEventArgs e)
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
        }

        private void ItemActivate(object sender, EventArgs e)
        {
            var sendingListView = (ListView)sender;

            var sendingActionFactory = (ActionFactory)sendingListView.SelectedItems[0];

            Core.AddAction(sendingActionFactory.Generate());
        }

        private static void _setActionDisplayedState([NotNull] ActionDisplayControl adc,
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
            foreach (ToolStripButton button in from MessageLevel value in Enum.GetValues(typeof(MessageLevel))
                                               where value != MessageLevel.All
                                               select new ToolStripButton(Enum.GetName(typeof(MessageLevel), value))
                                                      {
                                                          CheckOnClick = true,
                                                          Checked = DisplayedLevels.HasFlag(value),
                                                          Tag = value
                                                      })
            {
                button.Click += delegate(object sender1, EventArgs e1)
                                {
                                    ToolStripButton btn = (ToolStripButton)sender1;
                                    DisplayedLevels = DisplayedLevels.SetFlag((MessageLevel)btn.Tag,
                                                                              btn.Checked);
                                    UpdateOutputWindow();
                                };
                this.MessageFilterToolStrip.Items.Add(button);
            }

            this.UIActListBox.LargeImageList.Images.Add(@"generic", Resources.generic);

            this.UIActTab.Tag = this.UIActListBox;

            AddToActionTab(typeof(DemoAction));
            AddToActionTab(typeof(GetFileNameAction));
            AddToActionTab(typeof(ShowMessageBox));
            AddToActionTab(typeof(AskValues));
            AddToActionTab(typeof(ExitAction));
            AddToActionTab(typeof(SubJob));
            AddToActionTab(typeof(MultipleChoice));
            AddToActionTab(typeof(AskBooleans));

            this._urlTextbox = new ToolStripSpringTextBox();
            this._urlTextbox.KeyUp += UrlTextbox_KeyUp;
            this.NavigationToolbar.Stretch = true;
            this.NavigationToolbar.Items.Add(this._urlTextbox);
            this._urlTextbox.MergeIndex = this.NavigationToolbar.Items.Count - 2;
            GetActionPlugins();

            this.VarDataGridView.DataSource = Core.Variables;

            //this.VarDataGridView.Columns.Add


            //this._dataSet.Variables.VariablesRowChanged += Variables_VariablesRowChanged;
            //this._dataSet.Variables.RowDeleted += DataStore_Variables_RowDeleted;

            foreach (RowStyle s in this.ActionContainer.RowStyles)
            {
                s.Height = 20f;
                s.SizeType = SizeType.AutoSize;
            }

            MessageDataGridView.DataSource = this.messagesBindingSource; //this._messageFilterBindingSource;

            Core.ActionCompleted += UpdateHandlerDone;
            Core.BeforeAction += Program_BeforeAction;
            Core.JobCompleted += CompletedHandler;
            Core.ActionErrorOccurred += ErrorHandler;
            Core.ActionAdded += Program_ActionAdded;
            Core.ActionRemoved += Program_ActionRemoved;
            Core.AfterLoad += Program_AfterLoad;
            Core.StartStepping += CoreOnStartStepping;
            //Core.Variables.OnUpdate += VariablesOnOnUpdate;



            using (FileStream file = File.Open(EditFavourites.GetFaveFilename(), FileMode.OpenOrCreate))
            {
                try
                {
                    this._favouriteJobsDataStore.Favourites.ReadXml(file);
                }
                catch (XmlException)
                { }
            }
            UpdateFavourites();

            Dirty = false;

            ActionDisplayControl.SetDirty = delegate { Dirty = true; };

        }

        private void CoreOnStartStepping(object sender, StartSteppingEventArgs startSteppingEventArgs)
        {
            this._steppingMutex = startSteppingEventArgs.ResetEvent;
            this._stepping = true;
        }

        private void UpdateFavourites()
        {
            this.FavouritesMenu.DropDownItems.Clear();

            foreach (
                    ToolStripMenuItem item in
                            this._favouriteJobsDataStore.Favourites.Select(job => new ToolStripMenuItem(job.Name)
                                                                                  {
                                                                                      Tag = job.Filename
                                                                                  }))
            {
                item.Click += ItemOnClick;
                this.FavouritesMenu.DropDownItems.Add(item);
            }

            this.FavouritesMenu.DropDownItems.Add(this.FaveMenuSeparator);
            this.FavouritesMenu.DropDownItems.Add(this.EditFavouritesMenuItem);
        }

        private void ItemOnClick(object sender, EventArgs eventArgs)
        {
            var item = (ToolStripDropDownItem)sender;
            try
            {
                AskSave();
            }
            catch (AbortOperationException)
            { }
            Core.Job = Core.LoadJob((String)item.Tag);
        }

        private void GetActionPlugins()
        {
            var dir = new DirectoryInfo(Path.Combine(Application.StartupPath, Settings.Default.PluginFolder));

            IEnumerable<FileInfo> dlls = dir.EnumerateFiles(@"*.dll", SearchOption.AllDirectories);

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
                                            where type.GetCustomAttributes(typeof(ActionCategoryAttribute), true).Any()
                                            select type)
                {
                    AddToActionTab(pluginType);
                }
            }
        }

        private void AddToActionTab(Type pluginType)
        {
            Assembly asm = pluginType.Assembly;
            Object[] customAttributes = pluginType.GetCustomAttributes(typeof(ActionCategoryAttribute), true);

            ActionCategoryAttribute catAttribute = customAttributes.Cast<ActionCategoryAttribute>().First();

            TabPage tabPage =
                    this.AddActionTabs.TabPages.Cast<TabPage>().SingleOrDefault(tp => tp.Text == catAttribute.category);

            ListView catListView;
            if (tabPage != null)
            {
                catListView = (ListView)tabPage.Tag;
            }
            else
            {
                catListView = GenerateNewTabPage(catAttribute.category);
            }

            var af = new ActionFactory(catAttribute, pluginType, Core);

            if (!string.IsNullOrEmpty(catAttribute.iconName))
            {
                Type t = asm.GetType(pluginType.Namespace + @".Properties.Resources");
                		

                if (t != null)
                {
                    var icon = t.GetProperty(catAttribute.iconName, typeof(Icon)).GetValue(t, null) as Icon;
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
            Text = Core.Job.Name;

            this.WebBrowser.Navigate(Core.Job.InfoUrl);
        }

        private void Program_ActionRemoved(object sender, ActionEventArgs e)
        {
            IAction action = e.Action;
            ActionDisplayControl adc = this.ActionContainer.Controls
                                           .Cast<ActionDisplayControl>()
                                           .FirstOrDefault(a => a.ThisAction == action);
            if (adc == null)
            {
                return;
            }
            this.ActionContainer.Controls.Remove(adc);
            Dirty = true;
        }

        private ListView GenerateNewTabPage(String category)
        {
            var lv = new ListView
                     {
                             LargeImageList = this.ActionIcons,
                             SmallImageList = this.ActionIcons,
                             View = View.List
                     };

            var tabPage = new TabPage(category);
            tabPage.Controls.Add(lv);
            lv.Dock = DockStyle.Fill;
            tabPage.Tag = lv;
            lv.ItemDrag += BeginDrag;
            lv.ItemActivate += ItemActivate;
            this.AddActionTabs.TabPages.Add(tabPage);

            return lv;
        }

        private static void Program_BeforeAction(object sender, ActionEventArgs e)
        {
            IAction action = e.Action;
            var adc = (ActionDisplayControl)action.Tag;
            SetState(adc, ActionState.Running, Resources.MainWindow_Program_ActionStatus_Running);
        }

        private void Program_ActionAdded(object sender, ActionEventArgs e)
        {
            IAction action = e.Action;
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
            this.ActionContainer.Controls.Add(adc, 0, e.Index);
            this.ActionContainer.SetRow(adc, e.Index);
            Dirty = true;
        }

        private void adc_Deleted(object sender, CentipedeEventArgs e)
        {
            var adc = (ActionDisplayControl)sender;
            Core.RemoveAction(adc.ThisAction);
        }

        private void VarMenuDelete_Click(object sender, EventArgs e)
        { }

        private void DoLoad()
        {
            try
            {
                AskSave();
            }
            catch (AbortOperationException)
            { }

            DialogResult result = this.OpenFileDialog.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            Core.Job = Core.LoadJob(this.OpenFileDialog.FileName);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            foreach (ActionDisplayControl adc in this.ActionContainer.Controls)
            {
                SetState(adc, ActionState.None, String.Empty);
            }
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = Core.Job.Complexity;
            Boolean step = _modifierState.HasFlag(Keys.Control);
            this.backgroundWorker1.RunWorkerAsync(step);
        }

        [STAThread]
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Core.RunJob((bool)e.Argument);
        }

        private void MainWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            

            this._modifierState = e.Modifiers;

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
            Core.AddAction(((ActionFactory)data).Generate(), index);
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
            if (String.IsNullOrEmpty(Core.Job.Name))
            {
                var jobPropertyForm = new JobPropertyForm(Core);
                DialogResult result = jobPropertyForm.ShowDialog(this);
                if (result == DialogResult.Cancel)
                {
                    throw new AbortOperationException();
                }
            }
            this.saveFileDialog1.FileName = Core.Job.FileName;
            if (this.saveFileDialog1.ShowDialog(this) == DialogResult.Cancel)
            {
                throw new AbortOperationException();
            }

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Core.SaveJob(this.saveFileDialog1.FileName);
            Dirty = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //It's not really a percentage, but it serves the purpose
            this.progressBar1.Increment(e.ProgressPercentage);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Core.Dispose();
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
            this.ActionContainer.Controls.Clear();
            Core.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoLoad();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Core.Job.FileName))
            {
                try
                {
                    SaveJob();
                }
                catch (AbortOperationException)
                {
                    return;
                }
            }
            else
            {
                Core.SaveJob(Core.Job.FileName);
            }
            Dirty = false;
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveJob();
            }
            catch (AbortOperationException)
            {

            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditFavouritesMenuItem_Click(object sender, EventArgs e)
        {
            var editFavourites = new EditFavourites(this._favouriteJobsDataStore);

            editFavourites.ShowDialog();

            UpdateFavourites();
        }

        private MessageLevel DisplayedLevels
        {
            get
            {
                return this._displayedLevels;
            }
            set
            {
                this._displayedLevels = value;
                UpdateOutputWindow();
            }
        }

        private void UpdateOutputWindow()
        {
            foreach (JobDataSet.MessagesRow row in _dataSet.Messages)
            {
                row.Visible = DisplayedLevels.HasFlag(row.Level);
            }

            // return;

            //MessageDataGridView.CurrentCell = null;
            //foreach (DataGridViewRow row in MessageDataGridView.Rows)
            //{
            //    row.Visible = DisplayedLevels.HasFlag((MessageLevel)row.Cells[3].Value);
            //}
            ////MessageDataGridView.Show();
            ////MessageDataGridView.PerformLayout();
            //MessageDataGridView.Focus();
        }

        private void MessageDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //DataGridViewRow row = this.MessageDataGridView.Rows[e.RowIndex];
            //var messageRow = (JobDataSet.MessagesRow)this.jobDataSet1.Messages.Rows[e.RowIndex];
            //row.Visible = this._messageLevelsShown.HasFlag(messageRow.Level);
        }

        private void UrlTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = ((ToolStripTextBox)sender);
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.WebBrowser.GoBack();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.WebBrowser.GoForward();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new JobPropertyForm(Core);
            form.ShowDialog(this);
        }

        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Core.Dispose();

            

            base.Dispose();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
        }

        private void addCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(Core.Job.Name))
            {
                try
                {
                    SaveJob();
                }
                catch (AbortOperationException)
                {
                    return;
                }
            }
            _favouriteJobsDataStore.Favourites.AddFavouritesRow(Core.Job.Name, Core.Job.FileName);
            UpdateFavourites();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            _dataSet.Messages.Clear();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                AskSave();
            }
            catch (AbortOperationException)
            {
                e.Cancel = true;
            }
            Settings.Default.MainWindowState = WindowState;
            Settings.Default.MessageFilterSetting = DisplayedLevels;
            Settings.Default.MainWindowHeight = Height;
            Settings.Default.MainWindowWidth = Width;
            Settings.Default.MainWindowLocation = Location;
            Settings.Default.SplitContainer1Point = SplitContainer1.SplitterDistance;
            Settings.Default.SplitContainer2Point = SplitContainer2.SplitterDistance;
            Settings.Default.SplitContainer3Point = SplitContainer3.SplitterDistance;

            Settings.Default.Save();
        }

        private void visitGetSatisfactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://getsatisfaction.com/centipede");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutForm()).Show(this);
        }

        private void stepThroughToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this._stepping)
            {
                this._steppingMutex.Set();
            }
            else
            {
                backgroundWorker1.RunWorkerAsync(true);
            }
        }

        private void abortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.AbortRun();
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

