using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yahtzee_new
{
    public class DiceSet
    {
        public PictureBox[] dicePictureBoxes;
        private static Random random = new Random();
        private readonly int[] diceValues;
        public int[] diceResults => (int[])diceValues.Clone();
        private bool[] isSelected;

        public DiceSet(PictureBox[] pictureBoxes)
        {
            dicePictureBoxes = pictureBoxes;
            random = new Random();
            isSelected = new bool[5];
            diceValues = new int[5];
            InitializeDicePictureBoxes();
        }

        private void DicePictures(int index)
        {
            int diceValue = diceValues[index];
            string resourcePath = $"yahtzee_new.pictures.dice{diceValue}.png";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    dicePictureBoxes[index].Image = Image.FromStream(stream);
                }
            }
        }

        private void InitializeDicePictureBoxes()
        {
            for (int i = 0; i < dicePictureBoxes.Length; i++)
            {
                int index = i;
                dicePictureBoxes[i].Click += (sender, e) => ToggleDiceSelection(index);
            }
        }

        public void ToggleDiceSelection(int index)
        {
            if (index < 0 || index >= dicePictureBoxes.Length)
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid die index.");

            isSelected[index] = !isSelected[index];
            dicePictureBoxes[index].BorderStyle = isSelected[index] ? BorderStyle.Fixed3D : BorderStyle.None;
        }

        public void ClearDiceSelection()
        {
            for (int i = 0; i < isSelected.Length; i++)
            {
                isSelected[i] = false;
                dicePictureBoxes[i].BorderStyle = BorderStyle.None; // Reset display to default
            }
        }

        public void ResetDice()
        {
            for (int i = 0; i < dicePictureBoxes.Length; i++)
            {
                diceValues[i] = 6; // Set each die value to 6
                isSelected[i] = false; // No selection
                dicePictureBoxes[i].BorderStyle = BorderStyle.None; // Reset border style
                string resourcePath = $"yahtzee_new.pictures.dice{diceValues[i]}.png"; // Embedded resource path
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    if (stream != null)
                    {
                        dicePictureBoxes[i].Image = Image.FromStream(stream); // Load image from resource
                    }
                }
            }
        }

        public void FirstRoll()
        {
            for (int i = 0; i < dicePictureBoxes.Length; i++)
            {
                dicePictureBoxes[i].BorderStyle = BorderStyle.None; // Reset the border style to default
                diceValues[i] = random.Next(1, 7); // Roll a number between 1 and 6
                DicePictures(i); // Update the die image
                isSelected[i] = false; // No dice are selected after the first roll
            }
        }

        public void NotFirstRoll()
        {
            for (int i = 0; i < dicePictureBoxes.Length; i++)
            {
                if (isSelected[i]) // Only roll the selected dice
                {
                    diceValues[i] = random.Next(1, 7); // Roll a number between 1 and 6
                    DicePictures(i); // Update the die image
                }
            }
        }

    }
}
