using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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
                                 select new FieldAndPropertyWrapper((dynamic)fi)
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
                    MessageBox.Show(String.Format("Invalid Value entered in {0}.{1}: {2}", ThisAction.Name, argInfo.displayName, attrValue.Text));
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
// ReSharper disable UnusedMember.Global
            get
// ReSharper restore UnusedMember.Global
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
// ReSharper disable VirtualMemberNeverOverriden.Global
        public virtual Action ThisAction
// ReSharper restore VirtualMemberNeverOverriden.Global
        {
            get;
            protected set;
        }

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

    class FieldAndPropertyWrapper
    {
        public Type DeclaringType
        {
            get
            {
                return _member.DeclaringType;
            }
        }
        public FieldAndPropertyWrapper(PropertyInfo prop)
        {
            _member = prop;
            _getter = o => prop.GetValue(o, null);
            _setter = (o, v) => prop.SetValue(o, v, null);
            _memberType  = prop.PropertyType;
        }

        public FieldAndPropertyWrapper(FieldInfo field)
        {
            _member = field;
            _getter = field.GetValue;
            _setter = field.SetValue;
            _memberType  = field.FieldType;
        }

        readonly MemberInfo _member;

        private readonly Func<object, object> _getter;
        public T Get<T>(object o)
        {
            return (T)_getter(o);
        }
        private readonly Action<object, object> _setter;
        private readonly Type _memberType;

        public void Set<T>(object o, T v)
        {
            _setter(o, v);
        }


        public ActionArgumentAttribute GetArguementAttribute()
        {
            return _member.GetCustomAttributes(typeof(ActionArgumentAttribute), true).Cast<ActionArgumentAttribute>().SingleOrDefault();
        }


        public string Name
        {
            get
            {
                return _member.Name;
            }
        }

        public Type MemberType
        {
            get
            {
                return _memberType;
            }
        }

        public enum FieldType
        {
            //first value listed is used for default()
            Other, String, Numeric, Boolean,
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

        public FieldType GetFieldTypeCategory()
        {

            Type baseType = MemberType;

            while (!TypeMapping.ContainsKey(baseType) && baseType != typeof(object))
            {
                baseType = baseType.BaseType ?? typeof (Object);
            }

            FieldType fieldType;
            if (!TypeMapping.TryGetValue(baseType, out fieldType))
            {
                return FieldType.Other;
            }

            return fieldType;
        }
        public static implicit operator FieldAndPropertyWrapper(FieldInfo f)
        {
            return new FieldAndPropertyWrapper(f);
        }
        public static implicit operator FieldAndPropertyWrapper(PropertyInfo p)
        {
            return new FieldAndPropertyWrapper(p);
        }
        public static explicit operator FieldAndPropertyWrapper(MemberInfo m)
        {
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
    }
}
