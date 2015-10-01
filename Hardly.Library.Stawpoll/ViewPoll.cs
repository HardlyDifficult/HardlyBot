using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Hardly.Library.Strawpoll {
    public static class Strawpoll {
        public static Dictionary<string, int> GetJson(uint pollNum) {
            using(WebClient webClient = new WebClient()) {
                string reponse = webClient.DownloadString(@"https://strawpoll.me/api/v2/polls/" + pollNum);
                Console.WriteLine(reponse);
                JToken json = JToken.Parse(reponse);
                Dictionary<string, int> results = new Dictionary<string, int>();
                string[] options = json["options"].Values<string>().ToArray();
                int[] votes = json["votes"].Values<int>().ToArray();

                for(int i = 0; i < options.Length; i++) {
                    results.Add(options[i], votes[i]);
                    Console.WriteLine(options[i] + ": " + votes[i]);
                }
                return results;
            }
        }

        public static string GetWinner(uint pollNum) {
            Dictionary<string, int> poll = GetJson(pollNum);
            int topVote = 0;
            string topVoteOption = "";
            foreach(var pollSection in poll) {
                int votes = pollSection.Value;
                string option = pollSection.Key;
                if(votes > topVote) {
                    topVote = votes;
                    topVoteOption = option;
                }
            }
            return "The top vote was: " + topVoteOption + " with " + topVote + " votes.";
        }

    }
}