using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardly.Games;

namespace Hardly.Library.Twitch {
    public abstract class HoldemStatePlay : GameState<TwitchHoldem> {
        // TODO add timers (not statically). TimerSet timer = new TimerSet(new System.TimeSpan[] { TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10) }, new Action[] { WarningTimer, FinalTimer });
        TexasHoldemPlayer<SqlTwitchUser> lastKnownPlayer = null;

        public HoldemStatePlay(TwitchHoldem controller) : base(controller) {
            AddCommand(controller.room, "call", CallCommand, "Call...", null, false, TimeSpan.FromSeconds(0), false);
            AddCommand(controller.room, "raise", RaiseCommand, "Call...", null, false, TimeSpan.FromSeconds(0), false);
            AddCommand(controller.room, "check", CheckCommand, "Call...", null, false, TimeSpan.FromSeconds(0), false);
            AddCommand(controller.room, "fold", FoldCommand, "Call...", null, false, TimeSpan.FromSeconds(0), false);
            AddCommand(controller.room, "bet", BetCommand, "Call...", null, false, TimeSpan.FromSeconds(0), false);
        }

        private void BetCommand(SqlTwitchUser speaker, string additionalText) {
            if(speaker.id == controller.game.CurrentPlayer.playerIdObject.id) {
                ulong amount = controller.room.pointManager.GetPointsFromString(additionalText);
                if(controller.game.Bet(amount)) {
                    controller.room.SendWhisper(speaker, "You bet.");
                } else {
                    // ?
                }
            } else {
                controller.room.SendWhisper(speaker, "Not your turn, hold on...");
            }

            CheckForNextState();
        }

        private void FoldCommand(SqlTwitchUser speaker, string arg2) {
            if(speaker.id == controller.game.CurrentPlayer.playerIdObject.id) {
                if(controller.game.Fold()) {
                    controller.room.SendWhisper(speaker, "Later dude.");
                } else {
                    // ?
                }
            } else {
                controller.room.SendWhisper(speaker, "Not your turn, hold on...");
            }

            CheckForNextState();
        }

        private void CheckCommand(SqlTwitchUser speaker, string arg2) {
            if(speaker.id == controller.game.CurrentPlayer.playerIdObject.id) {
                if(controller.game.Check()) {
                    controller.room.SendWhisper(speaker, "Check check.");
                } else {
                    // ?
                }
            } else {
                controller.room.SendWhisper(speaker, "Not your turn, hold on...");
            }

            CheckForNextState();
        }

        private void RaiseCommand(SqlTwitchUser speaker, string additionalText) {
            if(speaker.id == controller.game.CurrentPlayer.playerIdObject.id) {
                ulong amount = controller.room.pointManager.GetPointsFromString(additionalText);
                if(amount <= 0) {
                    amount = 1;
                }
                if(controller.game.Raise(amount)) {
                    controller.room.SendWhisper(speaker, "You rose " + controller.room.pointManager.ToPointsString(amount));
                } else {
                    // ?
                }
            } else {
                controller.room.SendWhisper(speaker, "Not your turn, hold on...");
            }

            CheckForNextState();
        }

        private void CallCommand(SqlTwitchUser speaker, string arg2) {
            if(speaker.id == controller.game.CurrentPlayer.playerIdObject.id) {
                if(controller.game.Call()) {
                    controller.room.SendWhisper(speaker, "Called.");
                } else {
                    // ?
                }
            } else {
                controller.room.SendWhisper(speaker, "Not your turn, hold on...");
            }

            CheckForNextState();
        }

        private void CheckForNextState() {
            Log.debug("Twitch Game: checking round " + controller.game.round.ToString());

            switch(controller.game.round) {
            case Games.TexasHoldem<SqlTwitchUser>.Round.PreFlop:
                // Do nothing
                break;
            case Games.TexasHoldem<SqlTwitchUser>.Round.Flop:
                if(controller.SetState(GetType(), typeof(HoldemStatePlayFlop)))
                    return;
                break;
            case Games.TexasHoldem<SqlTwitchUser>.Round.Turn:
                if(controller.SetState(GetType(), typeof(HoldemStatePlayTurn)))
                    return;
                break;
            case Games.TexasHoldem<SqlTwitchUser>.Round.River:
                if(controller.SetState(GetType(), typeof(HoldemStatePlayRiver)))
                    return;
                break;
            case Games.TexasHoldem<SqlTwitchUser>.Round.GameOver:
                if(controller.SetState(GetType(), typeof(HoldemStateEndOfGame)))
                    return;
                break;
            default:
                Debug.Fail();
                break;
            }

            if(controller.game.CurrentPlayer != null && controller.game.CurrentPlayer.playerIdObject.id != lastKnownPlayer.playerIdObject.id) {
                Log.debug("Twitch Game: Player changed, up now is: " + controller.game.CurrentPlayer.playerIdObject.name);
                lastKnownPlayer = controller.game.CurrentPlayer;
                PlayerChanged();
            }
        }

        private void PlayerChanged() {
            string chatMessage = "You have ";
            chatMessage += controller.game.CurrentPlayer.hand.ToChatString();
            chatMessage += " and ";
            chatMessage += controller.room.pointManager.ToPointsString(controller.game.CurrentPlayer.bet);
            chatMessage += " on the line so far.  ";
            chatMessage += "Your up - ";
            if(controller.game.canCheck) {
                chatMessage += "!check or !raise ";
            } else {
                chatMessage += CallOrRaiseMessage();
            }

            if(controller.game.table.hand.cards.Count > 0) {
                chatMessage += ". On the table: " + controller.game.table.hand.ToChatString();
            }

            controller.room.SendWhisper(lastKnownPlayer.playerIdObject, chatMessage);
        }

        private string CallOrRaiseMessage() {
            return "!call for " + controller.room.pointManager.ToPointsString(controller.game.GetCallAmount()) + " !raise (amount is incremental to the call amount) or !fold";
        }

        internal override void Open() {
            base.Open();

            foreach(var player in controller.game.seatedPlayers) {
                if(player.playerIdObject.id == controller.game.CurrentPlayer.playerIdObject.id) {
                    lastKnownPlayer = player;
                    PlayerChanged();
                }
            }
        }

    }
}
