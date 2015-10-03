using System;

namespace Hardly.Library.Hearthstone {
    internal class HearthInternalStartingNewGame : HearthInternalState {
        string myName = null, opponentName = null;
        bool? isMyTurnFirst = null;

        public HearthInternalStartingNewGame(HearthstoneEventObserver eventObserver) : base(eventObserver) {
        }

        internal override HearthInternalState NewLogLine(string line, ref HearthGame game) {
            // My Name: TAG_CHANGE Entity=HardlySober tag=CONTROLLER value=1
            // Opponent Name: TAG_CHANGE Entity=The Innkeeper tag=CONTROLLER value=2
            // Whos Turn First: TAG_CHANGE Entity=HardlySober tag=FIRST_PLAYER value=1

            if(line.EndsWith(" tag=CONTROLLER value=1")) {
                myName = line.GetBetween("Entity=", " tag=");
            } else if(line.EndsWith(" tag=CONTROLLER value=2")) {
                opponentName = line.GetBetween("Entity=", " tag=");
            } else if(line.EndsWith(" tag=FIRST_PLAYER value=1")) {
                string startingPlayerName = line.GetBetween("Entity", " tag=FIRST_PLAYER value=1");
                if(myName != null) {
                    if(myName.Equals(startingPlayerName)) {
                        isMyTurnFirst = true;
                    } else {
                        isMyTurnFirst = false;
                    }
                }
            }

            if(myName != null && opponentName != null && isMyTurnFirst != null) {
                game = new HearthGame(myName, opponentName, isMyTurnFirst.Value);

                return new HearthInternalStateGameInProgress(eventObserver);
            }

            return this;
        }
    }
}