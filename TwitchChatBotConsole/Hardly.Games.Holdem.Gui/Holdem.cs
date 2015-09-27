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
        TexasHoldem<int> game;

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

            game.StartGame();
        }

        private void aButtonCheck_Click(object sender, EventArgs e) {

        }

        private void aButtonCall_Click(object sender, EventArgs e) {

        }

        private void aButtonBet_Click(object sender, EventArgs e) {

        }

        private void aButtonRaise_Click(object sender, EventArgs e) {

        }

        private void aButtonFold_Click(object sender, EventArgs e) {

        }
    }
}
