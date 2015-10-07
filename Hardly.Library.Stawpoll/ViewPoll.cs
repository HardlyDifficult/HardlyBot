using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Hardly.Library.Strawpoll {
    public static class Strawpoll {
        public static Dictionary<string, int> GetJson(uint pollNum) {
            using(WebClient webClient = new WebClient()) {
                string response = webClient.DownloadString(@"https://strawpoll.me/api/v2/polls/" + pollNum);
                Console.WriteLine(response);
                
                Dictionary<string, int> results = new Dictionary<string, int>();
                List<string> options = new List<string>();
                List<int> votes = new List<int>();

                if(response != null) {
                    response = response.GetAfter("options\":[\"");
                    while(response != null) {
                        var optionName = response.GetBefore("\"");
                        if(optionName != null) {
                            options.Add(optionName);
                        }
                        response = response.GetAfter("\",\"");
                    }

                    response = response.GetAfter("votes\":[\"");
                    while(response != null) {
                        var vote = response.GetBefore(",");
                        int voteNumber;
                        if(int.TryParse(vote, out voteNumber)) {
                            votes.Add(voteNumber);
                        }
                        response = response.GetAfter(",");
                    }
                }

                for(int i = 0; i < options.Count; i++) {
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