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
                            ShowGovernmentLinkForInstalling,
                                
                
                // OnUsing,
                    GetOSForUsing,
                        GetIDEForUsing,
                            GetCompilerForUsing,
                                ShowDocumentLinkForUsing,

            Gramma,
                // OnStandardLibary,
                    ShowLinksForStandardLibary,

                // OnStatement,
                    GetStatementTypeForStatement,
                        ShowLinksForStatement,

                // OnKeywords,
                    GetKeywordForKeywords,
                        ShowLinksForKeywords,

            WithCode,
                GetCode,
                    GetWrongCaseInput,
                        GetWrongCaseExpectOutput,
                            
        GetSimpleDescribe,
            ShowSerchResult,
                ShowDatabaseResult,
                    GetDetails,
                        RunAddQuestion,
    }

    public static class Configuration
    {
        public static Dictionary<StatusId, IBotStatusBehaviour<StatusId>> GetStatusBehaviours()
        {
            var list = new List<IBotStatusBehaviour<StatusId>>()
            {
                new WelcomeStatus(),
                    // On Installing
                        new EnvironmentStatus(),
                            new GetOSForInstallingStatus(),
                                new GetIDEForInstallingStatus(),
                                    new ShowGovernmentLinkForInstallingStatus(),

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
