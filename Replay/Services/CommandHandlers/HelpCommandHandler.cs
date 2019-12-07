﻿using Replay.Logging;
using Replay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Replay.Services.CommandHandlers
{
    class HelpCommandHandler : ICommandHandler
    {
        public bool CanHandle(string input) =>
            "help".Equals(input, StringComparison.OrdinalIgnoreCase);

        public Task<LineEvaluationResult> HandleAsync(int lineId, string text, IReplLogger logger)
        {
            var help = new[]
            {
                new[]
                {
                    "Welcome to Replay! ❤",
                    "",
                    "Keyboard Shortcuts",
                    "==================",
                },
                KeyboardShortcuts.KeyboardShortcutHelp,
                new[]
                {
                    "",
                    "Commands",
                    "========",
                },
                CommandHelp
            };

            string helpText = string.Join(Environment.NewLine, help.SelectMany(linegroups => linegroups));

            return Task.FromResult(
                new LineEvaluationResult(null, helpText, null, null)
            );
        }

        private static IReadOnlyCollection<string> CommandHelp => new[]
        {
            @"#nuget mypackagename – search for and install a nuget package.",
            @"#r ""path/to/lib.dll"" – reference a DLL.",
            @"exit – exit the application.",
            @"help – this help text :)"
        };
    }
}
