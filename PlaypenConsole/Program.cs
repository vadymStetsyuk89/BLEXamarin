using System;
using System.Collections.Generic;

namespace PlaypenConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Dictionary<Type, int> dic = new Dictionary<Type, int>();

        }
    }

    public class Animal { }

    public class Duck : Animal {
        public void Foo() { }
    }

    public class Transport { }

    public class Bike : Transport { }
}
