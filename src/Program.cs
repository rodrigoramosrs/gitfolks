using CommandLine;
using GitFolks.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GitFolks
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize(args);
            DoWork();
        }

        private static void DoWork()
        {
            bool result = GitFolks.Services.GitFolksService.ListAllUsersFromOrg();

            Environment.Exit(result ? 1 : -1);
        }

        private static void Initialize(string[] args)
        {
            Console.WriteLine($"GitFolks - {Assembly.GetEntryAssembly().GetName().Version}");

            CommandLine.Parser.Default.ParseArguments<Arguments.DefaultOptions>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);

            Console.WriteLine();
        }

        static void RunOptions(Arguments.DefaultOptions opts)
        {
            //handle options
            //if (string.IsNullOrEmpty(opts.Org))
            //{
            //    Console.WriteLine("Please provide organization to scan. to see all parameters use -h");
            //    Environment.Exit(-1);
            //}

            GlobalConfiguration.SetWorkingPath(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));

            GlobalConfiguration.SetGithubToken(opts.GithubToken);

            GlobalConfiguration.SetOrgName(opts.Org);

            if (!Directory.Exists(GlobalConfiguration.OutputPath))
                Directory.CreateDirectory(GlobalConfiguration.OutputPath);

            Console.WriteLine($"OS: {OS.GetCurrent()} - Architecture: {RuntimeInformation.OSArchitecture.ToString()} architecture");
            Console.WriteLine($"Working Path: {GlobalConfiguration.WorkingPath}");
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {

            foreach (var error in errs)
            {
                if (error.Tag != ErrorType.MissingRequiredOptionError) continue;

                Environment.Exit(-1);
            }

        }
    }
}
