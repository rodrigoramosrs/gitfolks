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
            ExtractUsersFromOrg();
            ExtractUsersFromOrgByRepoCommit();
            Console.WriteLine($"Done... ");
            return true;
        }

        private static void ExtractUsersFromOrgByRepoCommit()
        {
            Console.WriteLine($"Enumerating users from projects in org: { GlobalConfiguration.OrgName } ");

            Dictionary<int, Author> AuthorList = new Dictionary<int, Author>();
            StringBuilder userListContent = new StringBuilder();

            string rootPath = $"{GlobalConfiguration.OutputPath}/{GlobalConfiguration.OrgName}";

            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

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

            Console.WriteLine($"Found {AuthorList.Count} users through github commit");
            Console.WriteLine($" + [ Org - { GlobalConfiguration.OrgName } ]");
            foreach (var author in AuthorList)
            {
                userListContent.AppendLine($"{author.Value.Login} | { author.Value.HtmlUrl }");
                Console.WriteLine($" | {author.Value.Login} | { author.Value.HtmlUrl }");
            }
            
            
            

            File.WriteAllText($"{rootPath}/{GlobalConfiguration.OrgName}_commit_user_list.txt", userListContent.ToString());
        }

        private static void ExtractUsersFromOrg()
        {
            Console.WriteLine($"Enumerating users from org: { GlobalConfiguration.OrgName } ");

            string rootPath = $"{GlobalConfiguration.OutputPath}/{GlobalConfiguration.OrgName}";
            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

            var githubUserList = GithubService.GetAllColaboratorsFromOrg().GetAwaiter().GetResult();

            

            Console.WriteLine($"Found {githubUserList.Count} user(s) for org { GlobalConfiguration.OrgName }");
            Console.WriteLine($"");
            Console.WriteLine($" + [ Org - { GlobalConfiguration.OrgName } ]");
            StringBuilder userListContent = new StringBuilder();
            StringBuilder userListWithRepo = new StringBuilder();

            foreach (var ghUser in githubUserList)
            {
                userListContent.AppendLine($"{ghUser.Login} | { ghUser.HtmlUrl }");

                Console.WriteLine($" | {ghUser.Login} | { ghUser.HtmlUrl }");

            }

            File.WriteAllText($"{rootPath}/{GlobalConfiguration.OrgName}_user_list.txt", userListContent.ToString());
            //Process.Start($"{GlobalConfiguration.OutputPath}");
        }
    }
}
