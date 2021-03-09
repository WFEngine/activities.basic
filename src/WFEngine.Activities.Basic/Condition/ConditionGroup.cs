using System.Collections.Generic;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic.Condition
{
    public class ConditionGroup
    {
        public string ArgumentType { get; set; }
        public List<ConditionGroup> ParentConditions { get; set; }
        //public List<WFArgument> Conditions { get; set; }
        public ConditionItem ConditionItem { get; set; }
        public string Operator { get; set; }
        public List<WFBlock> Blocks { get; set; }
    }
}
