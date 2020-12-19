using WFEngine.Activities.Core;
using WFEngine.Activities.Core.Helper;

namespace WFEngine.Activities.Basic
{
    public class Container : WFActivity
    {      
        public override WFResponse Run()
        {
            Current.Blocks.RunBlock(Variables);
            return new WFResponse();
        }
    }
}
