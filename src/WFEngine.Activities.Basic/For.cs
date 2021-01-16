using System;
using System.Linq;
using WFEngine.Activities.Basic.Condition;
using WFEngine.Activities.Core;
using WFEngine.Activities.Core.Helper;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic
{
    public class For : WFActivity
    {
        public override WFResponse Run()
        {
            WFArgument startValueArgument = Arguments.FirstOrDefault(x => x.Name == "StartValue");
            object startValue = null;
            if (startValueArgument.IsVariable)
            {
                var startValueVariable = VariableHelper.FindVariable(startValueArgument.GetFirstArgumentFromJson<string>(), Variables);
                startValue = startValueVariable.Value;
            }
            else
            {
                startValue = Convert.ChangeType(startValueArgument.Value, Type.GetType(startValueArgument.ArgumentType));
            }

            WFArgument conditionArgument = Arguments.FirstOrDefault(x => x.Name == "ConditionItem");
            WFArgument assignArgument = Arguments.Where(x=>x != startValueArgument && x!= conditionArgument).FirstOrDefault();
            while (ConditionHelper.RunCondition(ref conditionArgument,Variables))
            {
                Current.Blocks.RunBlock(Variables);
                Assign assign = new Assign();
                assign.Variables = new System.Collections.Generic.List<WFVariable>();
                assign.Variables.AddRange(Variables);
                assign.Arguments = new System.Collections.Generic.List<WFArgument>();
                assign.Arguments.Add(assignArgument);
                assign.Run();
            }

            return new WFResponse();
        }
    }
}
