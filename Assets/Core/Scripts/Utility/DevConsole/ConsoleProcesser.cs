using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Console
{
    public class ConsoleProcesser
    {
        readonly string prefix;
        readonly IEnumerable<IConsoleCommand> commands;

        public ConsoleProcesser(string prefix, IEnumerable<IConsoleCommand> commands)
        {
            this.prefix = prefix;
            this.commands = commands;
        }

        public void Process(ConsoleCommand command)
        {

        }

        public void Process(string commandInput)
        {
            if (!commandInput.StartsWith(prefix))
                return;

            commandInput = commandInput.Remove(0, prefix.Length);

            string[] args = commandInput.Split(' ');
            commandInput = args[0];
            args = args.Skip(1).ToArray();
            
            Process(commandInput, args);
        }

        public void Process(string commandInput, string[] args)
        {
            foreach (ConsoleCommand command in commands)
            {
                if (!commandInput.Equals(command.Command, System.StringComparison.OrdinalIgnoreCase))
                    continue;

                if (command.Process(args))
                {
                    return;
                }
            }
        }
    }
}
