using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Centipede.Properties;
using CentipedeInterfaces;
using ResharperAnnotations;


namespace Centipede.Actions
{
    /// <summary>
    /// The control used to display an action.  Contains all logic required to determine what arguments to display
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public partial class ActionDisplayControl : UserControl
    {
        /// <summary>
        /// The status of the action, displayed as a tooltip on the status icon.
        /// </summary>
        public String StatusMessage
        {
            set
            {
                this.StatusTooltip.SetToolTip(StatusIconBox, value);
            }  
            get
            {
                return this.StatusToolTip.GetToolTip(StatusIconBox);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //protected ActionDisplayControl()
        //{
        //    Selected = false;
        //    InitializeComponent();
        //}

        /// <summary>
        /// 
        /// </summary>
        protected void SetProperties()
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            BackColor = SystemColors.Control;
            NameLabel.Text = ThisAction.Name;

            CommentTextBox.Text = ThisAction.Comment;

            ThisAction.Tag = this;
        }

        /// <summary>
        /// Create a new ActionDisplayControl object
        /// </summary>
        /// <param name="action">The <see cref="Centipede.Action"/> this contol is displaying</param>
        /// <param name="generateArgumentFields">If <c>true</c>, automatically generate argument fields from the members 
        ///     of <paramref name="action"/> tagged with <see cref="T:CentipedeInterfaces.ActionArgumentAttribute"/>
        ///     If passed as <c>false</c> from a subclass, the subclass must do this itself.
        /// </param>
        public ActionDisplayControl(IAction action, bool generateArgumentFields = true)
        {
            Selected = false;
            InitializeComponent();

            _thisAction = action;

            SetProperties();

            StatusToolTip = new ToolTip();
            StatusToolTip.SetToolTip(StatusIconBox, "");

            ActionIcon.Image = GetActionIcon(action);
            
            if (generateArgumentFields)
            {
                GenerateArguments();
            }
        }

        private static Image GetActionIcon(IAction action)
        {
            Type actionType = action.GetType();

            ActionCategoryAttribute categoryAttribute =
                    (ActionCategoryAttribute)
                    actionType.GetCustomAttributes(typeof (ActionCategoryAttribute), true).Single();

            Image image = null;

            if (!String.IsNullOrEmpty(categoryAttribute.iconName))
            {
                Type t = actionType.Assembly.GetType(actionType.Namespace + @".Properties.Resources");
                if (t != null)
                {
                    var icon = t.GetProperty(categoryAttribute.iconName, typeof (Icon)).GetValue(t, null) as Icon;
                    if (icon != null)
                    {
                        image = icon.ToBitmap();
                    }
                }
            }
            return image;
        }

        private void GenerateArguments()
        {
            Type actionType = ThisAction.GetType();

            var fieldArguments = from fi in actionType.GetMembers()
                                 where fi is FieldInfo || fi is PropertyInfo
                                 select (FieldAndPropertyWrapper)fi
                                 into wrapped
                                 where wrapped.GetArguementAttribute() != null
                                 select wrapped;

            foreach (FieldAndPropertyWrapper arg in fieldArguments)
            {
                AttributeTable.Controls.AddRange(GenerateFieldControls(arg));
            }
        }

        private Control[] GenerateFieldControls(FieldAndPropertyWrapper arg)
        {
            ActionArgumentAttribute attrData = arg.GetArguementAttribute();
            var attrLabel = new Label { Text = GetArgumentName(arg), Dock = DockStyle.Fill };

            if (!string.IsNullOrEmpty(attrData.usage))
            {
                ArgumentTooltips.SetToolTip(attrLabel, attrData.usage);
            }
            Control attrValue;
            {
                switch (arg.GetFieldTypeCategory())
                {
                case FieldAndPropertyWrapper.FieldType.Boolean:
                    CheckBox cb = new CheckBox
                                  {
                                          Anchor = AnchorStyles.Top | AnchorStyles.Left,
                                          Checked = arg.Get<Boolean>(ThisAction)
                                  };
                    cb.CheckedChanged += GetChangedHandler(arg) ??
                                         ((sender, e) => arg.Set(ThisAction, ((CheckBox)sender).Checked));
                    cb.CheckedChanged += SetDirty;
                    attrValue = cb;

                    break;
                    //case FieldAndPropertyWrapper.FieldType.Other:
                    //case FieldAndPropertyWrapper.FieldType.String:
                    //case FieldAndPropertyWrapper.FieldType.Numeric:
                default:
                    attrValue = new TextBox
                                {
                                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                                        BackColor = attrData.Literal
                                                            ? SystemColors.Info
                                                            : SystemColors.Window
                                };
                    object obj = arg.Get<Object>(ThisAction); //.ToString();
                    attrValue.Text = (obj ?? "").ToString();
                    attrValue.TextChanged += GetChangedHandler(arg);
                    attrValue.TextChanged += SetDirty;
                    break;
                }

                attrValue.Tag = arg;
                attrValue.Leave += GetLeaveHandler(arg);
            }

            attrLabel.Margin = new Padding
                               {
                                       Top = (attrValue.Height - attrLabel.Height) / 2,
                                       Bottom = attrLabel.Margin.Bottom,
                                       Left = attrLabel.Margin.Left,
                                       Right = attrLabel.Margin.Right
                               };
            
            return new[] { attrLabel, attrValue };
        }

        /*
        private Control GetDisplayControl(FieldAndPropertyWrapper fieldAndPropertyWrapper)
        {
            return null;
        }
*/

        private EventHandler GetChangedHandler(FieldAndPropertyWrapper arg)
        {
            ActionArgumentAttribute argAttr = arg.GetArguementAttribute();
            if (argAttr.onChangedHandlerName == null)
            {
                return null;
            }
            MethodInfo method = arg.DeclaringType.GetMethod(argAttr.onChangedHandlerName);
            return (sender, e) => method.Invoke(ThisAction, new[] { sender, e });
        }

        private EventHandler GetLeaveHandler(FieldAndPropertyWrapper arg)
        {
            ActionArgumentAttribute argAttr = arg.GetArguementAttribute();

            if (argAttr.onLeaveHandlerName == null)
            {
                if (argAttr.onChangedHandlerName == null)
                {
                    return attrValue_TextChanged;
                }
                return null;
            }
            Type actionType = ThisAction.GetType();
            MethodInfo method = actionType.GetMethod(argAttr.onLeaveHandlerName);
            return (sender, e) => method.Invoke(ThisAction, new[] { sender, e });
        }

        private static String GetArgumentName(FieldAndPropertyWrapper argument)
        {
            ActionArgumentAttribute argAttr = argument.GetArguementAttribute();
            return argAttr.displayName ?? argument.Name;
        }

        private void attrValue_TextChanged(object sender, EventArgs e)
        {
            TextBox attrValue = sender as TextBox;

            if (attrValue == null)
            {
                return;
            }
            FieldAndPropertyWrapper f = (FieldAndPropertyWrapper)attrValue.Tag;
            if (f == null)
            {
                return;
            }
            ActionArgumentAttribute argInfo = f.GetArguementAttribute();

            object value = f.Get<Object>(ThisAction);
            MethodInfo parser = f.MemberType.GetMethod(@"Parse", new[] { typeof (String) });
            if (parser != null)
            {
                try
                {
                    value = parser.Invoke(f, new object[] { attrValue.Text });
                }
                catch (Exception)
                {
                    MessageBox.Show(String.Format(Resources.ActionDisplayControl_attrValue_TextChanged_Invalid_Value_entered_message,
                                                  ThisAction.Name, argInfo.displayName, attrValue.Text));
                }
            }
            else
            {
                value = attrValue.Text;
            }


            f.Set(ThisAction, value);
        }

        ///
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>
        /// A System.Drawing.Color that represents the background color of the control.
        /// The default is the value of the System.Windows.Forms.Control.DefaultBackColor
        /// property.
        /// </value>
        private new Color BackColor
        {
            set
            {
                if (!Selected)
                {
                    base.BackColor = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Selected { get; private set; }

        private void ExpandButton_Click(object sender, EventArgs e)
        {
            if (!AttributeTable.Visible)
            {
                AttributeTable.Visible = true;
                ExpandButton.Text = @"-";
            }
            else
            {
                AttributeTable.Visible = false;
                ExpandButton.Text = @"+";
            }
        }

        private ActionState _state = ActionState.None;

        /// <summary>
        /// Sets the displayed state of the action.
        /// <seealso cref="T:Centipede.Actions.ActionState"/>
        /// </summary>
        public ActionState State
        {
            get
            {
                return _state;
            }
            set
            {
                switch (value)
                {
                case ActionState.None:
                    StatusIconBox.Image = null;
                    BackColor = SystemColors.Control;
                    break;
                case ActionState.Running:
                    StatusIconBox.Image = StatusIcons.Images[@"Run.png"];
                    BackColor = Color.DarkGray;
                    break;
                case ActionState.Error:
                    StatusIconBox.Image = StatusIcons.Images[@"Error.png"];
                    BackColor = Color.DarkRed;
                    break;
                case ActionState.Completed:
                    StatusIconBox.Image = StatusIcons.Images[@"OK.png"];
                    BackColor = SystemColors.Control;
                    break;
                }
                _state = value;
            }
        }

        /// <summary>
        /// The <see cref="T:Centipede.Action"/> this control is displaying.
        /// </summary>
        public virtual IAction ThisAction
        {
            get
            {
                return this._thisAction;
            }
            protected set
            {
                this._thisAction = value;
            }
        }

        protected ToolTip StatusToolTip;
        private IAction _thisAction;

        /// <summary>
        /// 
        /// </summary>
        public static EventHandler SetDirty;

        /// <summary>
        /// 
        /// </summary>
        public event DeletedEventHandler Deleted;

        private void CommentTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                ThisAction.Comment = textBox.Text;
                
            }
            SetDirty(sender, e);
        }

        //private void ActionDisplayControl_DragEnter(object sender, DragEventArgs e)
        //{
        //    e.Effect = DragDropEffects.Move;
        //}

        //private void ActionDisplayControl_DragDrop(object sender, DragEventArgs e)
        //{
        //    var data = e.Data.GetData("WindowsForms10PersistentObject");
        //    int index = Program.GetIndexOf(ThisAction);
        //    Program.AddAction((data as ActionFactory).Generate(), index);
        //}

        private void ActMenuDelete_Click(object sender, EventArgs e)
        {
            var handler = Deleted;
            if (handler != null)
            {
                handler(this, CentipedeEventArgs.Empty);
            }

        }

        /// <summary>
        /// Refresh the values displayed in Argument controls
        /// </summary>
        public virtual void UpdateControls()
        {
            foreach (Control control in AttributeTable.Controls)
            {
                if ((control is TextBox))
                {
                    control.Text = ((FieldAndPropertyWrapper)control.Tag).Get<Object>(ThisAction).ToString();
                }
                if (control is CheckBox)
                {
                    (control as CheckBox).Checked = ((FieldAndPropertyWrapper)control.Tag).Get<bool>(ThisAction);
                }
            }
        }

        private void ActionDisplayControl_Load(object sender, EventArgs e)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DeletedEventHandler(object sender, CentipedeEventArgs e);
    }

    /// <summary>
    /// The state of the <see cref="T:Centipede.Action"/> stored in <see cref="P:Centipede.Actions.ActionDisplayControl.ThisAction"/>
    /// </summary>
    public enum ActionState
    {
        /// <summary>
        /// Action has not been executed yet 
        /// </summary>
        None = 0,

        /// <summary>
        /// Action is currently being executed
        /// </summary>
        Running = 1,

        /// <summary>
        /// Action has been completed
        /// </summary>
        Completed = 2,

        /// <summary>
        /// An error occurred
        /// </summary>
        Error = -1
    }
}