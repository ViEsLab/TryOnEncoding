using System;

namespace ViEEncoding {
    public class VarInt {
        /// <summary>
        /// 将int类型数据转为VarInt形式
        /// </summary>
        /// <param name="rawValue"> 原数据 </param>
        /// <returns></returns>
        public static byte[] RawInt32ToVarInt(int rawValue) {
            int length = GetRawInt32Size(rawValue);
            byte[] varIntBuffer = new byte[length];
            int index = 0;
            while (true) {
                if ((rawValue & ~0x7f) == 0) {
                    varIntBuffer[index] = (byte)(rawValue & 0x7f);
                    break;
                } else {
                    varIntBuffer[index] = (byte)((rawValue & 0x7f) | 0x80);
                    rawValue = rawValue >> 7;
                }
                index++;
            }
            return varIntBuffer;
        }

        /// <summary>
        /// 计算数据字节长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetRawInt32Size(Int32 value) {
            if ((value & (0xffffffff << 7)) == 0) {
                return 1;
            } else if ((value & (0xffffffff << 14)) == 0) {
                return 2;
            } else if ((value & (0xffffffff << 21)) == 0) {
                return 3;
            } else if ((value & (0xffffffff << 28)) == 0) {
                return 4;
            } else {
                return 5;
            }
        }

        /// <summary>
        /// 恢复数据
        /// </summary>
        /// <param name="buffer"> 字节流数组 </param>
        /// <returns> 原数据 </returns>
        public static int GetVarIntRawValue(byte[] buffer) {
            Int32 result = 0;
            for(int i = 0; i < buffer.Length; i++) {
                byte value = buffer[i];
                if ((value & 0xff) == 0) {
                    break;
                }
                result = result | (value & 127) << (7 * i);
            }
            return result;
        }
    }
}