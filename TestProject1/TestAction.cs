using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Action = Centipede.Action;


namespace TestProject1
{
    class TestAction : Centipede.Action
    {
        public bool CtorCalled = false;
        public bool DoActionCalled = false;
        public bool InitActionCalled = false;
        public bool CleanupActionCalled = false;
        public bool GetNextCalled = false;
        public bool DisposeCalled = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public TestAction(Dictionary<string, object> v)
                : base("TestAction", v)
        {
            CtorCalled = true;
        }

        /// <summary>
        /// Perform the action
        /// </summary>
        protected override void DoAction()
        {
            DoActionCalled = true;
            TestFunctions();

        }

        public System.Action TestFunctions = () => { };

        /// <summary>
        /// Override in abstract subclasses to allow code to be executed to e.g. set up variables, 
        /// converting an object from Variables to a specific type.
        /// <example>
        /// protected override void InitAction()
        /// {
        ///     this.TextFile = Variables["TextFile"] as FileStream;
        /// }
        /// </example>
        /// </summary>
        protected override void InitAction()
        {
            base.InitAction();
            InitActionCalled = true;
        }

        /// <summary>
        /// Cleanup, e.g. saving local variables back into Variables
        /// </summary>
        protected override void CleanupAction()
        {
            CleanupActionCalled = true;
            base.CleanupAction();
        }

        /// <summary>
        /// Get the next action, can be overridden to allow custom flow control
        /// </summary>
        /// <returns>Action</returns>
        public override Action GetNext()
        {
            GetNextCalled = true;
            return base.GetNext();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            this.DisposeCalled = true;
            base.Dispose();
        }
    }
}
