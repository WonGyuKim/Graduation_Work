using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ConsoleApp1
{
    [Serializable]
    class GameObject
    {
        protected List<GameObject> list = new List<GameObject>();
        protected int linkCount = 0;
        private double x;
        private double y;
        private double z;
        protected string name;
        private double rotate_v;

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

        public double Rotate_v
        {
            get { return rotate_v; }
            set { rotate_v = value; }
        }

        public virtual void Add_list(GameObject go)
        {
            this.list.Add(go);
            this.linkCount++;
            go.list.Add(this);
            go.linkCount++;
        }

        public virtual void print_link()
        {
            Console.Write(this.name + " Link Name : ");
            foreach(GameObject go in this.list)
            {
                Console.Write(go.name + ", ");
            }
            Console.WriteLine($" Location X : {x}, Y : {y}, Z : {z}");
        }

        public void Locate(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    class Motor : GameObject
    {
       public Motor()
       {
            Console.Write("Motor Name : ");
            string s = Console.ReadLine();
            this.name = s;
       }
    }

    [Serializable]
    class Beam : GameObject
    {
        public Beam()
        {
            Console.Write("Beam Name : ");
            string s = Console.ReadLine();
            this.name = s;
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

        public override void Add_list(GameObject go)
        {
            base.Add_list(go);
            int Insert_num;
            Console.Write("오브젝트를 삽입할 곳을 입력하시오 : ");
            Insert_num = int.Parse(Console.ReadLine());
            this.link_hole.Add(Insert_num);
        }

        public override void print_link()
        {
            base.print_link();
            Console.WriteLine("Length : " + length);
            Console.WriteLine("Hole_Num : " + hole_num);
            Console.Write("link_hole : ");
            foreach(int li in link_hole)
            {
                Console.Write(li + ", ");
            }
            Console.WriteLine();
        }
    }

    [Serializable]
    class Axle : GameObject
    {
        public Axle()
        {
            Console.Write("Axle Name : ");
            string s = Console.ReadLine();
            this.name = s;
        }
        private double length;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        public override void print_link()
        {
            base.print_link();
            Console.WriteLine("Length : " + length);
        }
    }

    [Serializable]
    class Gear : GameObject
    {
        public Gear()
        {
            Console.Write("Gear Name : ");
            string s = Console.ReadLine();
            this.name = s;
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

        public override void print_link()
        {
            base.print_link();
            Console.WriteLine("Diameter : " + diameter);
            Console.WriteLine("Teeth : " + teeth);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stream ws = new FileStream("a.dat", FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            List<GameObject> list = new List<GameObject>();
            Motor M = new Motor();
            Axle A = new Axle();
            Gear G1 = new Gear();
            Gear G2 = new Gear();
            Beam B = new Beam();

            list.Add(M);
            list.Add(A);
            list.Add(G1);
            list.Add(G2);
            list.Add(B);

            M.Add_list(A);
            A.Add_list(G1);
            B.Add_list(A);
            G1.Add_list(G2);

            A.Length = 4;
            B.Length = 10;
            B.Hole_Num = 6;
            G1.Diameter = 8;
            G1.Teeth = 20;
            G2.Diameter = 4;
            G2.Teeth = 10;

            M.Locate(0, 0, 0);
            A.Locate(1, 2, 3);
            B.Locate(3.1, 4.7, 5.9);
            G1.Locate(7.3, 8.5, 9.1);
            G2.Locate(8, 9, 10);

            serializer.Serialize(ws, list);
            ws.Close();

            Stream rs = new FileStream("a.dat", FileMode.Open);
            BinaryFormatter deserializer = new BinaryFormatter();
            List<GameObject> list2;
            list2 = (List<GameObject>)deserializer.Deserialize(rs);
            rs.Close();
            
            foreach(GameObject go in list2)
            {
                go.print_link();
            }
        }
    }
}
