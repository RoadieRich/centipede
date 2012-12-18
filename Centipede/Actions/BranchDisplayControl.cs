using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Centipede.Actions;

namespace Centipede
{
    class BranchDisplayControl : ActionDisplayControl
    {
        BranchDisplayControl(Action action) : base(action)
        { }



        #region Designer required code
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BranchDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "BranchDisplayControl";
            this.Size = new System.Drawing.Size(191, 117);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}
