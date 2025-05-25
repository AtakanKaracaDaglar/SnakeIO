using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeIO
{
    public partial class Form1 : Form
    {

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        bool isStarted = false;

        public Form1()
        {
            InitializeComponent();

            //Sistem Dpi'ına göre yeniden ölçeklendirme yapmasını engellemek için.
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.DoubleBuffered = true;

            this.KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            this.KeyUp += (s, e) => Input.KeyUp(e.KeyCode);
            this.KeyDown += Form1_KeyDown;


            pbCanvas.Paint += updateGraphics;
            gameTimer.Interval = 2000 / Settings.Speed;
            gameTimer.Tick += updateScreen;
            gameTimer.Start();
        }

        public void Form1_KeyDown(object sender, KeyEventArgs k)
        {
            Console.WriteLine("bastığın buton  " + k.KeyCode);
            if (k.KeyCode == Keys.Return)
            {
                
                startGame();
                Console.WriteLine("oyun başladı");
                isStarted = true;
                Settings.GameOver = false;

            }
        }

        public void updateScreen(object sender, EventArgs e)
        {
            

            if (isStarted || Settings.GameOver == false)
            {

                //konumlar yazılacak
                if (Input.IsKeyPressed(Keys.W) && Settings.Directions != Input.Direc.Down)
                {
                    Settings.Directions = Input.Direc.Up;
                }
                else if (Input.IsKeyPressed(Keys.S) && Settings.Directions != Input.Direc.Up)
                {
                    Settings.Directions = Input.Direc.Down;
                }
                else if (Input.IsKeyPressed(Keys.A) && Settings.Directions != Input.Direc.Right)
                {
                    Settings.Directions = Input.Direc.Left;
                }
                else if (Input.IsKeyPressed(Keys.D) && Settings.Directions != Input.Direc.Left)
                {
                    Settings.Directions = Input.Direc.Right;
                }

                movePlayer();

                pbCanvas.Invalidate(); //picturebox'ı yeniden çizmek için

                score_lbl.Text = Settings.Score.ToString();
                Console.WriteLine("Aktif Yön: " + Settings.Directions);
            }
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (Settings.GameOver == false)
            {
                Brush snakeColor;

                //yılan parça kontrolü için
                for(int i = 0; i < Snake.Count; i++)
                {
                    if(i == 0)
                    {
                        snakeColor = Brushes.Red; //kafa
                    }
                    else
                    {
                        snakeColor = Brushes.Green; //vücut
                    }

                    //yılan vücudu çizimi
                    canvas.FillEllipse(snakeColor, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height, Settings.Width, Settings.Height));
                    //yemek çizimi
                    canvas.FillEllipse(Brushes.Yellow, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height));

                }

            }
            else
            {
                //game over yazısı
                string gameOverText = "Game Over\nPress Enter to Restart";
                end_lbl.Text = gameOverText;
                
            }


        }

        private void startGame()
        {
            Snake.Clear();
            Circle head = new Circle() { X = 13, Y = 16 };
            Snake.Add(head);

            Settings.Score = 0;
            Settings.GameOver = false;
            isStarted = true;


            generateFood();
        }

        private void movePlayer()
        {

            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.Directions)
                    {
                        case Input.Direc.Up:
                            Snake[i].Y--;
                            break;
                        case Input.Direc.Down:
                            Snake[i].Y++;
                            break;
                        case Input.Direc.Left:
                            Snake[i].X--;
                            break;
                        case Input.Direc.Right:
                            Snake[i].X++;
                            break;
                    }


                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;


                    //sınırlara değerse oyun biter.
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXpos || Snake[i].Y >= maxYpos)
                    {
                        die();
                    }
                    //vücuduna değerse oyun biter.
                    for(int j=1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }
                    }
                    //yemek yeme durumu
                    if (Snake[0].X ==food.X && Snake[0].Y == food.Y)
                    {
                        eat();
                        Settings.Speed += 50;
                    }

                }
                else
                {
                    //yılanın vücut parçalarını başa göre ayarlama
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

        }

        private void die()
        {
            Settings.GameOver = true;
            isStarted = false;
            
            Retry();
        }

        private void Retry()
        {
            if(Settings.GameOver == true)
            {
                DialogResult result = MessageBox.Show("Game Over", "Game end", MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Retry)
                {
                    Settings.GameOver = false;
                    startGame();

                }
                else if (result == DialogResult.Cancel)
                {
                    this.Close();
                }
            }
        }

        private void eat()
        {

            Circle body = new Circle()
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);
            Settings.Score += Settings.Points;
            generateFood();



        }
        private void generateFood()
        {
            //YEMEK OLUŞTURMA FONK.
            int maxXpos = pbCanvas.Size.Width / Settings.Width; 
            int maxYpos = pbCanvas.Size.Height / Settings.Height;

            Random rnd = new Random();

            food = new Circle()
            {
                X = rnd.Next(0, maxXpos),
                Y = rnd.Next(0, maxYpos)
            };




        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {

            if (Input.pressedKeys.Contains(Keys.Return))
            {
                
                startGame();
                end_lbl.Visible = false;
                movePlayer();
            }
        }
    }
}
