using System.Linq;
using WFEngine.Activities.Core;
using WFEngine.Activities.Core.Helper;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic.Switch
{
    public class Switch : WFActivity
    {
        public override WFResponse Run()
        {
            bool IsConditionRun = false;
            var conditionArgument = Arguments.FirstOrDefault(x => x.ArgumentType == "WFEngine.Activities.Basic.Switch.ConditionItem");
            object conditionVal = null;
            if (conditionArgument.IsVariable)
            {
                var conditionVariableName = conditionArgument.GetFirstArgumentFromJson<string>();
                WFVariable variable = VariableHelper.FindVariable(conditionVariableName, Variables);
                conditionVal = variable.Value;
            }
            else
            {
                conditionVal = conditionArgument.GetFirstArgumentFromJson();
            }
            var caseBlocks = Arguments.Where(x => x.ArgumentType == typeof(Case).FullName).ToList();
            foreach (var caseBlock in caseBlocks)
            {
                var _case = caseBlock.GetFirstArgumentParse<Case>();
                object caseVal = null;
                if (_case.Value.IsVariable)
                {
                    var caseValueVariableName = _case.Value.GetFirstArgumentFromJson<string>();
                    WFVariable variable = VariableHelper.FindVariable(caseValueVariableName, Variables);
                    caseVal = variable.Value;
                }
                else
                {
                    caseVal = _case.Value.GetFirstArgumentFromJson();
                }

                IsConditionRun = conditionVal.Equals(caseVal);
                if (IsConditionRun)
                {
                    _case.Blocks.FirstOrDefault().RunBlock();
                    break;
                }
            }

            if (!IsConditionRun)
            {
                var defaultBlock = Arguments.FirstOrDefault(x => x.ArgumentType == typeof(Default).FullName);
                var def = defaultBlock.GetFirstArgumentParse<Default>();
                def.Blocks.FirstOrDefault().RunBlock();
            }

            return new WFResponse();
        }
    }
}
