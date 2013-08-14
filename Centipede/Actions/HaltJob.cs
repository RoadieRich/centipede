using CentipedeInterfaces;

namespace Centipede.Actions
{
    [ActionCategory("Flow Control", DisplayName = "Halt Job", IconName="halt")]
    internal class HaltJob : Action
    {
        public HaltJob(System.Collections.Generic.IDictionary<string, object> variables, ICentipedeCore core)
            : base("Halt Job", variables, core)
        { }


        protected override void DoAction()
        { }

        public override IAction GetNext()
        {
            return null;
        }
    }
}
