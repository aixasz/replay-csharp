﻿using Replay.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Replay.Services.CommandHandlers
{
    /// <summary>
    /// An <see cref="ICommandHandler" /> for installing nuget packages.
    /// Handles lines like "#nuget Newtonsoft.Json"
    /// </summary>
    class NugetReferenceCommandHandler : ICommandHandler
    {
        private readonly ScriptEvaluator scriptEvaluator;
        private readonly WorkspaceManager workspaceManager;
        private readonly NugetPackageInstaller nugetInstaller;
        private const string CommandPrefix = "#nuget ";

        public NugetReferenceCommandHandler(ScriptEvaluator scriptEvaluator, WorkspaceManager workspaceManager, NugetPackageInstaller nugetInstaller)
        {
            this.scriptEvaluator = scriptEvaluator;
            this.workspaceManager = workspaceManager;
            this.nugetInstaller = nugetInstaller;
        }

        public bool CanHandle(string input) => input.StartsWith(CommandPrefix);

        public async Task<LineEvaluationResult> HandleAsync(int lineId, string text, IReplLogger logger)
        {
            string package = text.Substring(CommandPrefix.Length).Trim('"');
            var assemblies = (await nugetInstaller.Install(package, logger)).ToArray();
            if (assemblies.Any())
            {
                await scriptEvaluator.AddReferences(assemblies);
                await workspaceManager.CreateOrUpdateSubmissionAsync(lineId, string.Empty, assemblies);
            }
            return LineEvaluationResult.NoOutput;
        }
    }
}
