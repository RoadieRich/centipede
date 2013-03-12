using System;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    /// <summary>
    ///     Mark a field of a class as an argument for the function, used to format the ActionDisplayControl
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [MeansImplicitUse]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ActionArgumentAttribute : Attribute
    {

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public string displayControl;

        /// <summary>
        /// </summary>
        [CanBeNull]
        public String displayName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        public string onChangedHandlerName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        public String onLeaveHandlerName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        public string setterMethodName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        public String usage;

        /// <summary>
        ///     Anchor value for location of the argument's help.
        /// </summary>
        /// <remarks>
        ///     If <see cref="ActionCategoryAttribute.HelpText"/> or <see cref="ActionCategoryAttribute.HelpUri"/> are
        ///     set on the containing anchor, this can be set to an anchor within that page: <c>#ArgumentName</c>.
        /// </remarks>
        [CanBeNull]
        public String HelpUri;

        public Boolean Literal;
    }

    /// <summary>
    ///     Marks a class as an Action, to be displayed in the GUI listbox
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ActionCategoryAttribute : Attribute
    {
        // ReSharper disable InconsistentNaming

        /// <summary>
        ///     the category tab to add the action to
        /// </summary>
        [NotNull]
        public readonly String category;

        /// <summary>
        ///     name of a custom display control used to display the action on thr Actions listview
        /// </summary>
        public String displayControl;

        /// <summary>
        ///     The display name for the action, defaults to the classname
        /// </summary>
        public String displayName;

        /// <summary>
        ///     helptext for the action, displayed as a tooltip
        /// </summary>
        public String helpText;

        /// <summary>
        ///     name of the icon in the resource file
        /// </summary>
        public String iconName;

        /// <summary>
        ///     Uri of a help file related to the action
        /// </summary>
        /// <remarks>
        ///     If both this and <see cref="HelpText"/> are defined, <see cref="HelpText"/> value has priority.
        /// </remarks>
        public String HelpUri;

        /// <summary>
        ///     Html text for help related to the action
        /// </summary>
        /// <remarks>
        ///     If both this and <see cref="HelpUri"/> are defined, <see cref="HelpText"/> value has priority.
        /// </remarks>
        public String HelpText;

        /// <summary>
        ///     Marks a class as an Action, to be displayed in the GUI listbox
        /// </summary>
        /// <param name="category">
        ///     <see cref="ActionCategoryAttribute.category" />
        /// </param>
        public ActionCategoryAttribute([NotNull] String category)
        {
            this.category = category;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class DisplayTextAttribute : Attribute
    {
        public string DisplayString { get; set; }
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        private readonly string positionalString;

        // This is a positional argument
        public DisplayTextAttribute(string displayString)
        {
            DisplayString = displayString;
        }

        public static string ToDisplayString(Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            System.Reflection.FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the attributes
            DisplayTextAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(DisplayTextAttribute), false) as DisplayTextAttribute[];

            string strValue = value.ToString();

            // Return the first if there was a match.
            if (attribs != null && attribs.Length > 0)
            {
                strValue = attribs[0].DisplayString;
            }
            return strValue;
        }
    }
}
