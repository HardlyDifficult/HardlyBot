namespace Hardly.Library.Hearthstone {
    internal class HearthInternalStateGameInProgress : HearthInternalState {
        string currentEntity = null;
        bool firstCardOfTurn = false;

        public HearthInternalStateGameInProgress(HearthstoneEventObserver eventObserver) : base(eventObserver) {
            eventObserver.Observe(new NewGame(eventObserver.currentGame));
        }

        internal override HearthInternalState NewLogLine(string line, ref HearthGame game) {
            // TAG_CHANGE Entity=HardlySober tag=PLAYSTATE value=WON
            if(line.Contains("TAG_CHANGE Entity=" + game.myPlayerName + " tag=PLAYSTATE value=")) {
                if(line.EndsWith("WON")) {
                    return new HearthInternalStateOff(eventObserver, true);
                } else if(line.EndsWith("LOST")) {
                    return new HearthInternalStateOff(eventObserver, false);
                } else if(line.EndsWith("TIED")) {
                    return new HearthInternalStateOff(eventObserver, null);
                }
            }

            // Learning cards
            // 14:43:05.8675879 GameState.DebugPrintOptions() -   option 3 type=POWER mainEntity=[name=Fiery War Axe id=8 zone=HAND zonePos=4 cardId=CS2_106 player=1]
            if(line.Contains("type=POWER mainEntity=[name=")) {
                string cardId = line.GetBetween(" cardId=", " ");
                string cardName = line.GetBetween("mainEntity=[name=", " id=");
                if(cardId != null && cardName != null) {
                    SqlHearthstoneCard card = new SqlHearthstoneCard(cardId, cardName);
                    card.Save(); // TODO switch to lazy save
                }
            }

            // Drawing cards
            // D 14:42:40.6163131 GameState.DebugPrintPower() - ACTION_START Entity=HardlySober BlockType=TRIGGER Index=-1 Target=0
            // D 14:42:40.6163131 GameState.DebugPrintPower() -     SHOW_ENTITY - Updating Entity=[id=8 cardId= type=INVALID zone=DECK zonePos=0 player=1] CardID=CS2_106

            string otherPlayerName = game.myPlayerName.Equals(currentEntity) ? game.opponentPlayerName : game.myPlayerName;
            if(line.Contains("TAG_CHANGE Entity=" + otherPlayerName + " tag=CURRENT_PLAYER value=1")) {
                var entity = line.GetBetween("TAG_CHANGE Entity=", " tag=CURRENT_PLAYER");
                if(entity != null && !entity.Equals("GameEntity")) {
                    if(entity != currentEntity) {
                        currentEntity = entity;
                        firstCardOfTurn = true;
                    }
                }
            }

            if(firstCardOfTurn && currentEntity != null && line.Contains("SHOW_ENTITY - Updating Entity=")) {
                string cardId = line.GetAfter("CardID=");
                if(cardId != null) {
                    bool? myTurn = null;
                    if(currentEntity.Equals(game.myPlayerName)) {
                        myTurn = true;
                    } else if(currentEntity.Equals(game.opponentPlayerName)) {
                        myTurn = false;
                    }

                    if(myTurn != null) {
                        SqlHearthstoneCard card = new SqlHearthstoneCard(cardId);
                        eventObserver.Observe(new DrawCard(eventObserver.currentGame, myTurn.Value, card));
                        firstCardOfTurn = false;
                    }
                }
            }

            ///// TAG_CHANGE Entity=HardlySober tag=CURRENT_PLAYER value=1
            //if(currentEntity == null || line.Contains("TAG_CHANGE Entity=" + currentEntity + " tag=CURRENT_PLAYER value=0")) {
            //    if(almostReadyForNext) {
            //        lookForNewPlayer = true;
            //        almostReadyForNext = false;
            //    } else {
            //        almostReadyForNext = true;
            //    }
            //}
            
            return this;
        }
    }
}