using System;
using System.Collections.Generic;
using WFEngine.Activities.Core.Helper;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic.Condition
{
    public static class ConditionHelper
    {
        private static ConditionGroup oldConditionGroup = null;
        public static bool Run(this ConditionGroup conditionGroup, List<WFVariable> variables)
        {
            var result = true;
            for (int i = 0; i < conditionGroup.Conditions.Count; i++)
            {
                var conditionItem = conditionGroup.Conditions[i];
                var isConditionTrue = RunCondition(ref conditionItem, variables);
                if (!isConditionTrue)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static bool RunCondition(ref WFArgument argument, List<WFVariable> variables)
        {
            var result = true;
            if (argument.ArgumentType == typeof(ConditionItem).FullName)
            {
                var conditionItem = argument.GetFirstArgumentParse<ConditionItem>();
                var leftItemIsVariable = conditionItem.LeftItem.IsVariable;
                var rightItemIsVariable = conditionItem.RightItem.IsVariable;
                if (leftItemIsVariable)
                {
                    var leftVariableName = conditionItem.LeftItem.GetFirstArgumentFromJson<string>();
                    WFVariable leftVariable = VariableHelper.FindVariable(leftVariableName, variables);
                    if (rightItemIsVariable)
                    {
                        var rightVariableName = conditionItem.RightItem.GetFirstArgumentFromJson<string>();
                        WFVariable rightVariable = VariableHelper.FindVariable(rightVariableName, variables);
                        var leftVariableValue = leftVariable.GetFirstArgumentFromJson();
                        var rightVariableValue = rightVariable.GetFirstArgumentFromJson();
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
                    }
                    else
                    {
                        var leftVariableValue = leftVariable.Value;
                        var rightVariableValue = conditionItem.RightItem.GetFirstArgumentFromJson();
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
                    }
                }
                else
                {
                    var leftVariableValue = conditionItem.LeftItem.GetFirstArgumentFromJson();
                    if (rightItemIsVariable)
                    {
                        var rightVariableName = conditionItem.RightItem.GetFirstArgumentFromJson<string>();
                        WFVariable rightVariable = VariableHelper.FindVariable(rightVariableName, variables);
                        var rightVariableValue = rightVariable.Value;
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
                    }
                    else
                    {
                        var rightVariableValue = conditionItem.RightItem.GetFirstArgumentFromJson();
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
                    }
                }
            }
            else
            {                
                var conditionGroup = argument.GetFirstArgumentParse<ConditionGroup>();               
                var conditionResult = conditionGroup.Run(variables);
                if (oldConditionGroup == null)
                {
                    oldConditionGroup = conditionGroup;
                }
                else
                {
                    var oldConditionResult = oldConditionGroup.Run(variables);
                    result = RunCondition(conditionResult, oldConditionResult, oldConditionGroup.Operator);
                    oldConditionGroup = null;
                }                
            }
            return result;
        }

        private static bool RunCondition(object leftItemValue, object rightItemValue, string _operator)
        {
            if (_operator == "Equals")
            {
                return leftItemValue.Equals(rightItemValue);
            }
            if(_operator == "Not Equal")
            {
                return !leftItemValue.Equals(rightItemValue);
            }
            if(_operator == "Is Greater Than")
            {
                return (double)Convert.ChangeType(leftItemValue, typeof(Double)) > (double)Convert.ChangeType(rightItemValue, typeof(Double));
            }
            if (_operator =="Is Greater Than Or Equal To")
            {
                return (double)Convert.ChangeType(leftItemValue,typeof(Double)) >= (double)Convert.ChangeType(rightItemValue, typeof(Double));
            }
            if(_operator == "Is Less Than")
            {
                return (double)Convert.ChangeType(leftItemValue, typeof(Double)) < (double)Convert.ChangeType(rightItemValue, typeof(Double));
            }
            if(_operator == "Is Less Than Or Equal To")
            {
                return (double)Convert.ChangeType(leftItemValue, typeof(Double)) <= (double)Convert.ChangeType(rightItemValue, typeof(Double));
            }

            if (_operator == "AND")
            {                
                if ((bool)leftItemValue && (bool)rightItemValue)
                    return true;
                else
                    return false;
            }
            if(_operator == "OR")
            {
                if ((bool)leftItemValue || (bool)rightItemValue)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
