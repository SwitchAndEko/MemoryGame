/*
 * Memory Flip Card Games
 * Author:Switch_Squirrel
 * Date:05/13/2018
 * The Copyright of the cards' picture is belong to KADOKAWA Animation(No Game No Life Zero-Japan Bonus Gift).
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MemoryGame.Properties;
using System.Threading;
using System.Timers;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        Card[] cards = new Card[16]; //卡牌
        int finishedCount = 8; //配對數，歸零時遊戲結束
        bool Started = false; //遊戲是否開始
        int FlipedID = -1; //場上翻開的卡為單數時，被翻開的落單的卡牌編號
        int FlipedDirection = -1; //場上翻開的卡為單數時，被翻開的落單的卡牌位置
        System.Timers.Timer GameTimer = new System.Timers.Timer(); //計時器
        //計時器採手工原因：可將計時器分離主執行緒

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbl_Time.Text = "時間：";
            btn_Start.Text = "開始";
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 30;
            GameTimer.Elapsed += new ElapsedEventHandler(GameTimer_Tick); //計時器委派
        }

        private void shuffle()
        {
            flowLayoutPanel1.Controls.Clear(); //清空版面
            bool[] used = new bool[16]; //檢查卡牌是否已排入版面
            for (int i = 0; i < 16; i++)
            {
                Random r = new Random();
                int rd = r.Next(0, 8);
                while (used[rd] && used[rd + 8])
                    rd = r.Next(0, 8);
                rd++;

                cards[i] = new Card();
                cards[i].SetRealImage(new Bitmap((Bitmap)Resource1.ResourceManager.GetObject($"_{rd}")));
                rd--;

                if (used[rd])
                    rd += 8;
                used[rd] = true;
                if (rd > 7) rd -= 8;
                cards[i].SetID(rd);
                cards[i].SetDirection(i);
                cards[i].BackgroundImage = cards[i].GetCoverImage();
                cards[i].BackgroundImageLayout = ImageLayout.Stretch;
                cards[i].Height = 120;
                cards[i].Width = 120;
                cards[i].Click += Card_click; //Card類別本身繼承Button類別，加入Button委派
                flowLayoutPanel1.Controls.Add(cards[i]);
            }
        }

        private void Card_click(object sender, EventArgs e)
        {
            if (Started)
            {
                Card c = (Card)sender;
                c.BackgroundImage = c.GetRealImage(); //翻牌顯示圖像
                this.Invalidate();
                this.Update();
                this.Refresh();
                // Line84~Line87 立即刷新畫面
                int ThisID = c.GetID();
                int ThisDirection = c.GetDirection();
                if (FlipedID != -1) //若雙數張
                {
                    if (FlipedID == ThisID && FlipedDirection != ThisDirection) //若翻開兩張相同
                    {
                        finishedCount--;
                        cards[FlipedDirection].Enabled = false;
                        cards[ThisDirection].Enabled = false;
                    }
                    else //若編號不相同 or 同一張
                    {
                        if (FlipedID != ThisID) //若不相同，顯示0.5秒
                            Thread.Sleep(500);
                        cards[FlipedDirection].BackgroundImage = c.GetCoverImage();
                        cards[ThisDirection].BackgroundImage = c.GetCoverImage();
                    }
                    FlipedID = -1;
                    FlipedDirection = -1;
                }
                else //若單數張
                {
                    FlipedID = ThisID;
                    FlipedDirection = ThisDirection;
                }
            }
            if (finishedCount == 0)
            {
                Started = false;
                GameTimer.Enabled = false;
                MessageBox.Show("恭喜！您完成了所有配對！", "恭喜！");
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            shuffle();
            finishedCount = 8;
            Started = true;
            GameTimer.Interval = 1000;
            GameTimer.Enabled = true;
            progressBar1.Value = progressBar1.Maximum;
            for (int i = 0; i < 16; i++)
                cards[i].BackgroundImage = cards[i].GetCoverImage();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            progressBar1.Invoke((MethodInvoker) delegate //因將計時器移至另外的執行緒，不可直接操作UI，需使用委派
            {
                if (progressBar1.Value != 0)
                    progressBar1.Value--;
                else
                {
                    GameTimer.Enabled = false;
                    Started = false;
                    FlipedID = -1;
                    FlipedDirection = -1;
                    MessageBox.Show("時間到！", "失敗！");
                }
            });
        }
    }
}
