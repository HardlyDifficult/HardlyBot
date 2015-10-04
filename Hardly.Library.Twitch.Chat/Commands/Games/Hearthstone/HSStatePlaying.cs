using Hardly.Library.Hearthstone;

namespace Hardly.Library.Twitch {
    public abstract class HSStatePlaying : GameState<TwitchHearthstone> {
        public HSStatePlaying(TwitchHearthstone controller) : base(controller) {
        }

        internal virtual void NextTurn(DrawCard drawEvent) {
            if(drawEvent.myTurn) {
                controller.room.SendChatMessage("Hearthstone - It's my turn, drew: " + (drawEvent.card.name == null ? "unknown card.." : drawEvent.card.name) + ".");
            }
        }
    }
}
