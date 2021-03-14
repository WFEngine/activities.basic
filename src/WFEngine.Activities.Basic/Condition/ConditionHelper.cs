using System;
using System.Collections.Generic;
using System.Linq;
using WFEngine.Activities.Core.Helper;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic.Condition
{
    public static class ConditionHelper
    {
        //private static ConditionGroup oldConditionGroup = null;
        public static bool Run(this ConditionGroup conditionGroup, List<WFVariable> variables)
        {
            var result = true;
            result = RunCondition(conditionGroup, variables);
            return result;
        }

        public static bool RunCondition(ConditionGroup conditionGroup, List<WFVariable> variables)
        {
            var result = true;
            if (conditionGroup.ParentConditions.Any())
            {
                ConditionGroup oldConditionGroup = null;
                foreach (var parentCondition in conditionGroup.ParentConditions)
                {
                    if (conditionGroup == null || !conditionGroup.Equals(parentCondition))
                        oldConditionGroup = parentCondition;
                    result = RunCondition(parentCondition, variables);
                    var parentResult = RunCondition(oldConditionGroup, variables);
                    result = RunCondition(result, parentResult,oldConditionGroup.Operator);
                    if (!result) break;
                }
            }

            if(conditionGroup.ConditionItem.LeftItem!=null && conditionGroup.ConditionItem.RightItem != null)
            {
                var leftItemIsVariable = conditionGroup.ConditionItem.LeftItem.IsVariable;
                var rightItemIsVariable = conditionGroup.ConditionItem.RightItem.IsVariable;
                if (leftItemIsVariable)
                {
                    var leftVariableName = conditionGroup.ConditionItem.LeftItem.GetFirstArgumentFromJson<string>();
                    WFVariable leftVariable = VariableHelper.FindVariable(leftVariableName, variables);
                    if (rightItemIsVariable)
                    {
                        var rightVariableName = conditionGroup.ConditionItem.RightItem.GetFirstArgumentFromJson<string>();
                        WFVariable rightVariable = VariableHelper.FindVariable(rightVariableName, variables);
                        var leftVariableValue = leftVariable.GetFirstArgumentFromJson();
                        var rightVariableValue = rightVariable.GetFirstArgumentFromJson();
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionGroup.ConditionItem.Operator);
                    }
                    else
                    {
                        var leftVariableValue = leftVariable.Value;
                        var rightVariableValue = conditionGroup.ConditionItem.RightItem.GetFirstArgumentFromJson();
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionGroup.ConditionItem.Operator);
                    }
                }
                else
                {
                    var leftVariableValue = conditionGroup.ConditionItem.LeftItem.GetFirstArgumentFromJson();
                    if (rightItemIsVariable)
                    {
                        var rightVariableName = conditionGroup.ConditionItem.RightItem.GetFirstArgumentFromJson<string>();
                        WFVariable rightVariable = VariableHelper.FindVariable(rightVariableName, variables);
                        var rightVariableValue = rightVariable.Value;
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionGroup.ConditionItem.Operator);
                    }
                    else
                    {
                        var rightVariableValue = conditionGroup.ConditionItem.RightItem.GetFirstArgumentFromJson();
                        result = RunCondition(leftVariableValue, rightVariableValue, conditionGroup.ConditionItem.Operator);
                    }
                }                
            }

            return result;
        }

        //public static bool RunCondition(ref WFArgument argument, List<WFVariable> variables)
        //{
        //    var result = true;
        //    if (argument.ArgumentType == typeof(ConditionItem).FullName)
        //    {
        //        var conditionItem = argument.GetFirstArgumentParse<ConditionItem>();
        //        var leftItemIsVariable = conditionItem.LeftItem.IsVariable;
        //        var rightItemIsVariable = conditionItem.RightItem.IsVariable;
        //        if (leftItemIsVariable)
        //        {
        //            var leftVariableName = conditionItem.LeftItem.GetFirstArgumentFromJson<string>();
        //            WFVariable leftVariable = VariableHelper.FindVariable(leftVariableName, variables);
        //            if (rightItemIsVariable)
        //            {
        //                var rightVariableName = conditionItem.RightItem.GetFirstArgumentFromJson<string>();
        //                WFVariable rightVariable = VariableHelper.FindVariable(rightVariableName, variables);
        //                var leftVariableValue = leftVariable.GetFirstArgumentFromJson();
        //                var rightVariableValue = rightVariable.GetFirstArgumentFromJson();
        //                result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
        //            }
        //            else
        //            {
        //                var leftVariableValue = leftVariable.Value;
        //                var rightVariableValue = conditionItem.RightItem.GetFirstArgumentFromJson();
        //                result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
        //            }
        //        }
        //        else
        //        {
        //            var leftVariableValue = conditionItem.LeftItem.GetFirstArgumentFromJson();
        //            if (rightItemIsVariable)
        //            {
        //                var rightVariableName = conditionItem.RightItem.GetFirstArgumentFromJson<string>();
        //                WFVariable rightVariable = VariableHelper.FindVariable(rightVariableName, variables);
        //                var rightVariableValue = rightVariable.Value;
        //                result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
        //            }
        //            else
        //            {
        //                var rightVariableValue = conditionItem.RightItem.GetFirstArgumentFromJson();
        //                result = RunCondition(leftVariableValue, rightVariableValue, conditionItem.Operator);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var conditionGroup = argument.GetFirstArgumentParse<ConditionGroup>();
        //        var conditionResult = conditionGroup.Run(variables);
        //        if (oldConditionGroup == null)
        //        {
        //            oldConditionGroup = conditionGroup;
        //        }
        //        else
        //        {
        //            var oldConditionResult = oldConditionGroup.Run(variables);
        //            result = RunCondition(conditionResult, oldConditionResult, oldConditionGroup.Operator);
        //            oldConditionGroup = null;
        //        }
        //    }
        //    return result;
        //}

        private static bool RunCondition(object leftItemValue, object rightItemValue, string _operator)
        {
            if (_operator == "Equals")
            {
                return leftItemValue.Equals(rightItemValue);
            }
            if (_operator == "Not Equal")
            {
                return !leftItemValue.Equals(rightItemValue);
            }
            if (_operator == "Is Greater Than")
            {
                double leftItemIsDouble;
                double rightItemIsDouble;
                if (double.TryParse(leftItemValue.ToString(), out leftItemIsDouble) && double.TryParse(rightItemValue.ToString(), out rightItemIsDouble))
                    return (double)Convert.ChangeType(leftItemValue, typeof(Double)) > (double)Convert.ChangeType(rightItemValue, typeof(Double));

                char leftItemIsChar;
                char rightItemIsChar;
                if (char.TryParse(leftItemValue.ToString(), out leftItemIsChar) && char.TryParse(rightItemValue.ToString(), out rightItemIsChar))
                    return (char)Convert.ChangeType(leftItemValue, typeof(char)) > (char)Convert.ChangeType(leftItemValue, typeof(char));
            }
            if (_operator == "Is Greater Than Or Equal To")
            {
                double leftItemIsDouble;
                double rightItemIsDouble;
                if (double.TryParse(leftItemValue.ToString(), out leftItemIsDouble) && double.TryParse(rightItemValue.ToString(), out rightItemIsDouble))
                    return (double)Convert.ChangeType(leftItemValue, typeof(Double)) >= (double)Convert.ChangeType(rightItemValue, typeof(Double));

                char leftItemIsChar;
                char rightItemIsChar;
                if (char.TryParse(leftItemValue.ToString(), out leftItemIsChar) && char.TryParse(rightItemValue.ToString(), out rightItemIsChar))
                    return (char)Convert.ChangeType(leftItemValue, typeof(char)) >= (char)Convert.ChangeType(leftItemValue, typeof(char));
            }
            if (_operator == "Is Less Than")
            {
                double leftItemIsDouble;
                double rightItemIsDouble;
                if (double.TryParse(leftItemValue.ToString(), out leftItemIsDouble) && double.TryParse(rightItemValue.ToString(), out rightItemIsDouble))
                    return (double)Convert.ChangeType(leftItemValue, typeof(Double)) < (double)Convert.ChangeType(rightItemValue, typeof(Double));

                char leftItemIsChar;
                char rightItemIsChar;
                if (char.TryParse(leftItemValue.ToString(), out leftItemIsChar) && char.TryParse(rightItemValue.ToString(), out rightItemIsChar))
                    return (char)Convert.ChangeType(leftItemValue, typeof(char)) < (char)Convert.ChangeType(leftItemValue, typeof(char));
            }
            if (_operator == "Is Less Than Or Equal To")
            {
                double leftItemIsDouble;
                double rightItemIsDouble;
                if (double.TryParse(leftItemValue.ToString(), out leftItemIsDouble) && double.TryParse(rightItemValue.ToString(), out rightItemIsDouble))
                    return (double)Convert.ChangeType(leftItemValue, typeof(Double)) <= (double)Convert.ChangeType(rightItemValue, typeof(Double));

                char leftItemIsChar;
                char rightItemIsChar;
                if (char.TryParse(leftItemValue.ToString(), out leftItemIsChar) && char.TryParse(rightItemValue.ToString(), out rightItemIsChar))
                    return (char)Convert.ChangeType(leftItemValue, typeof(char)) <= (char)Convert.ChangeType(leftItemValue, typeof(char));
            }
            if (_operator == "Is Null")
            {
                return leftItemValue == null;
            }
            if (_operator == "Is Not Null")
            {
                return leftItemValue != null;
            }

            if (_operator == "AND")
            {
                if ((bool)leftItemValue && (bool)rightItemValue)
                    return true;
                else
                    return false;
            }
            if (_operator == "OR")
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
