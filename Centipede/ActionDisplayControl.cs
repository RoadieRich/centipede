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
    public partial class ActionDisplayControl : UserControl
    {
        public ActionDisplayControl(Action action)
        {
            InitializeComponent();
            NameLabel.Text = action.Name;
            //this.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.Dock = DockStyle.Right;
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
                if (attr.Key == "source")
                {
                    attrValue.Multiline = true;
                    attrValue.Font = new Font(FontFamily.GenericMonospace, 12);
                }
                attrValue.Text = attr.Value.ToString();
                attrValue.Dock = DockStyle.Fill;
                AttributeTable.Controls.Add(attrValue);
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

        private Boolean _selected = false;
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
                    this.BackColor = SystemColors.MenuHighlight;
                    _selected = true;
                }
                else
                {
                    this.BackColor = SystemColors.Control;
                    _selected = false;
                }
            }
        }

        public readonly Action Action;
    }
}
