using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Game
{
    public class GameObject
    {

    }

    public class Graph
    {
        public List<Parts> list = new List<Parts>();
        private int list_count = 0;

        public void Add_list(Parts parts)
        {
            this.list.Add(parts);
            this.list_count++;
        }

        public void AddEdge(Parts from, Parts to)
        {
            from.neighbors.Add(to);
            from.Link_Count++;
            to.neighbors.Add(from);
            to.Link_Count++;
        }
    }

    [Serializable]
    public class Parts
    {
        public GameObject gameobject;
        public List<Parts> neighbors;
        private double x;
        private double y;
        private double z;
        private int link_count;
        protected string name;
        protected string type_name;

        public int Link_Count
        {
            get { return link_count; }
            set { link_count = value; }
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public void Locate(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    class Motor : Parts
    {
        public Motor()
        {
            Console.Write("Motor Name : ");
            string s = Console.ReadLine();
            this.name = s;
            this.type_name = "Motor";
        }
    }

    [Serializable]
    class Beam : Parts
    {
        public Beam()
        {
            Console.Write("Beam Name : ");
            string s = Console.ReadLine();
            this.name = s;
            this.type_name = "Beam";
        }

        private double length;
        private int hole_num;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        public int Hole_Num
        {
            get { return hole_num; }
            set { hole_num = value; }
        }

        private List<int> link_hole = new List<int>();
    }

    [Serializable]
    class Axle : Parts
    {
        public Axle()
        {
            Console.Write("Axle Name : ");
            string s = Console.ReadLine();
            this.name = s;
            this.type_name = "Axle";
        }
        private double length;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }
    }

    [Serializable]
    class Gear : Parts
    {
        public Gear()
        {
            Console.Write("Gear Name : ");
            string s = Console.ReadLine();
            this.name = s;
            this.type_name = "Gear";
        }

        private double diameter;
        private int teeth;

        public double Diameter
        {
            get { return diameter; }
            set { diameter = value; }
        }

        public int Teeth
        {
            get { return teeth; }
            set { teeth = value; }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
