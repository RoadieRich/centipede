﻿using System;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    public class BaseCentipedeAttribute : Attribute
    {
        [UsedImplicitly]
        public static GetI18nCallbackDelegate I18n;

        protected static void GetI18n(ref string text)
        {

            GetI18nCallbackDelegate i18nCallback = I18n;

            if (i18nCallback != null)
            {
                text = i18nCallback(text);
            }
        }
    }


    /// <summary>
    ///     Mark a field of a class as an argument for the function, used to format the ActionDisplayControl
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [MeansImplicitUse]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [CLSCompliant(false)]
    public sealed class ActionArgumentAttribute : BaseCentipedeAttribute
    {// ReSharper disable CSharpWarnings::CS0612

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull, Obsolete("use DisplayControl")]
        public string displayControl;

        public String DisplayControl
        {
            get
            {
                return displayControl;
            }
            set
            {
                displayControl = value;
            }
        }



        /// <summary>
        /// </summary>
        [CanBeNull, Obsolete]
        public String displayName;

        /// <summary>
        /// </summary>
        [CanBeNull]
        public string DisplayName
        {
            get
            {
                GetI18n(ref this.displayName);
                return this.displayName;
            }
            set
            {
                this.displayName = value;
            }
        }

        /// <summary>
        /// </summary>
        [CanBeNull]
        public string OnChangedHandlerName
        {
            get
            {
                return this.onChangedHandlerName;
            }
            set
            {
                this.onChangedHandlerName = value;
            }
        }

        /// <summary>
        /// </summary>
        [CanBeNull]
        public string OnLeaveHandlerName
        {
            get
            {
                return this.onLeaveHandlerName;
            }
            set
            {
                this.onLeaveHandlerName = value;
            }
        }

        /// <summary>
        /// </summary>
        [CanBeNull]
        public string SetterMethodName
        {
            get
            {
                return this.setterMethodName;
            }
            set
            {
                this.setterMethodName = value;
            }
        }

        /// <summary>
        /// </summary>
        [CanBeNull]
        public string Usage
        {
            get
            {
                GetI18n(ref this.usage);
                return this.usage;
            }
            set
            {
                this.usage = value;
            }
        }

        /// <summary>
        ///     Anchor value for location of the argument's help.
        /// </summary>
        /// <remarks>
        ///     If <see cref="ActionCategoryAttribute.HelpText"/> or <see cref="ActionCategoryAttribute.HelpUri"/> are
        ///     set on the containing anchor, this can be set to an anchor within that page: <c>#ArgumentName</c>.
        /// </remarks>
        [CanBeNull]
        public string HelpUri { get; set; }

        public bool Literal { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// </summary>
        [CanBeNull]
        [Obsolete]
        public string onChangedHandlerName;

        /// <summary>
        /// </summary>
        [CanBeNull, Obsolete]
        public String onLeaveHandlerName;

        /// <summary>
        /// </summary>
        [CanBeNull, Obsolete]
        public string setterMethodName;

        /// <summary>
        /// </summary>
        [CanBeNull, Obsolete]
        public String usage;
    }

    /// <summary>
    ///     Marks a class as an Action, to be displayed in the GUI listbox
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [CLSCompliant(false)]
    public sealed class ActionCategoryAttribute : BaseCentipedeAttribute
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
        [Obsolete]
        public String displayControl;

        /// <summary>
        ///     The display name for the action, defaults to the classname
        /// </summary>
        [Obsolete]
        public String displayName;

        /// <summary>
        ///     helptext for the action, displayed as a tooltip
        /// </summary>
        public String Usage
        {
            get
            {
                GetI18n(ref this._usage);
                return this._usage;
            }
            set { this._usage = value; }
        }

        /// <summary>
        ///     name of the icon in the resource file
        /// </summary>
        public String IconName;


        /// <summary>
        ///     Html text for help related to the action
        /// </summary>
        /// <remarks>
        ///     If both this and <see cref="HelpUri"/> are defined, <see cref="HelpText"/> value has priority.
        /// </remarks>
        public string HelpText
        {
            get
            {
                GetI18n(ref this._helpText);
                return this._helpText;
            }
            set { this._helpText = value; }
        }

        /// <summary>
        ///     Uri of a help file related to the action
        /// </summary>
        /// <remarks>
        ///     If both this and <see cref="HelpText"/> are defined, <see cref="HelpText"/> value has priority.
        /// </remarks>
        public String HelpUri;

        private string _usage;
        private string _helpText;

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

        public string DisplayName
        {
            get
            {
                GetI18n(ref this.displayName);
                return displayName;
            }
            set
            {
                displayName = value;
            }
        }

        public string DisplayControl
        {
            get
            {
                return displayControl;
            }
            set
            {
                displayControl = value;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class DisplayTextAttribute : Attribute
    {
        public string DisplayString { get; set; }
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

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

    public delegate string GetI18nCallbackDelegate(string text);

}
