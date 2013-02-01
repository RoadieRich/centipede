using System;
using System.Collections.Generic;
using Centipede;
using Action = Centipede.Action;


namespace TestProject1
{
    class TestAction : Action
    {
        private bool Equals(TestAction other)
        {
            return string.Equals(FieldArgument, other.FieldArgument) && string.Equals(PropertyArgument, other.PropertyArgument);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param>
        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) ||
                    obj.GetType() == GetType() &&
                    Equals((TestAction)obj));
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            unchecked
            {
                return ((FieldArgument != null ? FieldArgument.GetHashCode() : 0) * 397) ^ (PropertyArgument != null ? PropertyArgument.GetHashCode() : 0) ^ ((Comment!=null?Comment.GetHashCode() : 0) * 1559);
            }
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        public static bool operator ==(TestAction left, TestAction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TestAction left, TestAction right)
        {
            return !Equals(left, right);
        }

        public readonly int CtorCalled;
        public int DoActionCalled;
        public int InitActionCalled;
        public int CleanupActionCalled;
        public int GetNextCalled;
        public int DisposeCalled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public TestAction(Dictionary<string, object> v)
                : base("TestAction", v)
        {
            FieldArgument = "field argument value";
            CtorCalled++;
        }

        /// <summary>
        /// Perform the action
        /// </summary>
        protected override void DoAction()
        {
            DoActionCalled++;
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
            InitActionCalled++;
        }

        /// <summary>
        /// Cleanup, e.g. saving local variables back into Variables
        /// </summary>
        protected override void CleanupAction()
        {
            CleanupActionCalled++;
            base.CleanupAction();
        }

        /// <summary>
        /// Get the next action, can be overridden to allow custom flow control
        /// </summary>
        /// <returns>Action</returns>
        public override Action GetNext()
        {
            GetNextCalled++;
            return base.GetNext();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            DisposeCalled++;
            base.Dispose();
        }

        // ReSharper disable MemberCanBePrivate.Global
        [ActionArgument(displayName = "Field Argument")]
        public readonly String FieldArgument;



        [ActionArgument(displayName = "Property Argument")]
        public String PropertyArgument
        {
            get;
            set;
        }
        // ReSharper restore MemberCanBePrivate.Global
    }
}
