using System;
using System.Text;

namespace ChatCoreTest
{
    internal class Program
    {
        private static byte[] m_PacketData;
        private static uint m_Pos;
        private static byte[] newArray;

        public static void Main()
        {
            m_PacketData = new byte[1024];
            newArray = new byte[1024];
            m_Pos = 0;

            Write(109);
            Write(109.99f);
            Write("Hello!");
            byte[] lengthData = WriteLength(m_Pos);

       
       
            for (int i = 0; i < m_Pos + 4; i++)
            {
                if (i < 4)
                {
                    m_PacketData[i] = lengthData[i];
                }
                else
                {
                    m_PacketData[i] = newArray[i - 4];
                }
            }

            Console.Write($"Output Byte array(length:{m_Pos}): ");
            for (var i = 0; i < m_Pos + 4; i++)
            {
                Console.Write(m_PacketData[i] + ", ");
            }

            byte[] byteDataLength = new byte[4];
            byte[] byteDataInt = new byte[4];
            byte[] byteDataFloat = new byte[4];
            byte[] byteDataString = new byte[12];

            int k = 0;
            while (k < m_Pos + 4)
            {
                if (k < 4)
                {
                    byteDataLength[k] = m_PacketData[k];
                }
                else if (k >= 4 && k < 8)
                {
                    byteDataInt[k - 4] = m_PacketData[k];
                }
                else if (k >= 8 && k < 12)
                {
                    byteDataFloat[k - 8] = m_PacketData[k];
                }
                else if (k >= 16)
                {
                    byteDataString[k - 16] = m_PacketData[k];
                }

                k++;
            }


            uint length = ReadUint(byteDataLength);
            int ans1 = ReadInt(byteDataInt);
            float ans2 = ReadFloat(byteDataFloat);
            string ans3 = ReadString(byteDataString);

            Console.WriteLine("");
            Console.WriteLine(length);
            Console.WriteLine(ans1);
            Console.WriteLine(ans2);
            Console.WriteLine(ans3);
            Console.ReadLine();
        }

        // write an integer into a byte array
        private static bool Write(int i)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(i);
            _Write(bytes);
            return true;
        }

        // write a float into a byte array
        private static bool Write(float f)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(f);
            _Write(bytes);
            return true;
        }

        // write a string into a byte array
        private static bool Write(string s)
        {
            // convert string to byte array
            var bytes = Encoding.Unicode.GetBytes(s);
            // write byte array length to packet's byte array
            if (Write(bytes.Length) == false)
            {
                return false;
            }

            _Write(bytes);
            return true;
        }

        // write a byte array into packet's byte array
        private static void _Write(byte[] byteData)
        {
            // converter little-endian to network's big-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            byteData.CopyTo(newArray, m_Pos);
            m_Pos += (uint)byteData.Length;
        }

        private static byte[] WriteLength(uint length)
        {
            byte[] lengthByteData = BitConverter.GetBytes(length);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthByteData);
            }

            return lengthByteData;
        }

        private static uint ReadUint(byte[] uintByteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(uintByteData);
            }

            uint result = BitConverter.ToUInt32(uintByteData, 0);
            return result;
        }

        private static int ReadInt(byte[] intByteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intByteData);
            }

            int result = BitConverter.ToInt32(intByteData, 0);

            return result;
        }

        private static float ReadFloat(byte[] floatByteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(floatByteData);
            }


            float result = BitConverter.ToSingle(floatByteData, 0);
            return result;
        }

        private static string ReadString(byte[] stringByteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(stringByteData);
            }

            string result = System.Text.Encoding.Unicode.GetString(stringByteData);

            return result;
        }
    }

}
