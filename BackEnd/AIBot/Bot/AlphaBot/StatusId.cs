using Buaa.AIBot.Bot.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.AlphaBot.Status;

namespace Buaa.AIBot.Bot.AlphaBot
{
    public enum StatusId
    {
        Welcome,

            Environment,

                // OnInstalling,
                    GetOSForInstalling,
                        GetIDEForInstalling,
                            GovernmentLinkForInstalling,
                                
                
                // OnUsing,
                    GetOSForUsing,
                        GetIDEForUsing,
                            GetCompilerForUsing,
                                DocumentLinkForUsing,

            Gramma,
                OnStandardLibary,
                    ShowLinksForStandardLibary,

                OnStatement,
                    GetStatementTypeForStatement,
                        ShowLinksForStatement,

                OnKeywords,
                    GetKeywordForKeywords,
                        ShowLinksForKeywords,

            WithCode,
                GetCode,
                    GetWrongInput,
                        GetWrongOutput,
                            
        GetSimpleDescribe,
            ShowSerchResult,
                ShowDatabaseResult,
                    GetDetails,
                        AddQuestion,
    }

    public static class Configuration
    {
        public static Dictionary<StatusId, IBotStatusBehaviour<StatusId>> GetStatusBehaviours()
        {
            var list = new List<IBotStatusBehaviour<StatusId>>() 
            { 
                new WelcomeStatus(),
                new GetSimpleDescribeStatus(),
                new GetDetailsStatus(),
                new AddQuestionStatus()
            };
            var ret = new Dictionary<StatusId, IBotStatusBehaviour<StatusId>>();
            foreach (var status in list)
            {
                ret[status.Id] = status;
            }
            return ret;
        }
    }
}
