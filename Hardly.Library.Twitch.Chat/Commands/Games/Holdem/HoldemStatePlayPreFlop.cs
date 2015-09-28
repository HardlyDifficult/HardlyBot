namespace Hardly.Library.Twitch {
	public class HoldemStatePlayPreFlop : HoldemStatePlay {
		public HoldemStatePlayPreFlop(TwitchHoldem controller) : base(controller) {
        }

        internal override void Open() {
			base.Open();
		
			foreach(var player in controller.game.seatedPlayers) {
                if(player.playerIdObject.id != controller.game.CurrentPlayer.playerIdObject.id) {
                    string chatMessage = "You have ";
                    chatMessage += player.hand.ToChatString();
                    chatMessage += ".";
                    chatMessage += " Sit tight while others play.  I'll whisper again shortly.";
                    controller.room.SendWhisper(player.playerIdObject, chatMessage);
                }
			}
		}
	}
}