using System;
using System.Collections.Generic;
using System.Data;
using WFEngine.Activities.Core;
using WFEngine.Activities.Core.Helper;
using WFEngine.Activities.Core.Model;

namespace WFEngine.Activities.Basic
{
    public class Assign : WFActivity
    {
        List<string> operators = new List<string>()
        {
            "+",
            "-",
            "*",
            "/",
            "%"
        };
        public override WFResponse Run()
        {
            foreach (var argument in Arguments)
            {
                bool argumentContainsVariable = false;
                var argumentValueToString = argument.GetFirstArgumentFromJson<string>();
                if (argumentValueToString.Contains("$"))
                    argumentContainsVariable = true;
                WFVariable variable = VariableHelper.FindVariable(argument.Name, Variables);
                if (argumentContainsVariable)
                {
                    Type argumentType = Type.GetType(argument.ArgumentType);
                    argumentValueToString = argumentValueToString.ReplaceToVariables(Variables);
                    bool argumentConstaintOperator = false;
                    foreach (var _operator in operators)
                    {
                        if (argumentValueToString.Contains(_operator))
                        {
                            argumentConstaintOperator = true;
                            break;
                        }
                    }
                    if (argumentConstaintOperator)
                        using (DataTable dt = new DataTable())
                            variable.Value = Convert.ChangeType(dt.Compute(argumentValueToString, ""), argumentType);
                    else
                        variable.Value = Convert.ChangeType(argumentValueToString, argumentType);
                }
                else
                {
                    var argumentValue = argument.GetFirstArgumentFromJson();
                    variable.Value = argumentValue;
                }
            }
            return new WFResponse();
        }
    }
}
