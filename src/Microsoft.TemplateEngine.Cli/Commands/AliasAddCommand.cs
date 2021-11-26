﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.TemplateEngine.Cli.Commands
{
    internal class AliasAddCommand : BaseAliasAddCommand
    {
        internal AliasAddCommand(ITemplateEngineHost host, ITelemetryLogger logger, NewCommandCallbacks callbacks) : base(host, logger, callbacks, "add")
        {
            IsHidden = true;
        }
    }

    internal class LegacyAliasAddCommand : BaseAliasAddCommand
    {
        internal LegacyAliasAddCommand(ITemplateEngineHost host, ITelemetryLogger logger, NewCommandCallbacks callbacks) : base(host, logger, callbacks, "--alias")
        {
            AddAlias("-a");
            IsHidden = true;
        }
    }

    internal class BaseAliasAddCommand : BaseCommand<AliasAddCommandArgs>
    {
        internal BaseAliasAddCommand(ITemplateEngineHost host, ITelemetryLogger logger, NewCommandCallbacks callbacks, string commandName)
            : base(host, logger, callbacks, commandName) { }

        protected override Task<NewCommandStatus> ExecuteAsync(AliasAddCommandArgs args, IEngineEnvironmentSettings environmentSettings, InvocationContext context) => throw new NotImplementedException();

        protected override AliasAddCommandArgs ParseContext(ParseResult parseResult) => new(this, parseResult);
    }

    internal class AliasAddCommandArgs : GlobalArgs
    {
        public AliasAddCommandArgs(BaseAliasAddCommand command, ParseResult parseResult) : base(command, parseResult)
        {
        }
    }
}
