using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using WpfKey = System.Windows.Input.Key;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        private int snakeSize = 20;
        private int snakeLength = 4;
        private int score = 0;
        private int foodX = 40;
        private int foodY = 40;

        private int dirX = 0;
        private int dirY = 0;
        private TimeSpan snakeSpeed = new TimeSpan(2000000);
        private bool isGameStarted = false;
        DispatcherTimer timer = new DispatcherTimer();
        private List<int> posX = new List<int>(5);
        private List<int> posY = new List<int>(5);
        private int lastX;
        private int lastY;
        private int currX;
        private int currY;
        private Random randomNum = new Random();
        private int maxRandomNum = 500;
        private int headSnakeX;
        private int headSnakeY;

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = snakeSpeed;
            timer.Start();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            posX.Add(140);
            posY.Add(120);
            posX.Add(120);
            posY.Add(120);
            posX.Add(100);
            posY.Add(120);
            posX.Add(100);
            posY.Add(100);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            snakeAlgorithm();
        }

        private void snakeAlgorithm()
        {
            for (int i = snakeLength - 1; i >= 0; i--)
            {
                if (isGameStarted)
                {
                    updateDashBoard("SCORE: " + Convert.ToString(score));
                    if (i == snakeLength - 1)
                    {
                        lastX = posX[i];
                        lastY = posY[i];

                        if (posX[i] == foodX && posY[i] == foodY)
                        {
                            foodX = randomNum.Next(maxRandomNum / 20) * 20;
                            foodY = randomNum.Next(maxRandomNum / 20) * 20;
                            snakeLength++;
                            i += 1;
                            score += 10;

                            posX.Add(lastX + (dirX * snakeSize));
                            posY.Add(lastY + (dirY * snakeSize));
                            updateDashBoard("SCORE: " + score);
                        }
                        else
                        {
                            posY[i] = lastY + (dirY * snakeSize);
                            posX[i] = lastX + (dirX * snakeSize);
                        }
                    }
                    else
                    {
                        if (posX[snakeLength - 1] == posX[i] && posY[snakeLength - 1] == posY[i])
                        {
                            GameOver();
                            break;
                        }
                        currX = posX[i];
                        currY = posY[i];
                        posX[i] = lastX;
                        posY[i] = lastY;
                        lastX = currX;
                        lastY = currY;
                    }
                }
                else
                {
                    updateDashBoard("PAUSED");
                }
                headSnakeX = posX[snakeLength - 1];
                headSnakeY = posY[snakeLength - 1];
                fillRect(posX[i], posY[i]);
            }
            fillRect(foodX, foodY);
            if ((headSnakeX < 20) || (headSnakeX > 560) || (headSnakeY < 20) || (headSnakeY > 560))
                GameOver();
        }

        private void updateDashBoard(string text)
        {
            textBox.Text = text;
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (!(dirX == 0 && dirY == -1))
                    {
                        dirX = 0;
                        dirY = 1;
                        isGameStarted = true;
                    }
                    break;
                case Key.Up:
                    if (!(dirX == 0 && dirY == 1))
                    {
                        dirX = 0;
                        dirY = -1;
                        isGameStarted = true;
                    }
                    break;
                case Key.Left:
                    if (!(dirX == 1 && dirY == 0))
                    {
                        dirX = -1;
                        dirY = 0;
                        isGameStarted = true;
                    }
                    break;
                case Key.Right:
                    if (!(dirX == -1 && dirY == 0))
                    {
                        dirX = 1;
                        dirY = 0;
                        isGameStarted = true;
                    }
                    break;
                case Key.Space:
                    isGameStarted = !isGameStarted;
                    break;
            }
        }

        private void fillRect(int xcoord, int ycoord)
        {
            Rectangle myRect = new Rectangle();
            myRect.Stroke = System.Windows.Media.Brushes.Black;
            myRect.Fill = System.Windows.Media.Brushes.SkyBlue;
            myRect.Height = snakeSize;
            myRect.Width = snakeSize;

            Canvas.SetLeft(myRect, xcoord);
            Canvas.SetTop(myRect, ycoord);
            paintCanvas.Children.Add(myRect);

            if (paintCanvas.Children.Count == snakeLength + 4)
                paintCanvas.Children.RemoveAt(1);
        }

        private void GameOver()
        {
            MessageBox.Show("Game Over");
            this.Close();
        }

    }
}
