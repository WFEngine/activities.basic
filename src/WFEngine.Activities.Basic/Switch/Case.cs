using System.Collections.Generic;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic.Switch
{
    public class Case
    {
        public WFArgument Value { get; set; }
        public List<WFBlock> Blocks { get; set; }
    }
}
