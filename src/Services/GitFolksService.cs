using GitFolks.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GitFolks.Services
{
    public static class GitFolksService
    {
        public static bool ListAllUsersFromOrg()
        {
            //return ExtractUsersFromOrg();

            return ExtractUsersFromOrgRepo();
        }

        private static bool ExtractUsersFromOrgRepo()
        {
            Console.WriteLine($"Enumerating users from projects in org: { GlobalConfiguration.OrgName } ");

            var ghProjectFromOrg = GithubService.GetAllGithubReposFrom(GlobalConfiguration.OrgName).GetAwaiter().GetResult();
            foreach (var project in ghProjectFromOrg)
            {
                //Extracting users from commit
                var users = GithubService.GetAllCommitsFromRepo(project.Id).GetAwaiter().GetResult();
                Dictionary<string, string> userLists = new Dictionary<string, string>();
                var distinctusers = users.Select(x => x.User).Distinct();
            }
            return true;
        }

        private static bool ExtractUsersFromOrg()
        {
            Console.WriteLine($"Enumerating users from org: { GlobalConfiguration.OrgName } ");

            var githubUserList = GithubService.GetAllColaboratorsFromOrg().GetAwaiter().GetResult();

            if (!Directory.Exists($"{GlobalConfiguration.OutputPath}")) Directory.CreateDirectory($"{GlobalConfiguration.OutputPath}");

            Console.WriteLine($"Found {githubUserList.Count} user(s) for org { GlobalConfiguration.OrgName }");
            Console.WriteLine($"");
            Console.WriteLine($" + [ Org - { GlobalConfiguration.OrgName } ]");
            StringBuilder userListContent = new StringBuilder();
            StringBuilder userListWithRepo = new StringBuilder();

            foreach (var ghUser in githubUserList)
            {
                userListContent.AppendLine($"{ghUser.Login} | { ghUser.Url }");

                Console.WriteLine($" | {ghUser.Login} | { ghUser.Url }");

            }

            File.WriteAllText($"{GlobalConfiguration.OutputPath}/{GlobalConfiguration.OrgName}_user_list.txt", userListContent.ToString());
            //Process.Start($"{GlobalConfiguration.OutputPath}");

            Console.WriteLine($"Done... ");
            return true;
        }
    }
}
