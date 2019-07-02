using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    //мета
    [Serializable]
    public class Meta
    {
        //список фигур 
        //public readonly List<Figure> figures = new List<Figure>();
      
        //сохранение в файл
        public void MetaSave(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
                new BinaryFormatter().Serialize(fs, this);
        }

        //чтение из файла
        public static Meta MetaLoad(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
                return (Meta)new BinaryFormatter().Deserialize(fs);
        }
    }

    [Serializable]
    public abstract class Figure
    {
        //линии фигуры
        readonly SerializableGraphicsPath serializablePath = new SerializableGraphicsPath();
        protected GraphicsPath Path { get { return serializablePath.path; } }

        public Point p0, p1;
        public Figure(Point a, Point b)
        {
            p0 = a; p1 = b;
        }
    }
    [Serializable]
    public class Line : Figure
    {
       public Line(Point a, Point b): base(a, b) {}
    }
    [Serializable]
    public class Rect : Figure
    {      
        public Rect(Point a, Point b): base(a,b) {}             
    }
    [Serializable]
    public class Ellips: Figure
    {       
        public Ellips(Point a, Point b): base(a,b) {}              
    }

    //сериализуемая обертка над GraphicsPath
    [Serializable]
    public class SerializableGraphicsPath : ISerializable
    {
        public GraphicsPath path = new GraphicsPath();

        public SerializableGraphicsPath()
        {
        }

        private SerializableGraphicsPath(SerializationInfo info, StreamingContext context)
        {
            if (info.MemberCount > 0)
            {
                PointF[] points = (PointF[])info.GetValue("p", typeof(PointF[]));
                byte[] types = (byte[])info.GetValue("t", typeof(byte[]));
                path = new GraphicsPath(points, types);
            }
            else
                path = new GraphicsPath();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (path.PointCount > 0)
            {
                info.AddValue("p", path.PathPoints);
                info.AddValue("t", path.PathTypes);
            }
        }
    }
}
