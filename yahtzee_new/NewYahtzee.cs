using System.Drawing.Text;

namespace yahtzee_new
{
    public partial class NewYahtzee : Form
    {
        private PictureBox[] dicePictureBoxes;
        private DiceSet diceSet;
        private ScoreCalculator scoreCalculator;
        private Round? round;
        private dgvGamePlay dataGridView;
        private FullGameMng? gameManager;
        private Help help;

        public NewYahtzee()
        {
            InitializeComponent();

            this.Load += NewYahtzee_Load;

            help = new Help();

            dicePictureBoxes = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };

            dataGridView = new dgvGamePlay();

            if (dataGridView == null)
            {
                throw new InvalidOperationException("DataGridView initialization failed.");
            }
            this.Controls.Add(dataGridView);
            dataGridView.Size = new Size(500, 300);
            dataGridView.Location = new Point(10, 100);

            diceSet = new DiceSet(dicePictureBoxes);

        }

        private void NewYahtzee_Load(object? sender, EventArgs e)
        {
            try
            {
                gameManager = new FullGameMng(dataGridView, btnRoll, btnNextRound, dicePictureBoxes);
                gameManager.StartGame(this.ClientSize);
                scoreCalculator = new ScoreCalculator(gameManager.GetCurrentPlayer());
                round = new Round(diceSet, dataGridView, btnRoll, gameManager);

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            help.ShowScoringHelpDialog();
        }
    }
}
