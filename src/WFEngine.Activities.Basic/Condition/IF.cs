using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WFEngine.Activities.Core;
using WFEngine.Activities.Core.Helper;

namespace WFEngine.Activities.Basic.Condition
{
    public class IF : WFActivity
    {
        /* İhtiyaçlar
         * Değişkenleri Birbirleriyle Karşılaştırma 
         * Değişkenleri Bir Değer İle Karşılaştırma
         * Condition Grupları Oluşturabilme
         */
        public override WFResponse Run()
        {
            bool isConditionsTrue = false;
            foreach (var argument in Arguments)
            {
                if(argument.ArgumentType == typeof(ConditionGroup).FullName)
                {
                    var conditionGroup = argument.GetFirstArgumentParse<ConditionGroup>();
                    var isConditionTrue = conditionGroup.Run(Variables);    
                    if(isConditionTrue)
                    {
                        isConditionsTrue = true;
                        conditionGroup.Blocks.RunBlock(Variables);
                        break;
                    }
                }
            }

            if (!isConditionsTrue)
            {
                var elseArgument = Arguments.FirstOrDefault(x => x.ArgumentType == "WFEngine.Activities.Basic.Condition.Else");
                var blankConditionGroup = elseArgument.GetFirstArgumentParse<ConditionGroup>();
                if (blankConditionGroup.Blocks.Any())
                {
                    blankConditionGroup.Blocks[0].RunBlock(Variables);
                }
            }
            return new WFResponse();
        }
    }
}
