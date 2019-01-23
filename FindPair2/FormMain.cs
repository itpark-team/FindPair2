using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindPair2
{
    public partial class FormMain : Form
    {
        static Random rnd = new Random();

        const char GUESSED_CELL = '!';//если отгадана
        const char EMPTY_CELL = '.';//если отгадана

        const int N = 6;//кол-во строк
        const int M = 8;//кол-во столбцов
        char[,] field = new char[N, M];//игровое поле
        const int CELL_SIZE = 100;

        int i1Click = -1, j1Click = -1;//координаты ячейки 1 клетки из пары
        int i2Click = -1, j2Click = -1;//координаты ячейки 2 клетки из пары

        int gameStep = 0;

        Bitmap bitmap;
        Graphics graphics;

        private void ClearField()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    field[i, j] = EMPTY_CELL;
                }
            }
        }

        private void GenerateField()
        {
            int countPairs = N * M / 2;
            for (int k = 0; k < countPairs; k++)
            {
                char currentLetter = (char)rnd.Next('А', 'Я' + 1);

                for (int kk = 0; kk < 2; kk++)
                {
                    int i, j;
                    do
                    {
                        i = rnd.Next(0, N);
                        j = rnd.Next(0, M);
                    }
                    while (field[i, j] != EMPTY_CELL);
                    field[i, j] = currentLetter;
                }
            }
        }

        private void DrawField()
        {
            graphics.Clear(Color.White);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (field[i, j] != GUESSED_CELL)
                    {
                        graphics.DrawRectangle(new Pen(Color.Red), j * CELL_SIZE, i * CELL_SIZE, CELL_SIZE, CELL_SIZE);

                        if (i == i1Click && j == j1Click || i == i2Click && j == j2Click)
                        {
                            graphics.DrawString(field[i, j].ToString(), new Font("Arial", 20), new SolidBrush(Color.Black), j * CELL_SIZE + CELL_SIZE / 3, i * CELL_SIZE + CELL_SIZE / 3);
                        }
                    }
                    else
                    {
                        graphics.FillRectangle(new SolidBrush(Color.Red), j * CELL_SIZE, i * CELL_SIZE, CELL_SIZE, CELL_SIZE);
                    }
                }
            }
            pictureBoxField.Image = bitmap;
            pictureBoxField.Refresh();
        }

        private bool IsWin()
        {
            int countGuessed = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if(field[i,j]==GUESSED_CELL)
                    {
                        countGuessed++;
                    }
                }
            }
            return countGuessed == N * M;   
        }

        private void pictureBoxField_MouseDown(object sender, MouseEventArgs e)
        {
            gameStep++;

            if (gameStep == 1)
            {
                i1Click = e.Y / CELL_SIZE;
                j1Click = e.X / CELL_SIZE;

                DrawField();
            }
            else if (gameStep == 2)
            {
                i2Click = e.Y / CELL_SIZE;
                j2Click = e.X / CELL_SIZE;

                DrawField();
                System.Threading.Thread.Sleep(500);

                if (field[i1Click, j1Click] == field[i2Click, j2Click])
                {
                    field[i1Click, j1Click] = GUESSED_CELL;
                    field[i2Click, j2Click] = GUESSED_CELL;
                }
                
                i1Click = i2Click = j1Click = j2Click = -1;
                
                DrawField();

                if(IsWin())
                {
                    pictureBoxField.Enabled = false;
                    MessageBox.Show("Победа! Вы отгадали все буквы!");
                }
                    
                gameStep = 0;
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            int width = CELL_SIZE * M + 1;
            int height = CELL_SIZE * N + 1;

            pictureBoxField.Width = width;
            pictureBoxField.Height = height;

            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);

            ClearField();
            GenerateField();
            DrawField();
        }
    }
}
