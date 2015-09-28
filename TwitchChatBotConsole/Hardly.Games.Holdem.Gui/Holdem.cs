using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hardly.Games.Holdem.Gui {
    public partial class Holdem : Form {
        TexasHoldem<int> game = new TexasHoldem<int>();

        public Holdem() {
            InitializeComponent();
        }

        private void aButtonStartGame_Click(object sender, EventArgs e) {
            game.Reset();
            game.SetBigBlind((ulong)aNumberBigBlind.Value);

            for(int i = 0; i < aNumberPlayers.Value; i++) {
                var pointManager = new PointManager();
                pointManager.TotalPointsInAccount = 100000;
                game.Join(i, pointManager);
            }

            if(game.CanStart()) {
                game.StartGame();
            }
        }

        private void aButtonCheck_Click(object sender, EventArgs e) {
            game.Check();
        }

        private void aButtonCall_Click(object sender, EventArgs e) {

        }

        private void aButtonBet_Click(object sender, EventArgs e) {
            game.Bet((ulong)aNumberBetOrRaiseAmount.Value);
        }

        private void aButtonRaise_Click(object sender, EventArgs e) {
            game.Raise((ulong)aNumberBetOrRaiseAmount.Value);
        }

        private void aButtonFold_Click(object sender, EventArgs e) {

        }

        private void aTimerRefresh_Tick(object sender, EventArgs e) {
            var player = game.CurrentPlayer;
            if(player != null) {
                aLabelCurrentPlayer.Text = player.playerIdObject.ToString();
                aLabelPlayerHand.Text = player.hand.ToChatString();
                aLabelBoardCards.Text = game.table.hand.ToChatString();
            } else {
                aLabelCurrentPlayer.Text = "";
                aLabelPlayerHand.Text = "";
                aLabelBoardCards.Text = "";
            }

            string winners = null;
            if(game.lastGameWinners != null) {
                foreach(var winner in game.lastGameWinners) {
                    if(winners == null) {
                        winners = "";
                    } else {
                        winners += ", ";
                    }
                    winners += winner.playerIdObject.ToString();
                }
            }
            aLabelWinners.Text = winners;

            string losers = null;
            if(game.lastGameLosers != null) {
                foreach(var loser in game.lastGameLosers) {
                    if(losers == null) {
                        losers = "";
                    } else {
                        losers += ", ";
                    }
                    losers += loser.playerIdObject.ToString();
                }
            }
            aLabelLosers.Text = losers;

            aLabelPot.Text = game.GetTotalPot().ToString();
            aLabelCallAmount.Text = game.GetCallAmount().ToString();
        }
    }
}
