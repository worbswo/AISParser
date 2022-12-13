using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISParser.Code.Parser
{
    
   
    public static class BitParser<T>  where T : IArithmeticable<T>,new()
    {

        #region Method
        /// <summary>
        /// 시작 비트와 사이즈,메시지를 입력받아 원하는 형태의 데이터로 반환하는 함수
        /// </summary>
        /// <param name="message">AIS message</param>
        /// <param name="startBit">시작 비트</param>
        /// <param name="size">비트 사이즈</param>
        /// <returns>T 형으로 반환</returns>
        public static T Parsing(byte[] message, int startBit, int size)
        {
            byte[] startBits = new byte[] { 0b_0011_1111, 0b_0001_1111, 0b_0000_1111, 0b_0000_0111, 0b_0000_0011, 0b_0000_0001 };
            int startByteBit = startBit % 6;
            int startByteIdx = startBit / 6 ;
            T result = new T();
            int endBit = (size - startByteBit) % 6;

            int iter = (size - startByteBit + endBit) / 6;
            int bitShift = (size - (6 - startByteBit));
            if (bitShift >= 0)
            {
                result = result.Add(message[startByteIdx] , startBits[startByteBit], bitShift,true);
                if (iter < 0)
                {
                    iter = -iter;
                }
            }
            else
            {
                result = result.Add(message[startByteIdx], startBits[startByteBit], -bitShift,false);
            }
            iter++;
            startByteIdx++;
            for (int i = 0; i < iter; i++)
            {
                bitShift -= 6;

                if (bitShift > 0)
                {
                    result = result.Add(message[startByteIdx], startBits[0], bitShift,true);
                }
                else
                {
                    result = result.Add(message[startByteIdx], startBits[0], -bitShift,false);
                }
                startByteIdx++;
            }

            return result;
        }
        #endregion
    }
}
