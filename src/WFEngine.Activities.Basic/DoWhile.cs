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
            ConditionGroup conditionGroup = new ConditionGroup()
            {
                ArgumentType = typeof(ConditionGroup).FullName,
                ConditionItem = conditionItem,
                Operator = "AND",
                ParentConditions = new System.Collections.Generic.List<ConditionGroup>()
            };
            do
            {
                Current.Blocks.RunBlock(Variables);
            } while (ConditionHelper.RunCondition(conditionGroup,Variables));
            return new WFResponse();
        }
    }
}
