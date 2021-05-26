﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Utils;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Buaa.AIBot.Services.CodeAnalyze
{
    public interface ICodeAnalyzeService
    {
        Task<CodeAnalyzeService.CodeAnalyzeResult> AnalyzeAsync(string code);
    }

    public class CodeAnalyzeService : ICodeAnalyzeService
    {
        public class CodeAnalyzeResult
        {
            public class CodeAnalyzeMessage
            {
                [JsonConverter(typeof(EnumJsonConverter<InfoLevel>))]
                public InfoLevel Level { get; set; }
                public string Desc { get; set; }
                public int Line { get; set; }
                public int Column { get; set; }
            }

            public string SourceCode { get; set; }
            public List<CodeAnalyzeMessage> Messages { get; set; }
        }

        private ICppCheckCallerFactory cppCheckCallerFactory;
        private ILogger<CodeAnalyzeService> logger;
        private Utils.GlobalCancellationTokenSource globalCancellationTokenSource;

        public CodeAnalyzeService(ICppCheckCallerFactory cppCheckCallerFactory, ILogger<CodeAnalyzeService> logger, GlobalCancellationTokenSource globalCancellationTokenSource)
        {
            this.cppCheckCallerFactory = cppCheckCallerFactory;
            this.logger = logger;
            this.globalCancellationTokenSource = globalCancellationTokenSource;
        }

        private static readonly IReadOnlySet<string> ignoredCategories = new HashSet<string>()
        {
            "missingIncludeSystem"
        };

        private bool TryGetDescription(CppCheckResult cppCheckResult, out string res)
        {
            if (ignoredCategories.Contains(cppCheckResult.Category))
            {
                res = null;
                return false;
            }
            res = CppCheckResultTanslation.Translate(cppCheckResult);
            return true;
        }

        public async Task<CodeAnalyzeResult> AnalyzeAsync(string code)
        {
            using (var cppcheck = await cppCheckCallerFactory.CreateCallerAsync(globalCancellationTokenSource.Token))
            {
                await cppcheck.WriteFileAsync(code);
                await cppcheck.FormatAsync();
                var res = await cppcheck.CppCheckAnalyzeAsync();
                var fmtCode = await cppcheck.ReadFileAsync();
                await cppcheck.CleanUpAsync();
                var messages = new List<CodeAnalyzeResult.CodeAnalyzeMessage>();
                foreach (var message in res)
                {
                    if (TryGetDescription(message, out var desc))
                    {
                        int line = -1;
                        int column = -1;
                        if (message.Location != null)
                        {
                            line = message.Location.Line;
                            column = message.Location.Column;
                        }
                        messages.Add(new CodeAnalyzeResult.CodeAnalyzeMessage()
                        {
                            Level = message.Level,
                            Desc = desc,
                            Line = line,
                            Column = column
                        });
                    }
                }
                var ret = new CodeAnalyzeResult()
                {
                    SourceCode = fmtCode,
                    Messages = messages
                };
                // logger.LogInformation(JsonSerializer.Serialize(ret));

                return ret;
            }
        }
    }
}
