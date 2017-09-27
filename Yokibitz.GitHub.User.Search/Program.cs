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

        static InMemoryCredentialStore credentials = new InMemoryCredentialStore(new Credentials("yokibitz", "somethingnew@1"));
        static GitHubClient client = new GitHubClient(new ProductHeaderValue("ophion"), credentials);

        static void Main(string[] args)
        {
            //var user = client.User.Get("yokibitz").Result;
            //Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
            //    user.Name,
            //    user.PublicRepos,
            //    user.Url);

            var request = new SearchUsersRequest("location:Singapore")
            {
                Location = "Singapore"
            };

            var users = client.Search.SearchUsers(request).Result;

            foreach(var user in users.Items)
            {
                Console.WriteLine("{0} - {1} - {2}", user.Login, user.Name, user.Location);
            }

            // this will take a while, let's wait for user input before exiting
            Console.ReadLine();
        }
    }
}
