namespace newpol
{
    public partial class Form1 : Form
    {
        const int size = 10;
        double a = -1, b = 1;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double[] xValues = new double[size];
            double[] yValues = new double[size];

            for (int i = 0; i < size; i++)
            {
                xValues[i] = a + i * (b - a) / (size - 1);
                yValues[i] = TestF(xValues[i]);
            }

            Func<double, double> lagrangePolynomial = x => InterpolateLagrangePolynomial(x, xValues, yValues, size);

            double integral = 0;
            double h = (b - a) / (size - 1);
            for (int i = 0; i < size - 1; i++)
            {
                integral += (lagrangePolynomial(xValues[i]) + lagrangePolynomial(xValues[i + 1])) * h / 2;
            }

            label1.Text = $"Приближенное значение интеграла: {integral}";
        }

        static double InterpolateLagrangePolynomial(double x, double[] xValues, double[] yValues, int size)
        {
            double lagrangePol = 0;

            for (int i = 0; i < size; i++)
            {
                double basicsPol = 1;
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                    {
                        basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                }
                lagrangePol += basicsPol * yValues[i];
            }

            return lagrangePol;
        }

        static double TestF(double x)
        {
            return x * x;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {


            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Параметры осей и графика
            int marginLeft = 40;
            int marginTop = 20;
            int marginBottom = 40;
            int marginRight = 20;
            int width = panel1.Width - marginLeft - marginRight;
            int height = panel1.Height - marginTop - marginBottom;

            // Рисуем оси
            Pen axisPen = new Pen(Color.Black, 1);
            g.DrawLine(axisPen, marginLeft, marginTop, marginLeft, panel1.Height - marginBottom);
            g.DrawLine(axisPen, marginLeft, panel1.Height - marginBottom, panel1.Width - marginRight, panel1.Height - marginBottom);

            // Рисуем график
            int numberOfPoints = size;
            PointF[] points = new PointF[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                double x = a + i * (b - a) / (numberOfPoints - 1);
                double y = TestF(x);

                points[i] = new PointF(
                    marginLeft + (float)(i * width / (numberOfPoints - 1)),
                    panel1.Height - marginBottom - (float)(y * height / (b * b))
                );
            }

            Pen graphPen = new Pen(Color.Blue, 2);
            g.DrawLines(graphPen, points);

            // Очистка ресурсов
            axisPen.Dispose();
            graphPen.Dispose();

        }
    }
}
