using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Parser.Interface
{
    /// <summary>
    /// 제너릭 합 연산을 위한 인터페이스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IArithmeticable<T>
    {
        T Add(byte value, byte mask, int shift, bool direction);
    }
    class MyFloat : IArithmeticable<MyFloat>
    {
        public float val = new float();
        public MyFloat()
        {
            this.val = 0;
        }
        public MyFloat(float val)
        {
            this.val = val;
        }
        public MyFloat Add(byte Value, byte mask, int shift, bool direction)
        {
            if (direction)
            {
                return new MyFloat(val + (float)((Value & mask) << shift));
            }
            else
            {
                return new MyFloat(val + (float)((Value & mask) >> shift));

            }
        }

    }
    class MyByte : IArithmeticable<MyByte>
    {
        public byte val = new byte();
        public MyByte()
        {
            this.val = 0;
        }
        public MyByte(byte val)
        {
            this.val = val;
        }
        public MyByte Add(byte Value, byte mask, int shift, bool direction)
        {
            if (direction)
            {
                return new MyByte((byte)(val + (byte)((Value & mask) << shift)));
            }
            else
            {
                return new MyByte((byte)(val + (byte)((Value & mask) >> shift)));

            }
        }

    }
    class MyInt : IArithmeticable<MyInt>
    {
        public int val = new int();
        public MyInt()
        {
            this.val = 0;
        }
        public MyInt(int val)
        {
            this.val = val;
        }
        public MyInt Add(byte Value, byte mask, int shift, bool direction)
        {
            if (direction)
            {
                return new MyInt((int)(val + (int)((Value & mask) << shift)));
            }
            else
            {
                return new MyInt((int)(val + (int)((Value & mask) >> shift)));

            }
        }

    }
    class MyUshort : IArithmeticable<MyUshort>
    {
        public ushort val = new ushort();
        public MyUshort()
        {
            this.val = 0;
        }
        public MyUshort(ushort val)
        {
            this.val = val;
        }
        public MyUshort Add(byte Value, byte mask, int shift, bool direction)
        {
            if (direction)
            {
                return new MyUshort((ushort)(val + (ushort)((Value & mask) << shift)));
            }
            else
            {
                return new MyUshort((ushort)(val + (ushort)((Value & mask) >> shift)));

            }
        }

    }
    class MyUint : IArithmeticable<MyUint>
    {
        public uint val;
        public MyUint()
        {
        }
        public MyUint(uint val)
        {
            this.val = val;
        }
        public MyUint Add(byte Value, byte mask, int shift, bool direction)
        {
            if (direction)
            {
                return new MyUint((uint)(val + (uint)((Value & mask) << shift)));
            }
            else
            {
                return new MyUint((uint)(val + (uint)((Value & mask) >> shift)));

            }
        }

    }
    class MySbyte : IArithmeticable<MySbyte>
    {
        public sbyte val = new sbyte();
        public MySbyte()
        {
            this.val = 0;
        }
        public MySbyte(sbyte val)
        {
            this.val = val;
        }
        public MySbyte Add(byte Value, byte mask, int shift, bool direction)
        {
            if (direction)
            {
                return new MySbyte((sbyte)(val + (sbyte)((Value & mask) << shift)));
            }
            else
            {
                return new MySbyte((sbyte)(val + (sbyte)((Value & mask) >> shift)));

            }
        }
    }
}
