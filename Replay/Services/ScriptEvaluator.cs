﻿using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Replay.Model;
using System.IO;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Replay.Services
{
    /// <summary>
    /// Evaluates C# code using the Roslyn Scripting API
    /// </summary>
    public class ScriptEvaluator
    {
        private ScriptOptions compilationOptions;
        private ScriptState<object> state;

        public ScriptEvaluator()
        {
            this.compilationOptions = ScriptOptions.Default
                .WithReferences(DefaultAssemblies.Assemblies.Value)
                .WithImports(DefaultAssemblies.DefaultUsings);

            var nugetCache = Path.Combine(
                Environment.GetEnvironmentVariable("UserProfile"),
                ".nuget",
                "packages"
            );
        }

        /// <summary>
        /// Run the script and return the result, capturing any exceptions or standard output.
        /// </summary>
        public async Task<EvaluationResult> EvaluateAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))

            {
                return new EvaluationResult();
            }

            using(var stdout = new ConsoleOutputWriter())
            {
                var evaluated = await EvaluateCapturingError(text);
                return new EvaluationResult
                {
                    ScriptResult = evaluated.Result,
                    Exception = evaluated.Exception,
                    StandardOutput = stdout.GetOutputOrNull()
                };
            }
        }

        private async Task<(ScriptState<object> Result, Exception Exception)> EvaluateCapturingError(string text)
        {
            try
            {
                state = state == null
                    ? await CSharpScript.RunAsync(text, compilationOptions)
                    : await state.ContinueWithAsync(text);
                return (state, state.Exception);
            }
            catch (Exception exception)
            {
                return (null, exception);
            }
        }
    }
}
