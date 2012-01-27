using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MemoryTestWF
{
    public partial class MemoryTestForm : Form
    {
        /// <summary>
        /// Inicializes a new instance of the MemoryTest.MemoryTestForm class.
        /// </summary>
        public MemoryTestForm()
        {
            InitializeComponent();
            buttons = new Button[] { button0, button1, button2, button3, button4 };
        }

        private Button[] buttons;//Array for 
        RandomNumbers randNums = new RandomNumbers();
        List<int> randNumsList = new List<int>(5);//The list with random numbers.
        List<int> userNumsList = new List<int>(5);//The list with random numbers entered by user.
        RandomLocation ranLoc = new RandomLocation();
        List<List<int>> randomLocation = new List<List<int>>(5);//The list with random X and Y coordinates of location of five elements.

        /// <summary>
        /// Event handler for buttonStart click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            numericUpDown.Enabled = false;
            buttonAbort.Enabled = true;

            textBox1.BackColor = this.BackColor;
            textBox1.Text = "";

            randomLocation = ranLoc.GetRandomArray();
            randNumsList = randNums.GetRandomNums();//Fills a list of numbers.

            //Displays random numbers on the buttons.
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].ForeColor = Color.Black;
                buttons[i].Text = Convert.ToString(randNumsList[i]);
                buttons[i].Location = new Point(randomLocation[i][0], randomLocation[i][1]);
                buttons[i].Visible = true;
            }

            Thread myThread = new Thread(MyThread);
            myThread.Start();

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Enabled = true;
            }
        }

        /// <summary>
        /// Event handler for buttons click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void buttonsClick(object sender, EventArgs e)
        {
            //Addition the numbers entered by user to the list.
            userNumsList.Add(Convert.ToInt32(char.GetNumericValue((sender.ToString().Last()))));

            //Verifies the correctness of sequence of numbers entered by user.
            if (userNumsList.Count == 5)
            {
                if (randNums.Equals(userNumsList))
                {
                    textBox1.BackColor = Color.LimeGreen;
                    textBox1.Text = "RIGHT";
                }
                else
                {
                    textBox1.BackColor = Color.Tomato;
                    textBox1.Text = "WRONG";
                }

                Reset();
            }
        }

        /// <summary>
        /// Event handler for buttonAbort click
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Parallel thread, performs a delay, after which hides the numbers from the user.
        /// </summary>
        public void MyThread()
        {
            Thread.Sleep((int)numericUpDown.Value);
            if (buttons[4].Text != "")
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].ForeColor = buttons[i].BackColor;
                }
            }
        }

        /// <summary>
        /// Sets controls in the start position.
        /// </summary>
        private void Reset()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Enabled = false;
            }
            buttonStart.Enabled = true;
            numericUpDown.Enabled = true;
            buttonAbort.Enabled = false;
            userNumsList.Clear();
            randomLocation.Clear();
            randNumsList.Clear();
        }
    }

    /// <summary>
    /// Generate a list with random numbers.
    /// </summary>
    class RandomNumbers
    {
        private Random gen = new Random();
        List<int> randomNumbersList = new List<int>(5);

        /// <summary>
        /// Generate a list with random numbers.
        /// </summary>
        /// <returns>List with random numbers.</returns>
        public List<int> GetRandomNums()
        {
            randomNumbersList.Clear();

            while (randomNumbersList.Count < 5)
            {
                int randomNumber = gen.Next(1, 9);

                if (!randomNumbersList.Contains(randomNumber))
                {
                    randomNumbersList.Add(randomNumber);
                }
            }

            return randomNumbersList;
        }

        /// <summary>
        /// Returns the value indication whether this instance is equal to a specified List.
        /// </summary>
        /// <param name="compList">List with int32 numbers.</param>
        /// <returns>true if lists are equivalent.</returns>
        public bool Equals(List<int> compList)
        {
            randomNumbersList.Sort();
            for (int i = 0; i < 5; i++)
            {
                if (randomNumbersList[i] != compList[i])
                {
                    return false;
                }
            }

            return true;

        }
    }

    /// <summary>
    /// Generate the list with random X and Y coordinates of location of five elements.
    /// </summary>
    class RandomLocation
    {
        private Random gen = new Random();
        private List<int> randomList;
        private List<List<int>> randomNumList = new List<List<int>>(5);
        private int randomNumber;

        /// <summary>
        /// Generate the list with random X and Y coordinates of location of five elements.
        /// </summary>
        /// <returns>List with rundom X and Y coordinates of location of five elements.</returns>
        public List<List<int>> GetRandomArray()
        {
            while (randomNumList.Count < 5)
            {
                randomList = new List<int>(2);

                randomNumber = gen.Next(0, 12);
                randomList.Add(randomNumber * 30);

                randomNumber = gen.Next(3, 9);
                randomList.Add(randomNumber * 30);

                if (Check(randomList))
                {
                    randomNumList.Add(randomList);
                }
            }

            return randomNumList;
        }

        /// <summary>
        /// Check for repeats in the list.
        /// </summary>
        /// <param name="ranList">List with int32 numbers.</param>
        /// <returns>True if the list is exclusive.</returns>
        private bool Check(List<int> ranList)
        {
            foreach (List<int> i in randomNumList)
            {
                if ((i[0] == ranList[0]) & (i[1] == ranList[1])) return false;
            }
            return true;
        }
    }
}
