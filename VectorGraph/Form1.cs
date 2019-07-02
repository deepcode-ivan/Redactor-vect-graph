using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VectorGraph
{
    public partial class Form1 : Form
    {
        Meta mf = new Meta();
        public Form1()
        {
            InitializeComponent();
        }
       
#region  Панель МЕНЮ---------------------------------------------------------------------

        private void New(object sender, EventArgs e)
        {                  
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            lines.Clear();
            rects.Clear();
            ellipses.Clear();           
        }
        private void Open(object sender, EventArgs e)
        {            
            OpenFileDialog oDial = new OpenFileDialog();
            oDial.InitialDirectory = @".\";
            oDial.Filter = @"VECT файлы (*.vct)|*.vct|Все файлы (*.*)|*.*";
            oDial.FilterIndex = 1;
            oDial.RestoreDirectory = true;
            if (oDial.ShowDialog() == DialogResult.OK)
            {
                mf = Meta.MetaLoad(oDial.FileName);
                //figures = mf;                
            }
        }
        private void Save(object sender, EventArgs e)
        {                    
            SaveFileDialog sDial = new SaveFileDialog();
            sDial.Filter = "VECT(*.vct)|*.vct";
            sDial.Title = "Сохранить в файл";
            if (sDial.ShowDialog() == DialogResult.OK)
                mf.MetaSave(sDial.FileName);            
        }
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
#endregion----------------------------------------------------------------------
        
        public List<Line> lines = new List<Line>(); // список линий
        public List<Rect> rects = new List<Rect>(); //список прямоугольников
        public List<Ellips> ellipses = new List<Ellips>(); //список эллиcов      
                  
        bool IsClicked = false; //нажата ли кнопка мыши
        int X0, Y0, X1, Y1;     //координаты начальной и конечной точек

        private enum Element { line, rectangle, ellips, empty };
        private Element x = Element.empty;

        //выбор линий
        private void toolStripButton4_Click(object sender, EventArgs e) {x = Element.line;}

        //выбор прямоугольников
        private void toolStripButton5_Click(object sender, EventArgs e) {x = Element.rectangle;}

        //выбор эллипсов
        private void toolStripButton6_Click(object sender, EventArgs e) {x = Element.ellips;}

        //выбор редактирования
        private void toolStripButton7_Click(object sender, EventArgs e) {x = Element.empty;}

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            IsClicked = true;
            {   //координаты первой точки
                X0 = e.X;
                Y0 = e.Y;
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {   //координаты мыши
            string coor = "X = " + e.X + " Y = " + e.Y;
            toolStripStatusLabel1.Text = coor;

            if (IsClicked) //если нажата кнопка мыши
            {
                if (x == Element.line || x == Element.rectangle || x == Element.ellips)
                {   //координаты второй точки
                    X1 = e.X;
                    Y1 = e.Y;
                    pictureBox1.Invalidate();
                }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //блоки редактирования
                if (x == Element.empty)
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].p0.X <= e.X + 6 && lines[i].p0.X >= e.X - 6 && lines[i].p0.Y <= e.Y + 6 && lines[i].p0.Y >= e.Y - 6)
                        {
                            X0 = lines[i].p0.X = e.X;
                            Y0 = lines[i].p0.Y = e.Y;                           
                        }  
                    }
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].p1.X <= e.X + 6 && lines[i].p1.X >= e.X - 6 && lines[i].p1.Y <= e.Y + 6 && lines[i].p1.Y >= e.Y - 6)
                        {
                            X1 = lines[i].p1.X = e.X;
                            Y1 = lines[i].p1.Y = e.Y;                            
                        }
                    }
                    for (int i = 0; i < rects.Count; i++)
                    {
                        if (rects[i].p0.X <= e.X + 6 && rects[i].p0.X >= e.X - 6 && rects[i].p0.Y <= e.Y + 6 && rects[i].p0.Y >= e.Y - 6)
                        {
                            X0 = rects[i].p0.X = e.X;
                            Y0 = rects[i].p0.Y = e.Y;
                        }
                    }
                    for (int i = 0; i < rects.Count; i++)
                    {
                        if (rects[i].p1.X <= e.X + 6 && rects[i].p1.X >= e.X - 6 && rects[i].p1.Y <= e.Y + 6 && rects[i].p1.Y >= e.Y - 6)
                        {
                            X1 = rects[i].p1.X = e.X;
                            Y1 = rects[i].p1.Y = e.Y;
                        }
                    }
                    for (int i = 0; i < ellipses.Count; i++)
                    {
                        if (ellipses[i].p0.X <= e.X + 6 && ellipses[i].p0.X >= e.X - 6 && ellipses[i].p0.Y <= e.Y + 6 && ellipses[i].p0.Y >= e.Y - 6)
                        {
                            X0 = ellipses[i].p0.X = e.X;
                            Y0 = ellipses[i].p0.Y = e.Y;
                        }
                    }
                    for (int i = 0; i < ellipses.Count; i++)
                    {
                        if (ellipses[i].p1.X <= e.X + 6 && ellipses[i].p1.X >= e.X - 6 && ellipses[i].p1.Y <= e.Y + 6 && ellipses[i].p1.Y >= e.Y - 6)
                        {
                            X1 = ellipses[i].p1.X = e.X;
                            Y1 = ellipses[i].p1.Y = e.Y;
                        }
                    }
                    pictureBox1.Invalidate();
                }
            }           
        } 

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            IsClicked = false;
            if (x == Element.line)
            {   //добаляем линию
                lines.Add(new Line(new Point(X0, Y0), new Point(X1, Y1)));
            }
            else if (x == Element.rectangle)
            {   //добавляем прямоугольник
                rects.Add(new Rect(new Point(X0, Y0), new Point(X1, Y1)));
            }
            else if (x == Element.ellips)
            {   //добавляем эллипс
                ellipses.Add(new Ellips(new Point(X0, Y0), new Point(X1, Y1)));
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {           
            Pen myPen = new Pen(Color.Black, 2); //перо для линий примитивов
            SolidBrush mB = new SolidBrush(Color.Red); //кисть для выделения точек
            // прорисовка линии
            if (x == Element.line)
            {   
                e.Graphics.DrawLine(myPen, new Point(X0, Y0), new Point(X1, Y1));             
                Rectangle pt0 = new Rectangle(X0 - 3, Y0 - 3, 6, 6);
                Rectangle pt1 = new Rectangle(X1 - 3, Y1 - 3, 6, 6);
                e.Graphics.FillRectangle(mB, pt0);
                e.Graphics.FillRectangle(mB, pt1);
            }
            //прорисовка прямоугольника
            if (x== Element.rectangle)
            {
                if (X0 > X1 && Y0 > Y1)      //влево вверх              
                    e.Graphics.DrawRectangle(myPen, new Rectangle(X1, Y1, X0 - X1, Y0 - Y1));
                else if (X0 < X1 && Y0 > Y1) //вправо вверх
                    e.Graphics.DrawRectangle(myPen, new Rectangle(X0, Y1, Math.Abs(X1 - X0), Y0 - Y1));
                else if (X0 > X1 && Y0 < Y1) //влево вниз
                    e.Graphics.DrawRectangle(myPen, new Rectangle(X1, Y0, Math.Abs(X1 - X0), Math.Abs(Y0 - Y1)));
                else
                    e.Graphics.DrawRectangle(myPen, new Rectangle(X0, Y0, X1 - X0, Y1 - Y0));             
                Rectangle pt0 = new Rectangle(X0 - 3, Y0 - 3, 6, 6);
                Rectangle pt1 = new Rectangle(X1 - 3, Y1 - 3, 6, 6);
                e.Graphics.FillRectangle(mB, pt0);
                e.Graphics.FillRectangle(mB, pt1);
            }
            //прорисовка эллипса
            if (x == Element.ellips)
            {
                e.Graphics.DrawEllipse(myPen, X0, Y0, X1 - X0, Y1 - Y0);
                Rectangle pt0 = new Rectangle(X0 - 3, Y0 - 3, 6, 6);
                Rectangle pt1 = new Rectangle(X1 - 3, Y1 - 3, 6, 6);
                e.Graphics.FillRectangle(mB, pt0);
                e.Graphics.FillRectangle(mB, pt1);
            }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // прорисовка линий из контейнера
            foreach (var p in lines)
            {              
                e.Graphics.DrawLine(myPen, p.p0, p.p1);              
                Rectangle pt0 = new Rectangle(p.p0.X - 3, p.p0.Y - 3, 6, 6);
                Rectangle pt1 = new Rectangle(p.p1.X - 3, p.p1.Y - 3, 6, 6);
                e.Graphics.FillRectangle(mB, pt0);
                e.Graphics.FillRectangle(mB, pt1);                              
            }
            //прорисовка прямоугольников из контейнеров
            foreach (var p in rects)
            {
                if (p.p0.X > p.p1.X && p.p0.Y > p.p1.Y) //влево вверх              
                { 
                    e.Graphics.DrawRectangle(myPen, new Rectangle(p.p1.X, p.p1.Y, p.p0.X - p.p1.X, p.p0.Y - p.p1.Y));                   
                }
                else if (p.p0.X < p.p1.X && p.p0.Y > p.p1.Y) //вправо вверх
                    e.Graphics.DrawRectangle(myPen, new Rectangle(p.p0.X, p.p1.Y, Math.Abs(p.p1.X - p.p0.X), p.p0.Y - p.p1.Y));
                else if (p.p0.X > p.p1.X && p.p0.Y < p.p1.Y) //влево вниз
                    e.Graphics.DrawRectangle(myPen, new Rectangle(p.p1.X, p.p0.Y, Math.Abs(p.p1.X - p.p0.X), Math.Abs(p.p0.Y - p.p1.Y)));
                else
                    e.Graphics.DrawRectangle(myPen, new Rectangle(p.p0.X, p.p0.Y, p.p1.X - p.p0.X, p.p1.Y - p.p0.Y));              
                Rectangle pt0 = new Rectangle(p.p0.X - 3, p.p0.Y - 3, 6, 6);
                Rectangle pt1 = new Rectangle(p.p1.X - 3, p.p1.Y - 3, 6, 6);
                e.Graphics.FillRectangle(mB, pt0);
                e.Graphics.FillRectangle(mB, pt1);
            }
            // прорисовка эллипсов из контейнеров
            foreach (var p in ellipses)
            {
                e.Graphics.DrawEllipse(myPen, new Rectangle(p.p0.X, p.p0.Y, p.p1.X - p.p0.X, p.p1.Y - p.p0.Y));
                Rectangle pt0 = new Rectangle(p.p0.X - 3, p.p0.Y - 3, 6, 6);
                Rectangle pt1 = new Rectangle(p.p1.X - 3, p.p1.Y - 3, 6, 6);
                e.Graphics.FillRectangle(mB, pt0);
                e.Graphics.FillRectangle(mB, pt1);
            }
        }
    }   
}
