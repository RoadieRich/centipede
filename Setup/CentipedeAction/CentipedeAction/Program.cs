// 0. References.  References to Action.dll and CentipedeInterfaces.dll are required, anything else
// is up to the programmer.  Please ensure "Copy To Local" is set to false in the reference properties.
using CentipedeInterfaces;
using Action = Centipede.Action;


// 1. Namespace: 
// Anything *except* Centipede
namespace CentipedeAction
{
    // 2. Create an action.  Actions are a public subclass of Centipede.Action, marked with 
    // CentipedeInterfaces.ActionCategoryAttribute. 
    // The first argument is the Tab to display the action on in the gui, other named arguments
    // control how it is displayed.  It is recommended to at least specify iconName.
    [ActionCategory("My Category", iconName = "MyIcon")]
    public class MyAction : Action
    {
        // 3. Constructor.  This exact signature is required on classes with the ActionCategoryAttribute.
        // The first argument to base() is the name of the action.  
        // It is not recommended to do too much in here, as it is only executed once, when the action is 
        // first added to the job.
        public MyAction(System.Collections.Generic.IDictionary<System.String, System.Object> variables, 
                        ICentipedeCore core)
                : base("Action Name", variables, core)
        { }

        // 4. Arguments.  An argument is simply a property or field marked with ActionArgumentAttribute.
        // It can be any type, although only int and family, double and family, string and bool are
        // supported as standard.  To make use of other types, it is recommended to create a custom
        // ActionDisplayControl (see relevant file).
        [ActionArgument]
        public string MyArgument1 = "Default Value";

        [ActionArgument]
        public int MyArgument2
        {
            get
            {
                return this._myArgument2;
            }
            set
            {
                this._myArgument2 = value;
            }
        }
        private int _myArgument2;

        // 5. Setup and Cleanup. These are performed every time the action is run - and are really intended for
        // use in an abstract base class, to perform init and cleanup common to all actions derived
        // from it.  Generally, this will be used to initialise class members from Variables, and to 
        // save them back afterwards.
        protected override void InitAction()
        {
            
        }
        protected override void CleanupAction()
        {
            
        }

        // 6. Actually do the action.  This is where the heavy lifting is intended to be done.  The 
        // job's Variables can be accessed through the Variables member, if you need to get other 
        // information about the job, use the members of the ICentipedeCore returned by GetCurrentCore().
        // 
        // GUI interactions should be performed using the Action.Ask or Action.Message methods.
        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="T:CentipedeInterfaces.ActionException">the action cannot be completed</exception>
        /// <exception cref="T:CentipedeInterfaces.FatalActionException">The job needs to halt</exception>
        protected override void DoAction()
        {
            // 7. Exceptions. You can throw any type of exception in any of the InitAction, DoAction,
            // and CleanupAction, although it is suggested that it is a subclass of ActionException.
            // You can also throw a FatalActionException, if it is not possible to continue processing
            // the job, although this should be discouraged.
        }

        // 8. Advanced interface The following overrides are all optional - there are sensible defaults 
        // provided by Action.
        
        // 8.1 Should be set to a value representing the relative time taken to execute the action.  
        // Defaults to 1.
        public override int Complexity
        {
            get
            {
                return base.Complexity;
            }
        }

        // 8.2 Custom load and save methods - obviously if one is implemented, the other should be, too
        // For example, if an argument can have a long value, that would be inappropriate to store in an
        // attribute on an xml tag - this could be stored as contents for the tag.
        public override void AddToXmlElement(System.Xml.XmlElement rootElement)
        {
            base.AddToXmlElement(rootElement);
        }
        protected override void PopulateMembersFromXml(System.Xml.XPath.XPathNavigator element)
        {
            base.PopulateMembersFromXml(element);
        }

        // 8.3 There is little reason to override GetNext, unless you are implementing your own flow 
        // control actions
        public override IAction GetNext()
        {
            return base.GetNext();
        }
        
        // 8.4 If any members do not clean up correctly, e.g. COM+ interop interfaces, this is where to do
        // that.  It is only ever called once on each action in the Job, usually when the action is deleted or the 
        // application is closed.
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}

// 9. Resources. No resources are required, but it is highly recommended to add an Icon, as mentioned
// above, ensuring it is publicly visible.  Enter the icon's Resources member name into the iconName argument of the ActionCategoryAttributes.

// 10. Compilation. Specify an assembly name (filename) in the project properties, and compile to a class library,
// which will receive the extention .dll, and copy it into the Centipede/Plugins folder.  Optionally, set that folder 
// as the output path in the Build tab of the Project Properties.