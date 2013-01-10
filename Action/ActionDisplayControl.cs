using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Linq.Expressions;

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
        protected ActionDisplayControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        protected void SetProperties()
        {
            this.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.BackColor = SystemColors.Control;
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
            InitializeComponent();
            thisAction = action;
            SetProperties();

            _statusToolTip = new ToolTip();
            _statusToolTip.SetToolTip(StatusIconBox, "");

            GenerateArguments();

        }

        private void GenerateArguments()
        {
            Type actionType = ThisAction.GetType();

            var fieldArguments = from fi in actionType.GetMembers().Where(fi => (fi is FieldInfo || fi is PropertyInfo)).Select(fi => new FieldAndPropertyWrapper(fi))
                                 where fi.GetArguementAttribute() != null
                                 select fi;



            foreach (FieldAndPropertyWrapper arg in fieldArguments)
            {
                AttributeTable.Controls.AddRange(GenerateFieldControls(arg));
            }
        }

        private Control[] GenerateFieldControls(FieldAndPropertyWrapper arg)
        {
            ActionArgumentAttribute attrData = arg.GetArguementAttribute();
            Label attrLabel = new Label();
            attrLabel.Text = GetArgumentName(arg);
            attrLabel.Dock = DockStyle.Fill;
            ArgumentTooltips.SetToolTip(attrLabel, attrData.usage);

            Control attrValue;
            switch (arg.GetFieldTypeCategory())
            {
                case FieldAndPropertyWrapper.FieldType.Boolean:
                    attrValue = new CheckBox();
                    attrValue.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    (attrValue as CheckBox).Checked = arg.Get<Boolean>(ThisAction);
                    break;
                case FieldAndPropertyWrapper.FieldType.Other:
                case FieldAndPropertyWrapper.FieldType.String:
                case FieldAndPropertyWrapper.FieldType.Numeric:
                default:
                    attrValue = new TextBox();
                    attrValue.Width = 250;
                    attrValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    attrValue.Text = arg.Get<Object>(thisAction).ToString();
                    break;

            }

            //attrValue.Dock = DockStyle.Top;

            attrValue.Tag = arg;
            attrValue.TextChanged += GetTextChangedHandler(arg);
            attrValue.Leave += GetLeaveHandler(arg);
            return new Control[2] { attrLabel, attrValue };
        }

        private T GetValue<T>(MemberInfo arg)
        {
            if (arg is FieldInfo)
            {
                return (T)(arg as FieldInfo).GetValue(ThisAction);
            }
            else
            {
                return (T)(arg as PropertyInfo).GetValue(ThisAction, null);
            }
        }

        private void GenerateArgumentsFromProperties()
        {
            //Type actionType = ThisAction.GetType();

            //var propertyArguments = from PropertyInfo pi in actionType.GetProperties()
            //                        where (GetArgumentAttribute(pi) != null)
            //                        select new FieldAndPropertyWrapper(pi);

            //foreach (FieldAndPropertyWrapper arg in propertyArguments)
            //{
            //    AttributeTable.Controls.AddRange(GenerateFieldControls(arg));
            //}
        }

        private EventHandler GetTextChangedHandler(FieldAndPropertyWrapper arg)
        {
            ActionArgumentAttribute argAttr = arg.GetArguementAttribute();
            Type actionType = ThisAction.GetType();
            if (argAttr.onTextChangedHandlerName == null)
            {
                return delegate { return; };
            }
            else
            {
                MethodInfo method = arg.DeclaringType.GetMethod(argAttr.onTextChangedHandlerName);
                return new EventHandler(
                    (sender, e) => method.Invoke(ThisAction, new Object[] { sender, e })
                );
            }
        }

        private EventHandler GetLeaveHandler(FieldAndPropertyWrapper arg)
        {
            ActionArgumentAttribute argAttr = arg.GetArguementAttribute();

            if (argAttr.onLeaveHandlerName == null)
            {
                if (argAttr.onTextChangedHandlerName == null)
                {
                    return attrValue_TextChanged;
                }
                else
                {
                    return delegate { return; };
                }

            }
            else
            {
                Type actionType = ThisAction.GetType();
                MethodInfo method = actionType.GetMethod(argAttr.onLeaveHandlerName);
                return new EventHandler(
                    (sender, e) => method.Invoke(ThisAction, new object[] { sender, e })
                );
            }
        }



        String GetArgumentName(FieldAndPropertyWrapper argument)
        {
            ActionArgumentAttribute argAttr = argument.GetArguementAttribute();
            return argAttr.displayName ?? argument.Name;
        }

        void attrValue_TextChanged(object sender, EventArgs e)
        {
            TextBox attrValue = sender as TextBox;
            String oldVal = attrValue.Text;
            FieldAndPropertyWrapper f = attrValue.Tag as FieldAndPropertyWrapper;
            ActionArgumentAttribute argInfo = f.GetArguementAttribute();

            Object value = f.Get<Object>(ThisAction);
            MethodInfo parser = f.GetMemberType().GetMethod("Parse", new Type[] { typeof(String) });
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

            f.Set(this.ThisAction, value);

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
        public virtual Action ThisAction
        {
            get
            {
                return thisAction;
            }
        }
        private ToolTip _statusToolTip;
        private string _statusMessage;

        /// <summary>
        /// 
        /// </summary>
        public event DeletedEventHandler Deleted;
        protected Action thisAction;

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

            var handler = this.Deleted;
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
        public CentipedeEventArgs(Type program, List<Action> actions, Dictionary<String, Object> variables)
            : base()
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

    class FieldAndPropertyWrapper
    {
        public Type DeclaringType
        {
            get
            {
                return member.DeclaringType;
            }
        }
        public FieldAndPropertyWrapper(MemberInfo member)
        {
            this.member = member;
            if (member is PropertyInfo)
            {
                _getter = o => (member as PropertyInfo).GetValue(o, null);
                _setter = (o, v) => (member as PropertyInfo).SetValue(o, v, null);
            }
            else
            {
                _getter = o => (member as FieldInfo).GetValue(o);
                _setter = (o, v) => (member as FieldInfo).SetValue(o, v);
            }
        }

        MemberInfo member = null;

        private readonly Expression<Func<object, object>> _getter;
        public T Get<T>(object o)
        {
            return (T)_getter.Compile()(o);
        }
        private readonly Expression<Action<object, object>> _setter;
        public void Set<T>(object o, T v)
        {
            _setter.Compile()(o, v);
        }


        public ActionArgumentAttribute GetArguementAttribute()
        {
            return member.GetCustomAttributes(typeof(ActionArgumentAttribute), true).Cast<ActionArgumentAttribute>().SingleOrDefault();
        }


        public string Name { get { return member.Name; } }

        public Type GetMemberType()
        {
            Type baseType;
            if (member is FieldInfo)
            {
                baseType = (member as FieldInfo).FieldType;
            }
            else
            {
                baseType = (member as PropertyInfo).PropertyType;
            }
            return baseType;

        }

        public enum FieldType
        {
            //first value listed is used for default()
            Other, String, Numeric, Boolean,
        }
        private static Dictionary<Type, FieldType> TypeMapping = new Dictionary<Type, FieldType>()
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

            Type baseType = GetMemberType();

            while (!TypeMapping.ContainsKey(baseType) && baseType != typeof(object))
            {
                baseType = baseType.BaseType;
            }

            FieldType fieldType;
            TypeMapping.TryGetValue(baseType, out fieldType);  //out's default value if key is not found

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

    }
}
