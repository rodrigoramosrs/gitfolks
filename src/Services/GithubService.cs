﻿using Octokit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GitFolks.Services
{
    public static class GithubService
    {
        private static readonly int RatelimitTimeout = 60000 * 2;

        

        private static GitHubClient BuildGithubClient()
        {
            var tokenAuth = new Credentials(GlobalConfiguration.GithubToken);

            var ghClient = new GitHubClient(new ProductHeaderValue("GITDORK"));
            ghClient.Credentials = tokenAuth;
            return ghClient;
        }

        public static async Task<IReadOnlyList<User>> GetAllColaboratorsFromRepo(long RepositoryId)
        {
            var ghClient = BuildGithubClient();
            //var commits = await ghClient.Repository.Commit.GetAll(RepositoryId);
            return await ghClient.Repository.Collaborator.GetAll(RepositoryId);
        }

        public static async Task<IReadOnlyList<GitHubCommit>> GetAllCommitsFromRepo(long RepositoryId)
        {
            var ghClient = BuildGithubClient();
            IReadOnlyList<GitHubCommit> result = null;
            do
            {
                try
                {
                    result = await ghClient.Repository.Commit.GetAll(RepositoryId);
                }
                catch (Octokit.ForbiddenException ex)
                {
                    Console.WriteLine("Waiting github rate limit...");
                    Thread.Sleep(RatelimitTimeout);
                }
                catch (Exception ex)
                {
                    result = new List<GitHubCommit>();
                    Console.WriteLine(ex.ToString());
                }
                


            } while (result == null);

            return result;
            //List<GitHubCommit> result = new List<GitHubCommit>();

            //var apiOptions = new ApiOptions
            //{
            //    StartPage = 1,
            //    PageSize = 100
            //};

            //IReadOnlyList<GitHubCommit> queryResult = null;
            //do
            //{
            //    queryResult = await ghClient.Repository.Commit.GetAll(RepositoryId, apiOptions);

            //    if (queryResult?.Count > 0)
            //        result.AddRange(queryResult);

            //    apiOptions.StartPage++;

            //} while (queryResult?.Count > 0);

            //return result;


        }

        public static async Task<IReadOnlyList<Branch>> GetAllBranchesFromRepo(long RepositoryId)
        {
            var ghClient = BuildGithubClient();
            IReadOnlyList<Branch> result = null;
            do
            {
                try
                {
                    result = await ghClient.Repository.Branch.GetAll(RepositoryId);
                }
                catch (Octokit.ForbiddenException ex)
                {
                    Console.WriteLine("Waiting github rate limit...");
                    Thread.Sleep(RatelimitTimeout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    result = new List<Branch>();
                }
            } while (result == null);
            return result;
        }

        public static async Task<List<Repository>> GetAllGithubReposFrom(string OrgOrUser, RepoSearchSort SortBy = RepoSearchSort.Updated)
        {
            var ghClient = BuildGithubClient();
            List<Repository> result = new List<Repository>();
            int TotalRepos = 0;
            int CurrentPage = 1;
            int PerPage = 100;


            var repoRequest = new SearchRepositoriesRequest();
            repoRequest.Archived = false;
            repoRequest.Fork = null;
            repoRequest.User = OrgOrUser;
            repoRequest.SortField = SortBy;

            //var FromDate = GlobalConfiguration.GithubFromDate.HasValue ? GlobalConfiguration.GithubFromDate.Value : DateTimeOffset.Now.AddMonths(-5);

            //repoRequest.Updated = new DateRange(FromDate, DateTimeOffset.Now.AddDays(1));

            int rateLimitCount = 0;
            repoRequest.PerPage = PerPage;
            do
            {
                try
                {
                    repoRequest.Page = CurrentPage;

                    var queryResult = await ghClient.Search.SearchRepo(repoRequest);
                    result.AddRange(queryResult.Items);

                    TotalRepos = queryResult.TotalCount;
                    CurrentPage++;
                    rateLimitCount++;

                    if (rateLimitCount > GlobalConfiguration.GithubRateRepoLimit)
                    {
                        rateLimitCount = 0;
                        Console.WriteLine("Waiting github rate limit...");
                        Thread.Sleep(RatelimitTimeout);
                    }
                }
                catch (Octokit.ForbiddenException ex)
                {
                    Console.WriteLine("Waiting github rate limit...");
                    Thread.Sleep(RatelimitTimeout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }


            } while (((CurrentPage * PerPage) < TotalRepos));

            return result;
        }

        static internal async Task<IReadOnlyList<User>> GetAllColaboratorsFromOrg()
        {
            var ghClient = BuildGithubClient();
            List<User> result = new List<User>();

            var apiOptions = new ApiOptions
            {
                StartPage = 1,
                //PageCount = CurrentPage,
                PageSize = 100
            };

            IReadOnlyList<User> queryResult = null;
            int rateLimitCount = 0;
            do
            {
                
                try
                {
                    queryResult = await ghClient.Organization.Member.GetAll(GlobalConfiguration.OrgName, apiOptions);

                    if (queryResult?.Count > 0)
                        result.AddRange(queryResult);

                    apiOptions.StartPage++;
                    rateLimitCount++;

                    if (rateLimitCount > GlobalConfiguration.GithubRateRepoLimit)
                    {
                        rateLimitCount = 0;
                        Console.WriteLine("Waiting github rate limit...");
                        Thread.Sleep(RatelimitTimeout);
                    }
                }
                catch (Octokit.ForbiddenException ex)
                {
                    Console.WriteLine("Waiting github rate limit...");
                    Thread.Sleep(RatelimitTimeout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }


            } while (queryResult?.Count > 0);

            return result;

        }

      
    }
}
