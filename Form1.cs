using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        List<int> Numbers = new List<int>() { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
        List<int> Numbers2 = new List<int>() { 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12 };
        string FirstChoice;
        string SecondChoice;
        int Tries;
        List<PictureBox> PictureBoxes ;
            
        PictureBox PicA;
        PictureBox PicB;
        int SeccendTime = 60;
        int MinuteTime;
        int CountDownTime;
        short Score = 0;
        bool GameOver = false;
        string Pictures;

        public Form1()
        {
            InitializeComponent();

            PictureBoxes = new List<PictureBox>() 
            { pb1, pb2, pb3,pb4, pb5, pb6, pb7, pb8, pb9, pb10, pb11, pb12 };

            RestartGame();

        }

        void CheckSpeed()
        {
            switch (cbSpeed.Text)
            {
                case "1":
                    GameTimer.Interval = 800;
                    break;

                case "1.5":
                    GameTimer.Interval = 300;
                    break;

                case "2":
                    GameTimer.Interval = 100;
                    break;
            }
        }
        void CheckSeccend()
        {
            if (CountDownTime < 10)
            {
                CountDownTime--;
                lblSeccend.Text = "0" + CountDownTime.ToString();
            }
            else
            {
                CountDownTime--;
                lblSeccend.Text = CountDownTime.ToString();
            }
        }

        void IsTimeUp()
        {
            if (CountDownTime < 1)
            {
                if (MinuteTime < 1)
                {
                    lblSeccend.ForeColor = Color.Red;
                    lblMinute.ForeColor = Color.Red;
                    lblComa.ForeColor = Color.Red;

                    MakeGameOver("Times Up", "You Lose");
                    foreach (PictureBox x in PictureBoxes)
                    {
                        if (x.Tag != null)
                        {
                            x.Image = Image.FromFile("D:\\MemoryPics/" + (string)x.Tag + ".png");

                        }
                    }
                }

                else
                {
                    MinuteTime--;
                    lblMinute.Text = "0" + MinuteTime;
                    CountDownTime = 60;
                    lblSeccend.Text = CountDownTime.ToString();
                }
            }
        }

        void Timer()
        {

            CheckSpeed();

            CheckSeccend();

            IsTimeUp();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            Timer();
        }

        void ShowPictures()
        {

            foreach (PictureBox x in PictureBoxes)
            {
                x.Image = Image.FromFile("MemoryPics/" + (string)x.Tag + ".png");
            }
        }
        void HidePictures()
        {
            foreach (PictureBox x in PictureBoxes)
            {
                x.Image = null;
            }
        }

        void CheckScore()
        {

            Score++;
            ProbScore.Value += 17;
            ProbScore.Refresh();

           lblScore.Text = Score.ToString();
           lblScore.Refresh();
        }

        void MakeGameOver(string Message, string Title)
        {
            GameTimer.Stop();
            GameOver = true;
            MessageBox.Show(Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SelectPic_MouseLeave(object sender, EventArgs e)
        {
            if (GameOver)
            {
                return;
            }

            if (FirstChoice != null && SecondChoice != null)
            {
                CheckPictures(PicA, PicB);
            }


        }

        private void SelectPic_Click(object sender, EventArgs e)
        {
            if (GameOver)
            {
                return;
            }

            if (FirstChoice == null)
            {
                PicA = sender as PictureBox;
                if (PicA.Tag != null && PicA.Image == null)
                {
                    PicA.Image = Image.FromFile("MemoryPics/" + (string)PicA.Tag + ".png");
                    FirstChoice = (string)PicA.Tag;
                }

                return;
            }
            if (SecondChoice == null)
            {
                PicB = sender as PictureBox;
                if (PicB.Tag != null && PicB.Image == null)
                {
                    PicB.Image = Image.FromFile("MemoryPics/" + (string)PicB.Tag + ".png");
                    SecondChoice = (string)PicB.Tag;
                }

                return;
            }

        }

        void CheckPictures(PictureBox A, PictureBox B)
        {
            if (FirstChoice == SecondChoice)
            {
                A.Tag = null;
                B.Tag = null;

                CheckScore();
            }

            Tries++;
            lblMoves.Text = Tries.ToString();

            FirstChoice = null;
            SecondChoice = null;

            if (PictureBoxes.All(o => o.Tag == null))
            {
                GameTimer.Stop();
                MakeGameOver("Great Work", "You Win");
                return;
            }

            foreach (PictureBox pics in PictureBoxes.ToList())
            {
                if (pics.Tag != null)
                {
                    pics.Image = null;
                }
            }


        }

        void ResetPictures()
        {
            if (rbPictures1.Checked == true)
            {
                var randomList = Numbers.OrderBy(x => Guid.NewGuid()).ToList();
                Numbers = randomList;
                for (int i = 0; i < PictureBoxes.Count; i++)
                {
                    PictureBoxes[i].Enabled = false;
                    PictureBoxes[i].Image = null;
                    PictureBoxes[i].Tag = Numbers[i].ToString();
                }
            }

            else
            {
                var randomList = Numbers2.OrderBy(x => Guid.NewGuid()).ToList();
                Numbers2 = randomList;
                for (int i = 0; i < PictureBoxes.Count; i++)
                {
                    PictureBoxes[i].Enabled = false;
                    PictureBoxes[i].Image = null;
                    PictureBoxes[i].Tag = Numbers2[i].ToString();
                }
            }
        }
        void ResetLables()
        {
            lblMoves.Text = Tries.ToString();
            lblMinute.Text = "0" + MinuteTime.ToString();
            lblMinute.ForeColor = Color.White;
            lblSeccend.Text = "0" + CountDownTime.ToString();
            lblSeccend.ForeColor = Color.White;
            lblComa.ForeColor = Color.White;
        }
        void ResetScore()
        {
            ProbScore.Value = 0;
            ProbScore.Minimum = 0;
            ProbScore.Maximum = 100;
            lblScore.Text = "0";
            Score = 0;
        }

        void RestartGame()
        {

            ResetPictures();

            GameTimer.Stop();
            Tries = 0;
            cbTimer.SelectedIndex = 0;
            cbSpeed.SelectedIndex = 0;
            MinuteTime = 0;        
            GameOver = false;
            CountDownTime = 0;

            ResetLables();

            btnStart.Enabled = true;
            btnShowAgain.Enabled = true;

            rbPictures1.Enabled = true;
            rbPictures2.Enabled = true;

            ResetScore();

        }

        void GetCheckTimer()
        {
            MinuteTime = Convert.ToInt32(cbTimer.Text) - 1;
            lblMinute.Text = "0" + MinuteTime;
            CountDownTime = 60;
            lblSeccend.Text = CountDownTime.ToString();
        }

        void StartGame()
        {
            foreach (PictureBox x in PictureBoxes)
            {
                x.Enabled = true;
            }

            GameTimer.Start();

            GetCheckTimer();

            btnStart.Enabled = false;
            btnShowAgain.Enabled = false;

            rbPictures1.Enabled = false;
            rbPictures2.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void btnShowAgain_Click(object sender, EventArgs e)
        {
            ShowPictures();
        }
        private void btnShowAgain_MouseLeave(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            HidePictures();
        }

        void changeColor(Guna2PictureBox pb)
        {
            if(TSBackGround.Checked == true)
            {

                pb.BackColor = Color.SandyBrown; 
                pb.FillColor = Color.SandyBrown;
            }

            else
            {
                

                pb.BackColor = Color.Black;
                pb.FillColor = Color.Black;
            }
        }

        void ResetPictureBoxes()
        {
                changeColor(pb1);
                changeColor(pb2);
                changeColor(pb3);
                changeColor(pb4);
                changeColor(pb5);
                changeColor(pb6);
                changeColor(pb7);
                changeColor(pb8);
                changeColor(pb9);
                changeColor(pb10);
                changeColor(pb11);
                changeColor(pb12);
        }

        void SwitchBackGround()
        {
            if (TSBackGround.Checked == true)
            {
                this.BackColor = Color.SandyBrown;
                Panel1.FillColor = Color.SandyBrown;
                Panel1.BackColor = Color.SandyBrown;

                gpGameDetails.BackColor = Color.SandyBrown;

                ResetPictureBoxes();
            }

            else
            {
                this.BackColor = Color.Black;
                Panel1.FillColor = Color.Black;
                Panel1.BackColor = Color.DimGray;

                gpGameDetails.BackColor = Color.FromArgb(255,64,64,64);

                ResetPictureBoxes();
            }
        }

        private void TSBackGround_CheckedChanged(object sender, EventArgs e)
        {
            SwitchBackGround();
        }

        private void rbPictures1_CheckedChanged(object sender, EventArgs e)
        {
            var randomList = Numbers.OrderBy(x => Guid.NewGuid()).ToList();
            Numbers = randomList;
            for (int i = 0; i < PictureBoxes.Count; i++)
            {
                PictureBoxes[i].Image = null;
                PictureBoxes[i].Tag = Numbers[i].ToString();
            }
        }

        private void rbPictures2_CheckedChanged(object sender, EventArgs e)
        {
            var randomList = Numbers2.OrderBy(x => Guid.NewGuid()).ToList();
            Numbers2 = randomList;
            for (int i = 0; i < PictureBoxes.Count; i++)
            {
                PictureBoxes[i].Image = null;
                PictureBoxes[i].Tag = Numbers2[i].ToString();
            }
        }

     
    }
}
