﻿using System;
using System.Collections.Generic;
using TwitchLib;
using System.Linq;
using Discord;
using TwitchLib.Models.Client;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace JefBot.Commands
{
    internal class HelpPluginCommand : IPluginCommand
    {
        public string PluginName => "Help";
        public string Command => "help";
        public string Help => "!help {command}";
        public IEnumerable<string> Aliases => new[] { "h" };
        public bool Loaded { get; set; } = true;

        List<IPluginCommand> plug = new List<IPluginCommand>();
        Random rng = new Random();


        public async Task<string> Action(Message message)
        {
            string res = null;
            await Task.Run(() => { res = CommandHelp(message); });
            return res;
        }


        public string CommandHelp(Message message)
        {
            try
            {
                if (message.Arguments.Count > 0)
                {
                    var args = message.Arguments;
                    var result = "";
                    plug = new List<IPluginCommand>();
                    plug.AddRange(Bot._plugins.Where(plug => plug.Aliases.Contains(args[0])).ToList());
                    plug.AddRange(Bot._plugins.Where(plug => plug.Command == args[0]).ToList());

                    foreach (var item in plug)
                    {
                        if (item.Command == args[0] || item.Aliases.Contains(args[0]))
                        {
                            result = item.Help;
                            break;
                        }
                    }
                    if (result == "" || result == null)
                        result = $"No command / alias found for {args[0]} and therefore no help can be given";
                    return $"{result}";
                }
                return $"{Help}";
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return e.Message;
            }
        }
    }
}
