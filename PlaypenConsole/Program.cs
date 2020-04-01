using System;
using System.Collections.Generic;
using System.Globalization;

namespace PlaypenConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            // Hexadecimal Representation of 0.0500
            //string HexRep = "00-00-00-08-83";
            //// Converting to integer
            //Int32 IntRep = Int32.Parse(HexRep, NumberStyles.AllowHexSpecifier);
            //// Integer to Byte[] and presenting it for float conversion
            //float f = BitConverter.ToSingle(BitConverter.GetBytes(IntRep), 0);






            byte[] bytes = new byte[16];

            
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
