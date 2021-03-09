using Newtonsoft.Json;
using System.Linq;
using WFEngine.Activities.Basic.Condition;
using WFEngine.Activities.Core;
using WFEngine.Activities.Core.Helper;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic
{
    public class DoWhile : WFActivity
    {
        public override WFResponse Run()
        {
            var conditionArgument = Arguments.FirstOrDefault(x => x.Name == "Condition");
            var conditionItem = conditionArgument.GetFirstArgumentParse<ConditionItem>();
            WFArgument conditionItemArgument = new WFArgument()
            {
                ArgumentType = typeof(ConditionItem).FullName,
                IsConstant = false,
                IsValue = false,
                IsVariable = false,
                Name = "Condition",
                Value = JsonConvert.SerializeObject(conditionItem)
            };
            //do
            //{
            //    Current.Blocks.RunBlock(Variables);
            //} while (ConditionHelper.RunCondition(ref conditionItemArgument,Variables));            
            return new WFResponse();
        }
    }
}
