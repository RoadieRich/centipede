using System;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    /// <summary>
    /// 
    /// </summary>
    public class AskEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        public String Message;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public String Title;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public AskEventEnums.AskType Type;
        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        public AskEventEnums.DialogResult Result = AskEventEnums.DialogResult.None;
        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
        public AskEventEnums.MessageIcon Icon = AskEventEnums.MessageIcon.Information;
    }

    /// <summary>
    /// 
    /// </summary>
    public static class AskEventEnums
    {
        /// <summary>
        /// 
        /// </summary>
        public enum AskType
        {
            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            AbortRetryIgnore = System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore,
            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            OK = System.Windows.Forms.MessageBoxButtons.OK,
            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            OKCancel = System.Windows.Forms.MessageBoxButtons.OKCancel,
            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            RetryCancel = System.Windows.Forms.MessageBoxButtons.RetryCancel,
            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            YesNo = System.Windows.Forms.MessageBoxButtons.YesNo,
            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            YesNoCancel = System.Windows.Forms.MessageBoxButtons.YesNoCancel
        }

        /// <summary>
        /// 
        /// </summary>
        public enum DialogResult
        {
            /// <summary>
            /// 
            /// </summary>
            None = System.Windows.Forms.DialogResult.None,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Abort = System.Windows.Forms.DialogResult.Abort,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Cancel = System.Windows.Forms.DialogResult.Cancel,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Ignore = System.Windows.Forms.DialogResult.Ignore,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            No = System.Windows.Forms.DialogResult.No,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            OK = System.Windows.Forms.DialogResult.OK,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Retry = System.Windows.Forms.DialogResult.Retry,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Yes = System.Windows.Forms.DialogResult.Yes
        }

        /// <summary>
        /// 
        /// </summary>
        public enum MessageIcon
        {
            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Asterisk = System.Windows.Forms.MessageBoxIcon.Asterisk,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Error = System.Windows.Forms.MessageBoxIcon.Error,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Exclamation = System.Windows.Forms.MessageBoxIcon.Exclamation,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Hand = System.Windows.Forms.MessageBoxIcon.Hand,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Information = System.Windows.Forms.MessageBoxIcon.Information,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            None = System.Windows.Forms.MessageBoxIcon.None,

            /// <summary>
            /// 
            /// </summary>
            Question = System.Windows.Forms.MessageBoxIcon.Question,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Stop = System.Windows.Forms.MessageBoxIcon.Stop,

            /// <summary>
            /// 
            /// </summary>
            [UsedImplicitly]
            Warning = System.Windows.Forms.MessageBoxIcon.Warning
        }
    }
}