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
    public partial class JobActionListBox : ListBox
    {
        public JobActionListBox()
        {
            InitializeComponent();
        }

        public void Add(Action action, Int32 at = -1)
        {
            TreeView actionTree = new TreeView();
            
            List<TreeNode> attrControls = new List<TreeNode>();
            TreeView tv = new TreeView();
            actionTree.Nodes.Add(action.Name);

            foreach (var attr in action.Attributes)
            {
                
                //TableLayoutPanel tlp = new TableLayoutPanel();
                //tlp.ColumnCount = 3;
                //Label lbl = new Label();
                //lbl.Text = attr.Key;
                
                //tlp.Controls.Add(lbl);
                //tlp.Controls.Add(new TextBox());

                actionTree.TopNode.Nodes.Add(attr.Key, attr.Value.ToString());

            }
            Items.Add(actionTree);
        }
    }
}
