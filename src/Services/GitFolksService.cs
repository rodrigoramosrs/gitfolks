using GitFolks.Utils;
using Octokit;
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

            Dictionary<int, Author> AuthorList = new Dictionary<int, Author>();

            //List<Author> authorList = new List<Author>();
            var ghProjectFromOrg = GithubService.GetAllGithubReposFrom(GlobalConfiguration.OrgName).GetAwaiter().GetResult();
            foreach (var project in ghProjectFromOrg)
            {
                //Extracting users from commit
                var commitList = GithubService.GetAllCommitsFromRepo(project.Id).GetAwaiter().GetResult();
                var distinctAuthorList = commitList.Select(x => x.Author).Distinct(new AuthorComparer()).ToList();

                foreach (var author in distinctAuthorList)
                {
                    if (author == null) continue;
                    if (AuthorList.ContainsKey(author.Id)) continue;

                    AuthorList.Add(author.Id, author);
                }
                //authorList.AddRange(distinctusers);

                //var teste = commitList[0].Commit.Message;
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
