using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Centipede
{
    internal partial class ActionDisplayControl : UserControl
    {
        internal ActionDisplayControl(Action action)
        {
            InitializeComponent();
            NameLabel.Text = action.Name;

            this.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.BackColor = SystemColors.Control;

            Action = action;
            Label attrLabel;
            TextBox attrValue;
            foreach (KeyValuePair<String, Object> attr in action.Attributes)
            {
                attrLabel = new Label();
                attrLabel.Text = attr.Key;
                attrLabel.Dock = DockStyle.Fill;
                
                AttributeTable.Controls.Add(attrLabel);

                attrValue = new TextBox();
                attrValue.Width = 250;
                if (attr.Key == "source")
                {
                    attrValue.Multiline = true;
                    attrValue.Height = 5000;
                    attrValue.Dock = DockStyle.Fill;
                    attrValue.ScrollBars = ScrollBars.Both;
                    attrValue.Font = new Font(FontFamily.GenericMonospace, 10);
                    attrValue.WordWrap = false;
                }
                attrValue.Text = attr.Value.ToString();
                attrValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                //attrValue.Dock = DockStyle.Fill;
                
                AttributeTable.Controls.Add(attrValue);
                attrValue.Tag = attr.Key;
                attrValue.TextChanged += new EventHandler(attrValue_TextChanged);
            }
        }

        void attrValue_TextChanged(object sender, EventArgs e)
        {
            TextBox attrValue = (TextBox)sender;
            Action.Attributes[(string)attrValue.Tag] = attrValue.Text;
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

        public readonly Action Action;

        private void ActMenuDelete_Click(object sender, EventArgs e)
        {
            
            ToolStripDropDownItem i = (ToolStripDropDownItem)sender;
            ContextMenuStrip cm = (ContextMenuStrip)i.Owner;
            ActionDisplayControl adc = (ActionDisplayControl)cm.SourceControl;
            Program.RemoveAction(adc.Action);
            ((TableLayoutPanel)adc.Parent).Controls.Remove(adc);
        }

        private void ActionDisplayControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.ToString();
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
