using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic.Condition
{
    public class ConditionItem
    {
        public WFArgument LeftItem { get; set; }
        public string Operator { get; set; }
        public WFArgument RightItem { get; set; }
    }
}
