using System;
using System.Linq;
using WFEngine.Activities.Core;
using WFEngine.Activities.Core.Helper;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic
{
    public class Random : WFActivity
    {
        public override WFResponse Run()
        {
            var returnArgument = Arguments.FirstOrDefault(x => x.Name == "ReturnValue");
            var startRangeArgument = Arguments.FirstOrDefault(x => x.Name == "StartRange");
            var endRangeArgument = Arguments.FirstOrDefault(x => x.Name == "EndRange");
            var returnValue = returnArgument.GetFirstArgumentFromJson<string>();
            WFVariable variable = VariableHelper.FindVariable(returnValue, Variables);
            var startRange =Convert.ChangeType(startRangeArgument.GetFirstArgumentFromJson<string>().ReplaceToVariables(Variables),Type.GetType(startRangeArgument.ArgumentType));
            var endRange =Convert.ChangeType(endRangeArgument.GetFirstArgumentFromJson<string>().ReplaceToVariables(Variables),Type.GetType(endRangeArgument.ArgumentType));
            System.Random rnd = new System.Random();
            variable.Value = Convert.ChangeType(rnd.Next((int)startRange, (int)endRange), Type.GetType(variable.Type));
            return new WFResponse();
        }
    }
}
