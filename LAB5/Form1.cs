using System;
using System.Drawing;
using System.Windows.Forms;

namespace LAB5
{
    public class Form1 : Form
    {
        private PointF P1, P2, P3, P4;
        private bool shouldDraw = false;

        TextBox txtX1, txtY1, txtX2, txtY2, txtX3, txtY3, txtX4, txtY4;
        Button btnDraw;
        Panel drawingPanel;

        public Form1()
        {
            this.Text = "Крива Безьє 3-го порядку";
            this.ClientSize = new Size(800, 500);

            Label lbl = new Label() { Text = "Координати точок:", Location = new Point(10, 10), AutoSize = true };
            this.Controls.Add(lbl);

            CreatePointInput("P1", 10, 40, out txtX1, out txtY1);
            CreatePointInput("P2", 10, 70, out txtX2, out txtY2);
            CreatePointInput("P3", 10, 100, out txtX3, out txtY3);
            CreatePointInput("P4", 10, 130, out txtX4, out txtY4);

            btnDraw = new Button()
            {
                Text = "Намалювати криву Безьє",
                Location = new Point(10, 170),
                Width = 180
            };
            btnDraw.Click += BtnDraw_Click;
            this.Controls.Add(btnDraw);

            // Панель для малювання справа
            drawingPanel = new Panel()
            {
                Location = new Point(220, 10),
                Size = new Size(560, 480),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            drawingPanel.Paint += DrawingPanel_Paint;
            this.Controls.Add(drawingPanel);
        }

        private void CreatePointInput(string label, int x, int y, out TextBox txtX, out TextBox txtY)
        {
            Label lbl = new Label() { Text = label, Location = new Point(x, y + 3), Width = 25 };
            this.Controls.Add(lbl);

            txtX = new TextBox() { Location = new Point(x + 30, y), Width = 50, Text = "50" };
            txtY = new TextBox() { Location = new Point(x + 90, y), Width = 50, Text = "50" };
            this.Controls.Add(txtX);
            this.Controls.Add(txtY);
        }

        private void BtnDraw_Click(object sender, EventArgs e)
        {
            float x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;

            bool ok =
                float.TryParse(txtX1.Text, out x1) &&
                float.TryParse(txtY1.Text, out y1) &&
                float.TryParse(txtX2.Text, out x2) &&
                float.TryParse(txtY2.Text, out y2) &&
                float.TryParse(txtX3.Text, out x3) &&
                float.TryParse(txtY3.Text, out y3) &&
                float.TryParse(txtX4.Text, out x4) &&
                float.TryParse(txtY4.Text, out y4);

            if (ok)
            {
                P1 = new PointF(x1, y1);
                P2 = new PointF(x2, y2);
                P3 = new PointF(x3, y3);
                P4 = new PointF(x4, y4);
                shouldDraw = true;
                drawingPanel.Invalidate(); // Малюємо на панелі
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректні координати.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!shouldDraw) return;

            Graphics g = e.Graphics;

            // Для зручності масштабування і зміщення:
            float scale = 4f;  // масштабування — збільшити крива
            float offsetX = 20;
            float offsetY = drawingPanel.Height - 20; // щоб y зростало вгору

            // Функція для трансформування координат
            PointF Transform(PointF pt)
            {
                return new PointF(offsetX + pt.X * scale, offsetY - pt.Y * scale);
            }

            PointF tP1 = Transform(P1);
            PointF tP2 = Transform(P2);
            PointF tP3 = Transform(P3);
            PointF tP4 = Transform(P4);

            // Малюємо опорні точки
            DrawPoint(g, tP1, Brushes.Red);
            DrawPoint(g, tP2, Brushes.Green);
            DrawPoint(g, tP3, Brushes.Blue);
            DrawPoint(g, tP4, Brushes.Orange);

            // Чотирикутник
            Pen penQuad = new Pen(Color.Gray, 1);
            g.DrawLine(penQuad, tP1, tP2);
            g.DrawLine(penQuad, tP2, tP3);
            g.DrawLine(penQuad, tP3, tP4);
            g.DrawLine(penQuad, tP4, tP1);

            // Крива Безьє
            DrawBezier(g, tP1, tP2, tP3, tP4, Pens.Black);
        }

        private void DrawPoint(Graphics g, PointF pt, Brush brush)
        {
            float r = 5;
            g.FillEllipse(brush, pt.X - r, pt.Y - r, r * 2, r * 2);
        }

        private void DrawBezier(Graphics g, PointF p0, PointF p1, PointF p2, PointF p3, Pen pen)
        {
            PointF prev = p0;
            for (float t = 0.01f; t <= 1; t += 0.01f)
            {
                float u = 1 - t;
                PointF pt = new PointF(
                    u * u * u * p0.X + 3 * u * u * t * p1.X + 3 * u * t * t * p2.X + t * t * t * p3.X,
                    u * u * u * p0.Y + 3 * u * u * t * p1.Y + 3 * u * t * t * p2.Y + t * t * t * p3.Y
                );
                g.DrawLine(pen, prev, pt);
                prev = pt;
            }
        }
    }
}
