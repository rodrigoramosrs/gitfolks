using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitFolks.Arguments
{
    public class DefaultOptions
    {

        [Option('o', "org", Required = true, HelpText = "Organization to scan.")]
        public string Org { get; set; }

        [Option('t', "token", Required = true, HelpText = "Github token.")]
        public string GithubToken { get; set; }
    }
}
