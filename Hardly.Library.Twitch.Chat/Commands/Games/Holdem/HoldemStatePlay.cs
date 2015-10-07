using System;
using Hardly.Games;

namespace Hardly.Library.Twitch {
    public abstract class HoldemStatePlay : GameState<TwitchHoldem> {
        TimerSet timer;
        TexasHoldemPlayer<TwitchUser> lastKnownPlayer = null;
        List<TwitchUser> playersLookingToFold = new List<TwitchUser>();

        public HoldemStatePlay(TwitchHoldem controller) : base(controller) {
            AddCommand(controller.room, "call", CallCommand, "Call...", null, false, null, false);
            AddCommand(controller.room, "raise", RaiseCommand, "Raise...", null, false, null, false);
            AddCommand(controller.room, "check", CheckCommand, "Check...", null, false, null, false);
            AddCommand(controller.room, "fold", FoldCommand, "Fold...", null, false, null, false);
            AddCommand(controller.room, "bet", BetCommand, "Bet...", null, false, null, false);
            AddCommand(controller.room, "allin", AllInCommand, "Bet or Raise all in...", null, false, null, false);

            timer = new TimerSet(new TimeSpan[] { TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30) }, new Action[] { WarningTimer, WarningTimer, FinalTimer });

            lastKnownPlayer = controller.game.currentPlayer;
            PlayerChanged();
            timer.Start();
            playersLookingToFold.Clear();
        }


        private void FinalTimer() {
            if(lastKnownPlayer.idObject.Equals(controller.game.currentPlayer.idObject)) {
                Log.debug("Holdem: Skipping turn for " + controller.game.currentPlayer.idObject.name);
                var player = controller.game.currentPlayer;
                if(!controller.game.Check()) {
                    controller.game.Fold();
                    controller.room.SendWhisper(player.idObject, "Time's up... I FOLDED for you.  You're welcome ;)");
                } else {
                    controller.room.SendWhisper(player.idObject, "Time's up... I checked for you.");
                }

                CheckForNextState();
            }
        }

        private void WarningTimer() {
            if(lastKnownPlayer.idObject.Equals(controller.game.currentPlayer.idObject)) {
                Log.debug("Holdem: Warning for " + controller.game.currentPlayer.idObject.name);
                PlayerChanged(true);
            }
        }

        private void AllInCommand(TwitchUser speaker, string arg2) {
            if(MyTurn(speaker)) {
                if(controller.game.canBet) {
                    ulong amount = controller.game.Bet(ulong.MaxValue);
                    if(amount > 0) {
                        Success(speaker, "You bet " + controller.room.pointManager.ToPointsString(amount) + ".");
                    } else {
                        FailedAction(speaker, "allin");
                    }
                } else if(controller.game.canRaise) {
                    ulong amount = controller.game.Raise(ulong.MaxValue);
                    if(amount > 0) {
                        Success(speaker, "You rose " + controller.room.pointManager.ToPointsString(amount) + ".");
                    } else {
                        FailedAction(speaker, "allin");
                    }
                } else {
                    FailedAction(speaker, "allin");
                }
            }

            CheckForNextState();
        }

        private void BetCommand(TwitchUser speaker, string additionalText) {
            if(MyTurn(speaker)) {
                ulong amount = controller.room.pointManager.GetPointsFromString(additionalText);
                amount = controller.game.Bet(amount);
                if(amount > 0) {
                    Success(speaker, "You bet " + controller.room.pointManager.ToPointsString(amount) + ".");
                } else {
                    FailedAction(speaker, "bet");
                }
            }

            CheckForNextState();
        }

        private bool MyTurn(TwitchUser speaker) {
            if(SpeakerInGame(speaker)) {
                if(speaker.id.Equals(controller.game.currentPlayer.idObject.id)) {
                    return true;
                } else {
                    controller.room.SendWhisper(speaker, "Not your turn, hold on...");
                    return false;
                }
            }

            return false;
        }
        
        private void FoldCommand(TwitchUser speaker, string arg2) {
            if(SpeakerInGame(speaker)) {
                if(speaker.id.Equals(controller.game.currentPlayer.idObject.id)) {
                    List<PlayingCard> cards = controller.game.currentPlayer.hand;
                    if(controller.game.Fold()) {
                        if(arg2 != null && arg2.ToLower().Contains("show")) {
                            controller.room.SendChatMessage("Holdem: " + speaker.name + " is out, had " + cards.ToString());
                        } else {
                            Success(speaker, "Later dude.");
                        }
                    } else {
                        if(controller.game.canCheck) {
                            CheckCommand(speaker, arg2);
                        } else {
                            FailedAction(speaker, "fold");
                        }
                    }
                } else {
                    playersLookingToFold.Add(speaker);
                    Success(speaker, "Will check or fold as soon as your turn is up.");
                }
            } 

            CheckForNextState();
        }

        private bool SpeakerInGame(TwitchUser speaker) {
            bool inGame = false;
            foreach(var player in controller.game.GetPlayers()) {
                if(speaker.id.Equals(player.idObject.id)) {
                    inGame = true;
                }
            }

            if(!inGame) {
                controller.room.SendWhisper(speaker, "You're not in this game, wait for the next...");
            }

            return inGame;
        }

        private void CheckCommand(TwitchUser speaker, string arg2) {
            if(MyTurn(speaker)) {
                if(controller.game.Check()) {
                    controller.room.SendWhisper(speaker, "Check check.");
                } else {
                    FailedAction(speaker, "check");
                }
            }

            CheckForNextState();
        }

        private void RaiseCommand(TwitchUser speaker, string additionalText) {
            if(MyTurn(speaker)) {
                ulong callAmount = controller.game.GetCallAmount();
                ulong amount = controller.room.pointManager.GetPointsFromString(additionalText);
                amount = controller.game.Raise(amount);
                if(amount > 0) {
                    Success(speaker, "You rose " + controller.room.pointManager.ToPointsString(amount - callAmount));
                } else {
                    if(controller.game.canBet) {
                        BetCommand(speaker, additionalText);
                    } else {
                        FailedAction(speaker, "raise");
                    }
                }
            } 

            CheckForNextState();
        }

        private void CallCommand(TwitchUser speaker, string arg2) {
            if(MyTurn(speaker)) {
                ulong amount = controller.game.GetCallAmount();
                amount = controller.game.Call();
                if(amount > 0) {
                    if(controller.game.sidepotPlayers.Contains(controller.game.currentPlayer)) {
                        Success(speaker, "Called & went all in");
                    } else {
                        Success(speaker, "Called for " + controller.room.pointManager.ToPointsString(amount));
                    }
                } else {
                    if(controller.game.canCheck) {
                        CheckCommand(speaker, arg2);
                    } else {
                        FailedAction(speaker, "call");
                    }
                }
            }

            CheckForNextState();
        }

        private void Success(TwitchUser speaker, string message) {
            controller.room.SendWhisper(speaker, message);
        }

        private void CheckForNextState() {
            Log.debug("Twitch Game: checking round " + controller.game.round.ToString());

            switch(controller.game.round) {
            case Games.TexasHoldem<TwitchUser>.Round.PreFlop:
                // Do nothing
                break;
            case Games.TexasHoldem<TwitchUser>.Round.Flop:
                if(controller.SetState(this, typeof(HoldemStatePlayFlop))) {
                    timer.Stop();
                    return;
                }
                break;
            case Games.TexasHoldem<TwitchUser>.Round.Turn:
                if(controller.SetState(this, typeof(HoldemStatePlayTurn))) {
                    timer.Stop();
                    return;
                }
                break;
            case Games.TexasHoldem<TwitchUser>.Round.River:
                if(controller.SetState(this, typeof(HoldemStatePlayRiver))) {
                    timer.Stop();
                    return;
                }
                break;
            case Games.TexasHoldem<TwitchUser>.Round.GameOver:
                if(controller.SetState(this, typeof(HoldemStateEndOfGame))) {
                    timer.Stop();
                    return;
                }
                break;
            default:
                Debug.Fail();
                break;
            }

            if(controller.game.currentPlayer != null && controller.game.currentPlayer.idObject.id != lastKnownPlayer?.idObject.id) {
                Log.debug("Twitch Game: Player changed, up now is: " + controller.game.currentPlayer.idObject.name);
                lastKnownPlayer = controller.game.currentPlayer;
                PlayerChanged();
                timer.Start();
            }
        }

        private void PlayerChanged(bool warning = false) {
            if(playersLookingToFold.Contains(controller.game.currentPlayer.idObject)) {
                FoldCommand(controller.game.currentPlayer.idObject, null);
            } else {
                string chatMessage = "";
                if(warning) {
                    chatMessage = "Get Moving! ";
                }
                chatMessage += "You have ";
                chatMessage += controller.game.currentPlayer.hand.ToString();
                chatMessage += " and ";
                chatMessage += controller.room.pointManager.ToPointsString(controller.game.currentPlayer.bet);
                chatMessage += " on the line so far.  ";
                chatMessage += "Your up - ";
                chatMessage += AvailableCommands();

                if(controller.game.tableCards.Count > 0) {
                    chatMessage += "-- On the table: " + controller.game.tableCards.ToString();
                }

                controller.room.SendWhisper(lastKnownPlayer.idObject, chatMessage);
            }
        }

        private string AvailableCommands() {
            string chatMessage = "";
            if(controller.game.canCheck) {
                chatMessage += "!check ";
            }
            if(controller.game.canBet) {
                chatMessage += "!bet ";
            }
            if(controller.game.canCall) {
                chatMessage += "!call " + controller.room.pointManager.ToPointsString(controller.game.GetCallAmount());
            }
            if(controller.game.canRaise) {
                chatMessage += "!raise ";
            }
            if(controller.game.canFold) {
                chatMessage += "!fold ";
            }

            return chatMessage;
        }

        private void FailedAction(TwitchUser speaker, string youTriedTo) {
            controller.room.SendWhisper(speaker, "Can't !" + youTriedTo + " right now -- you can " + AvailableCommands());
            timer.Start();
        }

        protected string OnTheTable() {
            ulong pot = controller.game.GetTotalPot();
            if(pot > 0) {
                return " " + controller.room.pointManager.ToPointsString(pot) + " in the pot";
            }

            return "";
        }
    }
}
