using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Centipede.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ActionDisplayControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public String StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                StatusTooltip.SetToolTip(StatusIconBox, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public ActionDisplayControl(Action action)
        {
            InitializeComponent();
            NameLabel.Text = action.Name;

            this.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.BackColor = SystemColors.Control;

            ThisAction = action;
            Assembly actionAssembly = Assembly.GetAssembly(action.GetType());
            Label attrLabel;
            TextBox attrValue;
                        
            _statusToolTip = new ToolTip();
            _statusToolTip.SetToolTip(StatusIconBox, "");

            Type actionType = action.GetType();

            List<FieldInfo> arguments = new List<FieldInfo>(from FieldInfo fi in actionType.GetFields()
                               where (fi.GetCustomAttributes(typeof(ActionArgumentAttribute), true).Count() > 0)
                               select fi);
            
            foreach (FieldInfo arg in arguments)
            {
                ActionArgumentAttribute attrData = arg.GetCustomAttributes(true)[0] as ActionArgumentAttribute;
                attrLabel = new Label();
                attrLabel.Text = GetArgumentName(arg);
                attrLabel.Dock = DockStyle.Fill;
                ArgumentTooltips.SetToolTip(attrLabel, attrData.usage);

                AttributeTable.Controls.Add(attrLabel);

                attrValue = new TextBox();
                attrValue.Width = 250;

                attrValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                if (arg.Name == "Source")
                {
                    attrValue.Multiline = true;
                    attrValue.Height = 175;
                    //attrValue.Dock = DockStyle.Fill;
                    attrValue.ScrollBars = ScrollBars.Both;
                    attrValue.Font = new Font(FontFamily.GenericMonospace, 10);
                    attrValue.WordWrap = false;
                }
                attrValue.Text = arg.GetValue(action).ToString();
                //attrValue.Dock = DockStyle.Top;

                AttributeTable.Controls.Add(attrValue);
                attrValue.Tag = arg;
                //attrValue.TextChanged += ;
                attrValue.Leave += new EventHandler(attrValue_TextChanged);
            }

            CommentTextBox.Text = action.Comment;

            action.Tag = this;
        }

        private String GetArgumentName(MemberInfo argument)
        {
            ActionArgumentAttribute argAttr = argument.GetCustomAttributes(typeof(ActionArgumentAttribute),true).Single() as ActionArgumentAttribute;
            return argAttr.displayName ?? argument.Name;
        }

        void attrValue_TextChanged(object sender, EventArgs e)
        {
            TextBox attrValue = sender as TextBox;
            String oldVal = attrValue.Text;  
            FieldInfo f = attrValue.Tag as FieldInfo;
            ActionArgumentAttribute argInfo = f.GetCustomAttributes(typeof(ActionArgumentAttribute), true).Single() as ActionArgumentAttribute;

            
            if (argInfo.setterMethodName != null)
            {
                Type type = this.ThisAction.GetType();
                MethodInfo setterMethod = type.GetMethod(argInfo.setterMethodName);
                    
                if (!(Boolean)setterMethod.Invoke(this.ThisAction, new object[] { attrValue.Text }))
                {
                    MessageBox.Show(String.Format("Invalid Value entered in {0}.{1}: {2}", this.ThisAction.Name, argInfo.displayName, attrValue.Text));
                }
            }
            else
            {
                Object value = f.GetValue(this.ThisAction);
                MethodInfo parser = f.FieldType.GetMethod("Parse");
                if (parser != null)
                {
                    try
                    {
                        value = parser.Invoke(f, new object[] { attrValue.Text });
                    }
                    catch (Exception)
                    {
                        String.Format("Invalid Value entered in {0}.{1}: {2}", this.ThisAction.Name, argInfo.displayName, attrValue.Text);
                    }

                }
                else
                {
                    value = attrValue.Text;
                }
                f.SetValue(this.ThisAction, value);
            }
        }
    
        private Boolean _selected = false;
        private Color _unselectedColour = SystemColors.Control;

        ///
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>
        /// A System.Drawing.Color that represents the background color of the control.
        /// The default is the value of the System.Windows.Forms.Control.DefaultBackColor
        /// property.
        /// </value>
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (!Selected)
                {
                    base.BackColor = value;
                }
                else
                {
                    _unselectedColour = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (value)
                {
                    _unselectedColour = BackColor;
                    base.BackColor = SystemColors.Highlight;
                }
                else
                {
                    base.BackColor = _unselectedColour;
                }
                _selected = value;
            }
        }

        private void ExpandButton_Click(object sender, EventArgs e)
        {
            if (!AttributeTable.Visible)
            {
                AttributeTable.Visible = true;
                ExpandButton.Text = "-";
            }
            else
            {
                AttributeTable.Visible = false;
                ExpandButton.Text = "+";
            }
        }

        private ActionState _state = ActionState.None;

        /// <summary>
        /// Sets the displayed state of the action.
        /// <see cref="ActionState"/>
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
                        StatusIconBox.Image = StatusIcons.Images["Run.png"];
                        BackColor = Color.DarkGray;
                        break;
                    case ActionState.Error:
                        StatusIconBox.Image = StatusIcons.Images["Error.png"];
                        BackColor = Color.DarkRed;
                        break;
                    case ActionState.Completed:
                        StatusIconBox.Image = StatusIcons.Images["OK.png"];
                        BackColor = SystemColors.Control;
                        break;
                }
                _state = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public readonly Action ThisAction;
        private ToolTip _statusToolTip;
        private string _statusMessage;

        /// <summary>
        /// 
        /// </summary>
        public event DeletedEventHandler Deleted;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DeletedEventHandler(object sender, CentipedeEventArgs e);

        
        private void CommentTextBox_TextChanged(object sender, EventArgs e)
        {
            ThisAction.Comment = (sender as TextBox).Text;
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

            ToolStripDropDownItem i = sender as ToolStripDropDownItem;
            ContextMenuStrip cm = i.Owner as ContextMenuStrip;

            this.Deleted.Invoke(this, null);

        }
      
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ActionState
    {
        /// <summary>
        /// 
        /// </summary>
        None = -1,

        /// <summary>
        /// 
        /// </summary>
        Running = 0,

        /// <summary>
        /// 
        /// </summary>
        Completed = 1,

        /// <summary>
        /// 
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class CentipedeEventArgs : EventArgs
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="program"></param>
        /// <param name="actions"></param>
        /// <param name="variables"></param>
        public CentipedeEventArgs(Type program, List<Action> actions, Dictionary<String, Object> variables) : base()
        {
            Program = program;
            Actions = actions;
            Variables = variables;
        }

        /// <summary>
        /// 
        /// </summary>
        public readonly Type Program;

        /// <summary>
        /// 
        /// </summary>
        public readonly List<Action> Actions;

        /// <summary>
        /// 
        /// </summary>
        public readonly Dictionary<string, object> Variables;
    }
}
