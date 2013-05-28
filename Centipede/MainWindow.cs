using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
    public partial class MainWindow : Form
    {
        #region Constructors

        public MainWindow(ICentipedeCore centipedeCore, Dictionary<string, string> arguments = null)
        {
            this._pendingUpdate = null;
            //Visible = false;

            InitializeComponent();
            this.SetUserProperties();

            this.WebBrowser.DocumentText = Resources.WelcomeScreen;

            //Visible = true;

            Core = centipedeCore;

            ActionFactory.MessageEvent = OnMessageHandlerDelegate;

            this._arguments = arguments;

            Core.Variables.PropertyChanged += OnVariablesOnRowChanged;

            this._dataSet.Messages.RowChanged += delegate
                                                     {
                                                         this.MessageDataGridView.Invalidate();
                                                         this.MessageDataGridView.Update();
                                                     };

            this.MessageDataGridView.CellFormatting += delegate(object sender, DataGridViewCellFormattingEventArgs args)
                                                           {
                                                               if (args.Value is IAction)
                                                                   args.Value = args.Value.ToString();
                                                           };

            this.DisplayedLevels = Settings.Default.MessageFilterSetting;

            this.Core.Window = this;
        }

        private void DemoMethod()
        {
            Object objectToReflect = null;
            Type type = objectToReflect.GetType();
            PropertyInfo property = type.GetProperty("PropertyName");
            object value1 = property.GetValue(objectToReflect, null);
            FieldInfo field = type.GetField("FieldName");
            object value2 = field.GetValue(objectToReflect);
        }

        private void SetUserProperties()
        { 

            IgnoreErrorsIn(FieldAndPropertyWrapper.SetPropertyOnObject, this, 
                f => f.Height, Settings.Default.MainWindowHeight);
            IgnoreErrorsIn(FieldAndPropertyWrapper.SetPropertyOnObject, this, 
                f => f.Width, Settings.Default.MainWindowWidth);
            IgnoreErrorsIn(FieldAndPropertyWrapper.SetPropertyOnObject, this, 
                f => f.Location, Settings.Default.MainWindowLocation);
            IgnoreErrorsIn(FieldAndPropertyWrapper.SetPropertyOnObject, this, 
                f => f.WindowState, Settings.Default.MainWindowState);
            IgnoreErrorsIn(FieldAndPropertyWrapper.SetPropertyOnObject, this.SplitContainer1, 
                s => s.SplitterDistance, Settings.Default.SplitContainer1Point);
            IgnoreErrorsIn(FieldAndPropertyWrapper.SetPropertyOnObject, this.SplitContainer2, 
                s => s.SplitterDistance, Settings.Default.SplitContainer2Point);
            IgnoreErrorsIn(FieldAndPropertyWrapper.SetPropertyOnObject, this.SplitContainer3, 
                s => s.SplitterDistance, Settings.Default.SplitContainer3Point);
            
        }
        
        private static void IgnoreErrorsIn<T1, T2>(Action<T1, Expression<Func<T1, T2>>, T2> function, T1 arg1, Expression<Func<T1, T2>> selector, T2 arg2, IEnumerable<Type> exceptionsToIgnore = null)
        {
            try
            {
                function(arg1, selector, arg2);
            }
            catch (Exception e)
            {
                if (exceptionsToIgnore != null)
                {
                    if (exceptionsToIgnore.Contains(e.GetType()))
                    {
                        Debug.WriteLine(e);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        

        #endregion

        #region Fields

        //private readonly JobDataSet _dataSet = new JobDataSet();

        private readonly FavouriteJobs _favouriteJobsDataStore = new FavouriteJobs();

        [UsedImplicitly]
        private Dictionary<string, string> _arguments;

        private bool _dirty;

        private ToolStripSpringTextBox _urlTextbox;

        private MessageLevel _displayedLevels;

        private ManualResetEvent _steppingMutex;

        private bool _stepping;
        private readonly Dictionary<FileInfo, List<Type>> _pluginFiles = new Dictionary<FileInfo, List<Type>>();

        private ActionEventArgs _pendingUpdate;

        #endregion

        #region Properties

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

        public ICentipedeCore Core { get; private set; }

        #endregion

        #region Methods

        private void OnVariablesOnRowChanged(object sender, PythonVariableChangedEventArgs args)
        {
            var scope = (PythonScope)sender;
            var name = args.PropertyName;
            var value = scope.GetVariable(name);

            //DataSet1.VariablesTableRow row = (DataSet1.VariablesTableRow)args.Row;
            String message = string.Format(Resources.MainWindow_OnVariablesOnRowChanged_Message_Text, args.Action.ToString(), name, value);
            System.Action action = () => this._dataSet.Messages
                                             .AddMessagesRow(DateTime.Now, message, Core.CurrentAction,
                                                             MessageLevel.VariableChange,
                                                             DisplayedLevels.HasFlag(MessageLevel.VariableChange));

            Invoke(action);
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
            Core.LoadJob((String)item.Tag);
        }

        private void OnMessageHandlerDelegate(object sender, MessageEventArgs e)
        {
            //MessageBox.Show(String.Format("{0}\n\nLevel: {1}", e.Message, e.Level.AsText()), "Message");

            this._dataSet.Messages.AddMessagesRow(DateTime.Now, e.Message, sender as Action, e.Level,
                                                  DisplayedLevels.HasFlag(e.Level));
        }

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
                                                  e.Fatal ? MessageBoxButtons.OK : MessageBoxButtons.AbortRetryIgnore,
                                                  MessageBoxIcon.Exclamation);
            
            _dataSet.Messages.AddMessagesRow(DateTime.Now, messageBuilder.ToString(), e.Action, MessageLevel.Error,
                                             DisplayedLevels.HasFlag(MessageLevel.Error));

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
            if (actionEventArgs.Stepping)
            {
                this._pendingUpdate = actionEventArgs;
            }

            DoAfterAction(actionEventArgs);

        }

        private void DoAfterAction(ActionEventArgs actionEventArgs)
        {
            IAction action = actionEventArgs.Action;

            var adc = (ActionDisplayControl)action.Tag;
            SetState(adc, ActionState.Completed, Resources.MainWindow_UpdateHandlerDone_Completed);

            //this is supposed to be a percentage, but this works.
            this.backgroundWorker1.ReportProgress(action.Complexity);
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
            this.RunButton.Text = Resources.MainWindow_CompletedHandler_Run;
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
                                               where (value != MessageLevel.All) && (value != MessageLevel.Default)
                                               select new ToolStripButton(value.AsText())
                                                      {
                                                          CheckOnClick = true,
                                                          Checked      = DisplayedLevels.HasFlag(value),
                                                          Tag          = value
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

            AddToActionTab( typeof( DemoAction        ));
            AddToActionTab( typeof( GetFileNameAction ));
            AddToActionTab( typeof( ShowMessageBox    ));
            AddToActionTab( typeof( AskValues         ));
            AddToActionTab( typeof( ExitAction        ));
            AddToActionTab( typeof( SubJobAction      ));
            AddToActionTab( typeof( MultipleChoice    ));
            AddToActionTab( typeof( AskBooleans       ));

            this._urlTextbox               = new ToolStripSpringTextBox();
            this._urlTextbox.KeyUp        += UrlTextbox_KeyUp;
            this._urlTextbox.MergeIndex    = this.NavigationToolbar.Items.Count - 2;

            this.NavigationToolbar.Items.Add(this._urlTextbox);
            
            this.NavigationToolbar.Stretch = true;

            GetActionPlugins();

            this.VarDataGridView.DataSource = (Core.Variables);

            //this.VarDataGridView.Columns.Add


            //this._dataSet.Variables.VariablesRowChanged += Variables_VariablesRowChanged;
            //this._dataSet.Variables.RowDeleted += DataStore_Variables_RowDeleted;

            foreach (RowStyle s in this.ActionContainer.RowStyles)
            {
                s.Height = 20f;
                s.SizeType = SizeType.AutoSize;
            }

            MessageDataGridView.DataSource = this.messagesBindingSource; //this._messageFilterBindingSource;

            Core.ActionCompleted     += UpdateHandlerDone;
            Core.BeforeAction        += Program_BeforeAction;
            Core.JobCompleted        += CompletedHandler;
            Core.ActionErrorOccurred += ErrorHandler;
            Core.ActionAdded         += Program_ActionAdded;
            Core.ActionRemoved       += Program_ActionRemoved;
            Core.AfterLoad           += Program_AfterLoad;
            Core.StartRun            += CoreOnStartRun;

            //Core.Variables.OnUpdate += VariablesOnOnUpdate;

            Action.PluginFolder = Settings.Default.PluginFolder;

            if (Settings.Default.ListOfFavouriteJobs == null)
            {
                Settings.Default.ListOfFavouriteJobs = new StringCollection();
            }


            this.UpdateFromFaveFile();

            UpdateFavourites();

            Dirty = false;

            ActionDisplayControl.SetDirty = delegate { Dirty = true; };
        }

        private void UpdateFromFaveFile()
        {
            string faveFilename = EditFavourites.GetFaveFilename();
            if (Directory.Exists(Path.GetDirectoryName(faveFilename)) && File.Exists(faveFilename))
            {
                try
                {
                    using (FileStream file = File.Open(faveFilename, FileMode.OpenOrCreate))
                    {
                        this._favouriteJobsDataStore.Favourites.ReadXml(file);
                    }

                    Settings.Default.ListOfFavouriteJobs.AddRange(
                        from job in this._favouriteJobsDataStore.Favourites
                        select job.Filename
                        );

                    try
                    {
                        Directory.Delete(Path.GetDirectoryName(faveFilename), true);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                }
                catch (XmlException)
                { }
            }
        }

        private void CentipedeJobOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            CentipedeJob job = (CentipedeJob)sender;

            switch (propertyChangedEventArgs.PropertyName)
            {
            case "InfoUrl":
                WebBrowser.Navigate(job.InfoUrl);
                break;
            case "Name":
                Text = job.Name;
                break;
            }
        }

        private void CoreOnStartRun(object sender, StartRunEventArgs startRunEventArgs)
        {
            this._steppingMutex = startRunEventArgs.ResetEvent;
            //this._stepping = true;
        }

        private void UpdateFavourites()
        {
            this.FavouritesMenu.DropDownItems.Clear();

            //foreach (var job in this._favouriteJobsDataStore.Favourites)
            
            foreach (string jobFilename in Settings.Default.ListOfFavouriteJobs)
            {
                var item = CentipedeJob.ToolStripItemFromFilename(jobFilename);
                item.Click += this.ItemOnClick;
                this.FavouritesMenu.DropDownItems.Add(item);
            }

            this.FavouritesMenu.DropDownItems.Add(this.FaveMenuSeparator);
            this.FavouritesMenu.DropDownItems.Add(this.addCurrentToolStripMenuItem);
            this.FavouritesMenu.DropDownItems.Add(this.EditFavouritesMenuItem);
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

                var actionTypes = (from type in pluginsInFile
                                   where !type.IsAbstract
                                   where type.GetCustomAttributes(typeof (ActionCategoryAttribute), true).Any()
                                   select type).ToArray();

                if (!actionTypes.Any())
                {
                    continue;
                }
                this._pluginFiles.Add(fi, new List<Type>());

                foreach (Type pluginType in actionTypes)
                {
                    this._pluginFiles[fi].Add(pluginType);
                    AddToActionTab(pluginType);
                }
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
                catListView = GenerateNewTabPage(catAttribute.category);
            }

            var af = new ActionFactory(catAttribute, pluginType, Core);

            if (!string.IsNullOrEmpty(catAttribute.iconName))
            {
                Type t = asm.GetType(string.Format(@"{0}.Properties.Resources", pluginType.Namespace));
                
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
            Core.Job.PropertyChanged += CentipedeJobOnPropertyChanged;
            Text = Core.Job.Name;
            progressBar1.Value = 0;

            this.WebBrowser.Navigate(Core.Job.InfoUrl);
            this.ActionContainer.VerticalScroll.Maximum = 0;
        }

        private void Program_ActionRemoved(object sender, ActionEventArgs e)
        {
            IAction action = e.Action;
            ActionDisplayControl adc = (ActionDisplayControl)action.Tag;
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

        private void Program_BeforeAction(object sender, ActionEventArgs e)
        {
            if (this._pendingUpdate != null)
            {
                DoAfterAction(_pendingUpdate);
                _pendingUpdate = null;
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

        private void Program_ActionAdded(object sender, ActionEventArgs e)
        {
            IAction action = e.Action;
            Type actionType = action.GetType();
            var actionAttribute = actionType.GetCustomAttributes(true)[0] as ActionCategoryAttribute;
            ActionDisplayControl adc;
            if (actionAttribute != null && actionAttribute.DisplayControl != null)
            {
                Type customADCType = actionType.Assembly.GetType(string.Format(@"{0}.{1}", actionType.Namespace,
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
            action.MessageHandler += this.OnMessageHandlerDelegate;
            adc.Deleted += adc_Deleted;
            adc.DragEnter += ActionContainer_DragEnter;
            adc.DragDrop += ActionContainer_DragDrop;
            this.ActionContainer.Controls.Add(adc, 0, e.Index);
            this.ActionContainer.ScrollControlIntoView(adc);
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
            {
                return;
            }

            DialogResult result = this.OpenFileDialog.ShowDialog(this);

            if (result != DialogResult.OK)
            {
                return;
            }

            Core.LoadJob(this.OpenFileDialog.FileName);
            Text = Core.Job.Name;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if(!_stepping)
            {
                StartRunning(false);
            }
            else
            {
                DoStep();
            }
        }

        private void StartRunning(bool step)
        {
            this._stepping = step;
            ResetDisplay();
            this.backgroundWorker1.RunWorkerAsync(step);
        }

        private void ResetDisplay()
        {
            foreach (ActionDisplayControl adc in this.ActionContainer.Controls)
            {
                SetState(adc, ActionState.None, String.Empty);
            }
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = Core.Job.Complexity;
        }

        [STAThread]
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Core.RunJob(_stepping);
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
                CentipedeJob job = Core.Job;
                var jobPropertyForm = new JobPropertyForm(ref job);
                DialogResult result = jobPropertyForm.ShowDialog(this);
                Core.Job = job;
                if (result == DialogResult.Cancel)
                {
                    throw new AbortOperationException();
                }
                
            }
            this.saveFileDialog1.FileName = !String.IsNullOrEmpty(Core.Job.FileName)
                                                    ? Core.Job.FileName
                                                    : Core.Job.Name;

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
            try
            { 
                base.Dispose(disposing); 
            }
            catch (NullReferenceException)
            { }
            Core.Dispose();
        }

        /// <summary>
        /// Check if save needed, ask to save, and save
        /// </summary>
        /// <exception cref="AbortOperationException">Throws abort operation exception if cancel is clicked at any 
        /// point</exception>
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
            { }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditFavouritesMenuItem_Click(object sender, EventArgs e)
        {
            //var editFavourites = new EditFavourites(Settings.Default.FavouriteJobs);
            var editFavourites = new EditFavourites();

            editFavourites.ShowDialog(this);

            UpdateFavourites();
        }

        private void UpdateOutputWindow()
        {
            MessageDataGridView.Parent.SuspendLayout();
            MessageDataGridView.Hide();
            foreach (JobDataSet.MessagesRow row in _dataSet.Messages)
            {
                row.Visible = DisplayedLevels.HasFlag(row.Level);
            }
            MessageDataGridView.Show();
            MessageDataGridView.Parent.ResumeLayout(true);
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
            CentipedeJob centipedeJob = Core.Job;
            var form = new JobPropertyForm(ref centipedeJob);
            form.ShowDialog(this);
            if (!form.Dirty)
            {
                return;
            }
            Dirty = true;
            this.WebBrowser.Navigate(Core.Job.InfoUrl);
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Core.Dispose();
            Dispose();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
        }

        private void addCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Core.Job.Name))
            {
                return;
            }
            try
            {
                SaveJob();
                //this._favouriteJobsDataStore.Favourites.AddFavouritesRow(Core.Job.Name, Core.Job.FileName);
                Properties.Settings.Default.ListOfFavouriteJobs.Add(Core.Job.FileName);
                UpdateFavourites();
            }
            catch (AbortOperationException)
            { }
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
            Process.Start(Resources.MainWindow_visitGetSatisfactionToolStripMenuItem_Click_GetSatisfaction_Url);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutForm(this._pluginFiles)).Show(this);
        }

        private void stepThroughToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this._stepping)
            {
                RunButton.Text = Resources.MainWindow_stepThroughToolStripMenuItem_Click_Step;
                StartRunning(true);
            }
            else
            {
                DoStep();
            }
        }

        private void DoStep()
        {
            this._steppingMutex.Set();
        }

        private void abortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.AbortRun();
        }

        private void resetJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.AbortRun();
            
            ResetDisplay();
            this._dataSet.Messages.Clear();
            
        }

        private void pythonReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://docs.python.org/2.7/");
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

