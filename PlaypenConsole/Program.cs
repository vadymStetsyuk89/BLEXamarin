using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PlaypenConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            byte[] bytes = Encoding.UTF8.GetBytes("hello");
            byte[] bytes2 = "a123"
                .Select(token => Convert.ToByte(token.ToString()))
                .ToArray();

            Guid gui = Guid.ParseExact("6e400001b5a3f393e0a9e50e24dcca9e","N");


            // Hexadecimal Representation of 0.0500
            //string HexRep = "00-00-00-08-83";
            //// Converting to integer
            //Int32 IntRep = Int32.Parse(HexRep, NumberStyles.AllowHexSpecifier);
            //// Integer to Byte[] and presenting it for float conversion
            //float f = BitConverter.ToSingle(BitConverter.GetBytes(IntRep), 0);







            
            //BitConverter.GetBytes(f).CopyTo(bytes, 0);
            //Guid g = new Guid(bytes);
            //string sG = g.ToString();

        }
    }

    public class Animal { }

    public class Duck : Animal {
        public void Foo() { }
    }

    public class Transport { }

    public class Bike : Transport { }

    public static class ByteExtensions
    {
        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }
    }
}
