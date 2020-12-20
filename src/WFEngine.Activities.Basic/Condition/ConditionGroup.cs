using System.Collections.Generic;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic.Condition
{
    public class ConditionGroup
    {
        public string Type { get; set; }
        public List<WFArgument> Conditions { get; set; }
        public string Operator { get; set; }
        public List<WFBlock> Blocks { get; set; }
    }
}
