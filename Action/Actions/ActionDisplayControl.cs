using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Centipede.Properties;


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
            set
            {
                StatusTooltip.SetToolTip(StatusIconBox, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ActionDisplayControl()
        {
            Selected = false;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            InitializeComponent();
        }

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
        /// 
        /// </summary>
        /// <param name="action"></param>
        public ActionDisplayControl(Action action)
        {
            Selected = false;
            InitializeComponent();

            ThisAction = action;

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            SetProperties();

            _statusToolTip = new ToolTip();
            _statusToolTip.SetToolTip(StatusIconBox, "");

            GenerateArguments();

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
            if(!string.IsNullOrEmpty(attrData.usage))
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
                    cb.CheckedChanged += GetChangedHandler(arg) ?? ((sender, e) => arg.Set(ThisAction, (sender as CheckBox).Checked));

                    attrValue = cb;

                    break;
                    //case FieldAndPropertyWrapper.FieldType.Other:
                    //case FieldAndPropertyWrapper.FieldType.String:
                    //case FieldAndPropertyWrapper.FieldType.Numeric:
                default:
                    attrValue = new TextBox
                                {
                                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                                };
                    object obj = arg.Get<Object>(ThisAction);//.ToString();
                    attrValue.Text = (obj ?? "").ToString();
                    attrValue.TextChanged += GetChangedHandler(arg);
                    break;
                }

                attrValue.Tag = arg;
                attrValue.Leave += GetLeaveHandler(arg);
            }

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



        String GetArgumentName(FieldAndPropertyWrapper argument)
        {
            ActionArgumentAttribute argAttr = argument.GetArguementAttribute();
            return argAttr.displayName ?? argument.Name;
        }

        void attrValue_TextChanged(object sender, EventArgs e)
        {
            var attrValue = sender as TextBox;
            var f = attrValue.Tag as FieldAndPropertyWrapper;
            ActionArgumentAttribute argInfo = f.GetArguementAttribute();

            var value = f.Get<Object>(ThisAction);
            MethodInfo parser = f.MemberType.GetMethod("Parse", new[] { typeof(String) });
            if (parser != null)
            {
                try
                {
                    value = parser.Invoke(f, new object[] { attrValue.Text });
                }
                catch (Exception)
                {
                    MessageBox.Show(String.Format(Resources.ActionDisplayControl_attrValue_TextChanged_Invalid_Value_entered_message, ThisAction.Name, argInfo.displayName, attrValue.Text));
                }
            }
            else
            {
                value = attrValue.Text;
            }

            f.Set(ThisAction, value);

        }

// ReSharper disable ConvertToConstant.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper restore FieldCanBeMadeReadOnly.Local
// ReSharper restore ConvertToConstant.Local

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
// ReSharper disable MemberCanBePrivate.Global
        public bool Selected
// ReSharper restore MemberCanBePrivate.Global
        {
            get;
            private set;
        }

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
            // ReSharper disable UnusedMember.Global
            get
            {
                return _state;
            }
            // ReSharper restore UnusedMember.Global
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

        // ReSharper disable VirtualMemberNeverOverriden.Global
        /// <summary>
        /// The <see cref="T:Centipede.Action"/> this control is displaying.
        /// </summary>
        public virtual Action ThisAction
        {
            get;
            protected set;
        }
        // ReSharper restore VirtualMemberNeverOverriden.Global

        private readonly ToolTip _statusToolTip;

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
            var handler = Deleted;
            if (handler != null)
            {
                handler(this, null);
            }

        }
    }

    /// <summary>
    /// The state of the <see cref="T:Centipede.Action"/> stored in <see cref="P:Centipede.Actions.ActionDisplayControl.ThisAction"/>
    /// </summary>
    public enum ActionState
    {
        /// <summary>
        /// Action has not been executed yet 
        /// </summary>
        None = -1,

        /// <summary>
        /// Action is currently being executed
        /// </summary>
        Running = 0,

        /// <summary>
        /// Actiion has been completed
        /// </summary>
        Completed = 1,

        /// <summary>
        /// 
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// Wrapper to allow uniform access to <see cref="T:System.Reflection.FieldInfo"/> and <see cref="T:System.Reflection.PropertyInfo"/>
    /// </summary>
    public class FieldAndPropertyWrapper : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="FieldAndPropertyWrapper"/>
        /// </summary>
        /// <param name="prop">The <see cref="PropertyInfo"/> to wrap</param>
        /// <seealso cref="FieldAndPropertyWrapper(FieldInfo)"/>
        public FieldAndPropertyWrapper(PropertyInfo prop)
        {
            _member = prop;
            _getter = o => prop.GetValue(o, null);
            _setter = (o, v) => prop.SetValue(o, v, null);
            _memberType  = prop.PropertyType;

            Cache[prop] = this;
        }

        /// <summary>
        /// Creates a new <see cref="FieldAndPropertyWrapper"/>
        /// </summary>
        /// <param name="field">The <see cref="FieldInfo"/> to wrap</param>
        /// <seealso cref="FieldAndPropertyWrapper(PropertyInfo)"/>
        public FieldAndPropertyWrapper(FieldInfo field)
        {
            _member = field;
            _getter = field.GetValue;
            _setter = field.SetValue;
            _memberType  = field.FieldType;

            Cache[field] = this;
        }


        /// <value>
        /// The <see cref="T:System.Type"/> that declares the member the <see cref="T:Centipede.Actions.FieldAndPropertyWrapper"/> is referencing
        /// </value>
        public Type DeclaringType
        {
            get
            {
                return _member.DeclaringType;
            }
        }


        private readonly MemberInfo _member;
        private readonly Func<object, object> _getter;
        
        /// <summary>
        /// Gets the value of the wrapped member
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The instance to get the value of the member of</param>
        /// <returns></returns>
        public T Get<T>(object o)
        {
            return (T)_getter(o);
        }
        private readonly Action<object, object> _setter;
        private readonly Type _memberType;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="value"></param>
        public void Set<T>(object o, T value)
        {
            _setter(o, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionArgumentAttribute GetArguementAttribute()
        {
            return _member.GetCustomAttributes(typeof(ActionArgumentAttribute), true).Cast<ActionArgumentAttribute>().SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return _member.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type MemberType
        {
            get
            {
                return _memberType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public enum FieldType
        {
            //first value listed is used for default()
            /// <summary>
            /// 
            /// </summary>
            Other, 
            
            /// <summary>
            /// 
            /// </summary>
            String, 

            /// <summary>
            /// 
            /// </summary>
            Numeric, 
            
            /// <summary>
            /// 
            /// </summary>
            Boolean,
        }
        private static readonly Dictionary<Type, FieldType> TypeMapping = new Dictionary<Type, FieldType>
                                                                          {
            {typeof(bool),      FieldType.Boolean},
            {typeof(byte),      FieldType.Numeric},
            {typeof(sbyte),     FieldType.Numeric},
            {typeof(char),      FieldType.String},
            {typeof(decimal),   FieldType.Numeric},
            {typeof(double),    FieldType.Numeric},
            {typeof(float),     FieldType.Numeric},
            {typeof(int),       FieldType.Numeric},
            {typeof(uint),      FieldType.Numeric},
            {typeof(long),      FieldType.Numeric},
            {typeof(ulong),     FieldType.Numeric},
            {typeof(short),     FieldType.Numeric},
            {typeof(ushort),    FieldType.Numeric},
            {typeof(string),    FieldType.String},
        };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FieldType GetFieldTypeCategory()
        {

            Type baseType = MemberType;

            while (!TypeMapping.ContainsKey(baseType) && baseType != typeof(object))
            {
                baseType = baseType.BaseType ?? typeof (Object);
            }

            FieldType fieldType;
            return TypeMapping.TryGetValue(baseType, out fieldType) ? fieldType : FieldType.Other;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static implicit operator FieldAndPropertyWrapper(FieldInfo f)
        {
            if (Cache.ContainsKey(f))
            {
                return Cache[f];
            }
            return new FieldAndPropertyWrapper(f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapped"></param>
        /// <returns></returns>
        public static explicit operator FieldInfo(FieldAndPropertyWrapper wrapped)
        {
            if (wrapped._member is FieldInfo)
            {
                return wrapped._member as FieldInfo;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator FieldAndPropertyWrapper(PropertyInfo p)
        {
            if (Cache.ContainsKey(p))
            {
                return Cache[p];
            }
            return new FieldAndPropertyWrapper(p);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapped"></param>
        /// <returns></returns>
        public static explicit operator PropertyInfo(FieldAndPropertyWrapper wrapped)
        {
            if (wrapped._member is PropertyInfo)
            {
                return wrapped._member as PropertyInfo;
            }
            throw new InvalidCastException();
        }

        private static readonly Dictionary<MemberInfo, FieldAndPropertyWrapper> Cache = new Dictionary<MemberInfo, FieldAndPropertyWrapper>(); 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static explicit operator FieldAndPropertyWrapper(MemberInfo m)
        {
            if (Cache.ContainsKey(m))
            {
                return Cache[m];
            }
            if (m is FieldInfo)
            {
                return new FieldAndPropertyWrapper(m as FieldInfo);
            }
            if (m is PropertyInfo)
            {
                return new FieldAndPropertyWrapper(m as PropertyInfo);
            }
            throw new InvalidCastException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapped"></param>
        /// <returns></returns>
        public static implicit operator MemberInfo(FieldAndPropertyWrapper wrapped)
        {
            return wrapped._member;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            Cache.Clear();
        }
    }
}
