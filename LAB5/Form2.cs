using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArchimedeanTree
{
    public partial class Form2 : Form
    {
        private Panel drawingPanel;
        private TextBox txtA, txtB, txtK;
        private Button btnDraw;

        private int a = 50, b = 80, k = 5;

        public Form2()
        {
            InitializeComponent();

            this.Text = "Фрактал - Дерево Архімеда";
            this.ClientSize = new Size(800, 600);

            Label lblA = new Label() { Text = "A:", Location = new Point(10, 20), AutoSize = true };
            txtA = new TextBox() { Location = new Point(40, 15), Width = 50, Text = "50" };
            Label lblB = new Label() { Text = "B:", Location = new Point(100, 20), AutoSize = true };
            txtB = new TextBox() { Location = new Point(130, 15), Width = 50, Text = "80" };
            Label lblK = new Label() { Text = "K:", Location = new Point(190, 20), AutoSize = true };
            txtK = new TextBox() { Location = new Point(220, 15), Width = 50, Text = "5" };

            btnDraw = new Button() { Text = "Намалювати дерево", Location = new Point(290, 13), Width = 150 };
            btnDraw.Click += BtnDraw_Click;

            drawingPanel = new Panel()
            {
                Location = new Point(10, 50),
                Size = new Size(780, 540),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            drawingPanel.Paint += DrawingPanel_Paint;

            this.Controls.Add(lblA);
            this.Controls.Add(txtA);
            this.Controls.Add(lblB);
            this.Controls.Add(txtB);
            this.Controls.Add(lblK);
            this.Controls.Add(txtK);
            this.Controls.Add(btnDraw);
            this.Controls.Add(drawingPanel);
        }

        private void BtnDraw_Click(object? sender, EventArgs e)
        {
            bool okA = int.TryParse(txtA.Text, out a);
            bool okB = int.TryParse(txtB.Text, out b);
            bool okK = int.TryParse(txtK.Text, out k);

            if (okA && okB && okK && k >= 0)
                drawingPanel.Invalidate();
            else
                MessageBox.Show("Перевірте правильність введення A, B та K.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DrawingPanel_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float startX = drawingPanel.Width / 2;
            float startY = drawingPanel.Height - 10;

            DrawTree(g, startX, startY, -90, k, a, b);
        }

        private void DrawTree(Graphics g, float x, float y, float angle, int depth, float a, float b)
        {
            if (depth == 0) return;

            float rad = angle * (float)Math.PI / 180;

            float x1 = x + b * (float)Math.Cos(rad);
            float y1 = y + b * (float)Math.Sin(rad);

            g.DrawLine(Pens.Brown, x, y, x1, y1);

            float newA = a * 0.7f;
            float newB = b * 0.7f;

            DrawTree(g, x1, y1, angle - a, depth - 1, newA, newB);
            DrawTree(g, x1, y1, angle + a, depth - 1, newA, newB);
        }
    }
}
