using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Centipede
{
    internal partial class ActionDisplayControl : UserControl
    {
        List<ToolTip> StatusToolTips = new List<ToolTip>();

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
        
        internal ActionDisplayControl(Action action)
        {
            InitializeComponent();
            NameLabel.Text = action.Name;

            this.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.BackColor = SystemColors.Control;

            ThisAction = action;
            Assembly actionAssembly = Assembly.GetAssembly(action.GetType());
            Label attrLabel;
            TextBox attrValue;

            foreach (ActionState state in Enum.GetValues(typeof(ActionState)))
            {
                StatusToolTips.Add(new ToolTip());
            }

            _statusToolTip = new ToolTip();
            _statusToolTip.SetToolTip(StatusIconBox, "");

            


            Type actionType = action.GetType();

            List<FieldInfo> arguments = new List<FieldInfo>(from FieldInfo fi in actionType.GetFields()
                               where (fi.GetCustomAttributes(typeof(ActionArgumentAttribute), true).Count() > 0)
                               select fi);
            
            foreach (FieldInfo arg in arguments)
            {
                attrLabel = new Label();
                attrLabel.Text = GetArgumentName(arg);
                attrLabel.Dock = DockStyle.Fill;

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
                attrValue.TextChanged += new EventHandler(attrValue_TextChanged);
            }

            CommentTextBox.Text = action.Comment;

            action.Tag = this;
        }

        private String GetArgumentName(MemberInfo argument)
        {
            ActionArgumentAttribute argAttr = argument.GetCustomAttributes(typeof(ActionArgumentAttribute),true).Single() as ActionArgumentAttribute;
            if (argAttr.displayName != null)
            {
                return argAttr.displayName;
            }
            else
            {
                return argument.Name;
            }
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
                f.SetValue(this.ThisAction, attrValue.Text);
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

        public readonly Action ThisAction;
        private ToolTip _statusToolTip;
        private string _statusMessage;

        private void ActMenuDelete_Click(object sender, EventArgs e)
        {

            ToolStripDropDownItem i = sender as ToolStripDropDownItem;
            ContextMenuStrip cm = i.Owner as ContextMenuStrip;
           
            Program.RemoveAction(ThisAction);
            (this.Parent as TableLayoutPanel).Controls.Remove(this);
            
        }

        private void ActionDisplayControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.ToString();
        }

        private void CommentTextBox_TextChanged(object sender, EventArgs e)
        {
            ThisAction.Comment = (sender as TextBox).Text;
        }

        private void ActionDisplayControl_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ActionDisplayControl_DragDrop(object sender, DragEventArgs e)
        {
            
            var data = e.Data.GetData("WindowsForms10PersistentObject");
            int index = Program.GetIndexOf(ThisAction);
            Program.AddAction((data as ActionFactory).Generate(), index);
        }
    }

    internal enum ActionState
    {
        None = -1,
        Running = 0,
        Completed = 1,
        Error = 2
    }
}
