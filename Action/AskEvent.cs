using System;


namespace Centipede
{
    /// <summary>
    /// 
    /// </summary>
    public class AskActionEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable NotAccessedField.Global
        public String Message;
        /// <summary>
        /// 
        /// </summary>
        public String Title;
        /// <summary>
        /// 
        /// </summary>
        public AskEventEnums.AskType Type;
        // ReSharper disable ConvertToConstant.Global
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        public AskEventEnums.DialogResult Result = AskEventEnums.DialogResult.None;
        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
        /// <summary>
        /// 
        /// </summary>
        public AskEventEnums.MessageIcon Icon = AskEventEnums.MessageIcon.Information;
        // ReSharper restore NotAccessedField.Global
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
            AbortRetryIgnore = System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore,
            /// <summary>
            /// 
            /// </summary>
            OK = System.Windows.Forms.MessageBoxButtons.OK,
            /// <summary>
            /// 
            /// </summary>
            OKCancel = System.Windows.Forms.MessageBoxButtons.OKCancel,
            /// <summary>
            /// 
            /// </summary>
            RetryCancel = System.Windows.Forms.MessageBoxButtons.RetryCancel,
            /// <summary>
            /// 
            /// </summary>
            YesNo = System.Windows.Forms.MessageBoxButtons.YesNo,
            /// <summary>
            /// 
            /// </summary>
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
            Abort = System.Windows.Forms.DialogResult.Abort,
            /// <summary>
            /// 
            /// </summary>
            Cancel = System.Windows.Forms.DialogResult.Cancel,
            /// <summary>
            /// 
            /// </summary>
            Ignore = System.Windows.Forms.DialogResult.Ignore,
            /// <summary>
            /// 
            /// </summary>
            No = System.Windows.Forms.DialogResult.No,
            /// <summary>
            /// 
            /// </summary>
            None = System.Windows.Forms.DialogResult.None,
            /// <summary>
            /// 
            /// </summary>
            OK = System.Windows.Forms.DialogResult.OK,
            /// <summary>
            /// 
            /// </summary>
            Retry = System.Windows.Forms.DialogResult.Retry,
            /// <summary>
            /// 
            /// </summary>
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
            Asterisk = System.Windows.Forms.MessageBoxIcon.Asterisk,
            /// <summary>
            /// 
            /// </summary>
            Error = System.Windows.Forms.MessageBoxIcon.Error,
            /// <summary>
            /// 
            /// </summary>
            Exclamation = System.Windows.Forms.MessageBoxIcon.Exclamation,
            /// <summary>
            /// 
            /// </summary>
            Hand = System.Windows.Forms.MessageBoxIcon.Hand,
            /// <summary>
            /// 
            /// </summary>
            Information = System.Windows.Forms.MessageBoxIcon.Information,
            /// <summary>
            /// 
            /// </summary>
            None = System.Windows.Forms.MessageBoxIcon.None,
            /// <summary>
            /// 
            /// </summary>
            Question = System.Windows.Forms.MessageBoxIcon.Question,
            /// <summary>
            /// 
            /// </summary>
            Stop = System.Windows.Forms.MessageBoxIcon.Stop,
            /// <summary>
            /// 
            /// </summary>
            Warning = System.Windows.Forms.MessageBoxIcon.Warning
        }
    }
}