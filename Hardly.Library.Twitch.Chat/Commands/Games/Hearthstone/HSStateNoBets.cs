using System;

namespace Hardly.Library.Twitch {
    public class HSStateNoBets : HSStatePlaying {
        public HSStateNoBets(TwitchHearthstone controller) : base(controller) {
        }

        internal override void Open() {
        }
    }
}
