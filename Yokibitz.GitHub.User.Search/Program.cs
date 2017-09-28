using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yokibitz.GitHub.User.Search
{
    class Program
    {
        static string owner = "github";
        static string name = "gitignore";

        static InMemoryCredentialStore credentials = new InMemoryCredentialStore(new Credentials("f8357fb1fc2147c80a8bc82b8e03d9fa036e2e5b"));
        static GitHubClient client = new GitHubClient(new ProductHeaderValue("ophion"), credentials);

        static void Main(string[] args)
        {
            var request = new SearchUsersRequest("location:Singapore");
            var totalNumberOfHits = client.Search.SearchUsers(request).Result.TotalCount;
            int monthInterval = 4;
            
            DateTime createdAt = new DateTime(2007, 10, 1);
            List<string> resultText = new List<string>();

            while (resultText.Count < totalNumberOfHits)
            {
                int resultsPerPage = 100;
                int currentPage = 1;
                int numberOfHits = 0;
                do
                {
                    System.Threading.Thread.Sleep(2000);
                    request.Created = new DateRange(createdAt, createdAt.AddMonths(monthInterval).AddDays(-1));
                    var internalResult = client.Search.SearchUsers(request).Result;
                    var users = internalResult;
                    foreach (var user in users.Items)
                    {
                        resultText.Add(string.Format("{0} - {1} - {2}", user.Login, user.Name, user.Location));
                    }

                    currentPage++;
                    request.Page = currentPage;
                    numberOfHits = internalResult.TotalCount;
                }
                while (numberOfHits - (currentPage * resultsPerPage) > -resultsPerPage);
                Console.WriteLine("{0} result for {1}", numberOfHits, createdAt.ToShortDateString());
                currentPage = 1;
                request.Page = currentPage;
                numberOfHits = 0;
                createdAt = createdAt.AddMonths(monthInterval);
            }

            System.Diagnostics.Debug.WriteLine("{0} users found", resultText.Count.ToString());

            //foreach(string user in resultText)
            //{
            //    Console.WriteLine(user);
            //}

            // this will take a while, let's wait for user input before exiting
            Console.ReadLine();
        }
    }
}
