﻿// Copyright © 2017 - 2021 Chocolatey Software, Inc
// Copyright © 2011 - 2017 RealDimensions Software, LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//
// 	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace chocolatey.tests.infrastructure.app.commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using chocolatey.infrastructure.app.attributes;
    using chocolatey.infrastructure.app.commands;
    using chocolatey.infrastructure.app.configuration;
    using chocolatey.infrastructure.app.domain;
    using chocolatey.infrastructure.app.services;
    using chocolatey.infrastructure.commandline;
    using Moq;
    using FluentAssertions;

    public class ChocolateySourceCommandSpecs
    {
        [ConcernFor("source")]
        public abstract class ChocolateySourceCommandSpecsBase : TinySpec
        {
            protected ChocolateySourceCommand command;
            protected Mock<IChocolateyConfigSettingsService> configSettingsService = new Mock<IChocolateyConfigSettingsService>();
            protected ChocolateyConfiguration configuration = new ChocolateyConfiguration();

            public override void Context()
            {
                configuration.Sources = "https://localhost/somewhere/out/there";
                command = new ChocolateySourceCommand(configSettingsService.Object);
            }
        }

        public class When_implementing_command_for : ChocolateySourceCommandSpecsBase
        {
            private List<string> results;

            public override void Because()
            {
                results = command.GetType().GetCustomAttributes(typeof(CommandForAttribute), false).Cast<CommandForAttribute>().Select(a => a.CommandName).ToList();
            }

            [Fact]
            public void Should_implement_source()
            {
                results.Should().Contain("source");
            }

            [Fact]
            public void Should_implement_sources()
            {
                results.Should().Contain("sources");
            }
        }

        public class When_configurating_the_argument_parser : ChocolateySourceCommandSpecsBase
        {
            private OptionSet optionSet;

            public override void Context()
            {
                base.Context();
                optionSet = new OptionSet();
                configuration.Sources = "https://localhost/somewhere/out/there";
            }

            public override void Because()
            {
                command.ConfigureArgumentParser(optionSet, configuration);
            }

            [Fact]
            public void Should_clear_previously_set_Source()
            {
                configuration.Sources.Should().BeEmpty();
            }

            [Fact]
            public void Should_add_name_to_the_option_set()
            {
                optionSet.Contains("name").Should().BeTrue();
            }

            [Fact]
            public void Should_add_short_version_of_name_to_the_option_set()
            {
                optionSet.Contains("n").Should().BeTrue();
            }

            [Fact]
            public void Should_add_source_to_the_option_set()
            {
                optionSet.Contains("source").Should().BeTrue();
            }

            [Fact]
            public void Should_add_short_version_of_source_to_the_option_set()
            {
                optionSet.Contains("s").Should().BeTrue();
            }

            [Fact]
            public void Should_add_user_to_the_option_set()
            {
                optionSet.Contains("user").Should().BeTrue();
            }

            [Fact]
            public void Should_add_short_version_of_user_to_the_option_set()
            {
                optionSet.Contains("u").Should().BeTrue();
            }

            [Fact]
            public void Should_add_password_to_the_option_set()
            {
                optionSet.Contains("password").Should().BeTrue();
            }

            [Fact]
            public void Should_add_short_version_of_password_to_the_option_set()
            {
                optionSet.Contains("p").Should().BeTrue();
            }
        }

        public class When_handling_additional_argument_parsing : ChocolateySourceCommandSpecsBase
        {
            private readonly IList<string> unparsedArgs = new List<string>();
            private Action because;

            public override void Because()
            {
                because = () => command.ParseAdditionalArguments(unparsedArgs, configuration);
            }

            public void Reset()
            {
                unparsedArgs.Clear();
                configSettingsService.ResetCalls();
            }

            [Fact]
            public void Should_use_the_first_unparsed_arg_as_the_subcommand()
            {
                Reset();
                unparsedArgs.Add("list");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.List);
            }

            [Fact]
            public void Should_throw_when_more_than_one_unparsed_arg_is_passed()
            {
                Reset();
                unparsedArgs.Add("wtf");
                unparsedArgs.Add("bbq");
                var errored = false;
                Exception error = null;

                try
                {
                    because();
                }
                catch (Exception ex)
                {
                    errored = true;
                    error = ex;
                }

                errored.Should().BeTrue();
                error.Should().NotBeNull();
                error.Should().BeOfType<ApplicationException>();
                error.Message.Should().Contain("A single sources command must be listed");
            }

            [Fact]
            public void Should_accept_add_as_the_subcommand()
            {
                Reset();
                unparsedArgs.Add("add");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.Add);
            }

            [Fact]
            public void Should_accept_uppercase_add_as_the_subcommand()
            {
                Reset();
                unparsedArgs.Add("ADD");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.Add);
            }

            [Fact]
            public void Should_remove_add_as_the_subcommand()
            {
                Reset();
                unparsedArgs.Add("remove");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.Remove);
            }

            [Fact]
            public void Should_accept_enable_as_the_subcommand()
            {
                Reset();
                unparsedArgs.Add("enable");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.Enable);
            }

            [Fact]
            public void Should_accept_disable_as_the_subcommand()
            {
                Reset();
                unparsedArgs.Add("disable");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.Disable);
            }

            [Fact]
            public void Should_set_unrecognized_values_to_list_as_the_subcommand()
            {
                Reset();
                unparsedArgs.Add("wtf");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.List);
            }

            [Fact]
            public void Should_default_to_list_as_the_subcommand()
            {
                Reset();
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.List);
            }

            [Fact]
            public void Should_handle_passing_in_an_empty_string()
            {
                Reset();
                unparsedArgs.Add(" ");
                because();

                configuration.SourceCommand.Command.Should().Be(SourceCommandType.List);
            }
        }

        public class When_validating : ChocolateySourceCommandSpecsBase
        {
            private Action because;

            public override void Because()
            {
                because = () => command.Validate(configuration);
            }

            [Fact]
            public void Should_throw_when_command_is_not_list_and_name_is_not_set()
            {
                configuration.SourceCommand.Name = "";
                const string expectedMessage = "When specifying the subcommand '{0}', you must also specify --name.";
                VerifyExceptionThrownOnCommand(expectedMessage);
            }

            [Fact]
            public void Should_throw_when_command_is_add_and_source_is_not_set()
            {
                configuration.SourceCommand.Name = "irrelevant";
                configuration.Sources = string.Empty;
                const string expectedMessage = "When specifying the subcommand '{0}', you must also specify --source.";
                VerifyExceptionThrownOnCommand(expectedMessage);
            }

            private void VerifyExceptionThrownOnCommand(string expectedMessage)
            {
                configuration.SourceCommand.Command = SourceCommandType.Add;

                var errored = false;
                Exception error = null;

                try
                {
                    because();
                }
                catch (Exception ex)
                {
                    errored = true;
                    error = ex;
                }

                errored.Should().BeTrue();
                error.Should().NotBeNull();
                error.Should().BeOfType<ApplicationException>();
                var commandName = configuration.SourceCommand.Command.ToStringSafe().ToLower();
                error.Message.Should().Be(expectedMessage.FormatWith(commandName));
            }

            [Fact]
            public void Should_continue_when_command_is_list_and_name_is_not_set()
            {
                configuration.SourceCommand.Command = SourceCommandType.List;
                configuration.SourceCommand.Name = "";
                because();
            }

            [Fact]
            public void Should_continue_when_command_is_not_list_and_name_is_set()
            {
                configuration.SourceCommand.Command = SourceCommandType.List;
                configuration.SourceCommand.Name = "bob";
                because();
            }
        }

        public class When_noop_is_called : ChocolateySourceCommandSpecsBase
        {
            public override void Because()
            {
                command.DryRun(configuration);
            }

            [Fact]
            public void Should_call_service_noop()
            {
                configSettingsService.Verify(c => c.DryRun(configuration), Times.Once);
            }
        }

        public class When_run_is_called : ChocolateySourceCommandSpecsBase
        {
            private Action because;

            public override void Because()
            {
                because = () => command.Run(configuration);
            }

            [Fact]
            public void Should_call_service_source_list_when_command_is_list()
            {
                configuration.SourceCommand.Command = SourceCommandType.List;
                because();
                configSettingsService.Verify(c => c.ListSources(configuration), Times.Once);
            }

            [Fact]
            public void Should_call_service_source_add_when_command_is_add()
            {
                configuration.SourceCommand.Command = SourceCommandType.Add;
                because();
                configSettingsService.Verify(c => c.AddSource(configuration), Times.Once);
            }

            [Fact]
            public void Should_call_service_source_remove_when_command_is_remove()
            {
                configuration.SourceCommand.Command = SourceCommandType.Remove;
                because();
                configSettingsService.Verify(c => c.RemoveSource(configuration), Times.Once);
            }

            [Fact]
            public void Should_call_service_source_disable_when_command_is_disable()
            {
                configuration.SourceCommand.Command = SourceCommandType.Disable;
                because();
                configSettingsService.Verify(c => c.DisableSource(configuration), Times.Once);
            }

            [Fact]
            public void Should_call_service_source_enable_when_command_is_enable()
            {
                configuration.SourceCommand.Command = SourceCommandType.Enable;
                because();
                configSettingsService.Verify(c => c.EnableSource(configuration), Times.Once);
            }
        }

        public class When_list_is_called : ChocolateySourceCommandSpecsBase
        {
            private Action because;

            public override void Because()
            {
                because = () => command.List(configuration);
            }

            [Fact]
            public void Should_call_service_source_list_when_command_is_list()
            {
                configuration.SourceCommand.Command = SourceCommandType.List;
                because();
                configSettingsService.Verify(c => c.ListSources(configuration), Times.Once);
            }
        }
    }
}
