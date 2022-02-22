using System;
using System.Collections.Generic;
using System.Text;

namespace GitFolks
{
    public static class GlobalConfiguration
    {
        public static string GithubToken { get; private set; }
        public static string OrgName { get; private set; }
        public static string WorkingPath { get; private set; }

        public static int GithubRateRepoLimit { get; set; } = 10;
        public static int GithubRateDorksLimit { get; set; } = 10;

        public static string OutputPath
        {
            get { return $"{GlobalConfiguration.WorkingPath}/gitfolks_output"; }
        }

        
        public static void SetGithubToken(string Token)
        {
            GithubToken = Token;
        }

        public static void SetOrgName(string orgName)
        {
            OrgName = orgName;
        }

        public static void SetWorkingPath(string workingPath)
        {
            WorkingPath = workingPath;
        }

       
    }
}
