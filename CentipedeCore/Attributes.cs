using System;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    /// <summary>
    ///     Mark a field of a class as an argument for the function, used to format the ActionDisplayControl
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [MeansImplicitUse]
    public sealed class ActionArgumentAttribute : Attribute
    {

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        [PublicAPI]
        public string displayControl;

        /// <summary>
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        public String displayName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        public string onChangedHandlerName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        public String onLeaveHandlerName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        public string setterMethodName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        public String usage;

        // ReSharper restore InconsistentNaming

    }

    /// <summary>
    ///     Marks a class as an Action, to be displayed in the GUI listbox
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
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
        ///     Marks a class as an Action, to be displayed in the GUI listbox
        /// </summary>
        /// <param name="category">
        ///     <see cref="ActionCategoryAttribute.category" />
        /// </param>
        public ActionCategoryAttribute(String category)
        {
            this.category = category;
        }
    }
}
