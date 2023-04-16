using System;

namespace ViEEncoding {
    public class VarInt {
        /// <summary>
        /// 将int类型数据转为VarInt形式
        /// </summary>
        /// <param name="rawValue"> 原数据 </param>
        /// <returns></returns>
        public static byte[] RawInt32ToVarInt(int rawValue) {
            int length = GetVarIntSize(rawValue);
            byte[] varIntBuffer = new byte[length];
            int index = 0;
            while (true) {
                if ((rawValue & ~0x7f) == 0) {
                    varIntBuffer[index] = (byte)(rawValue & 0x7f);
                    break;
                } else {
                    varIntBuffer[index] = (byte)((rawValue & 0x7f) | 0x80);
                    rawValue = LogicalRightMove(rawValue, 7);
                }
                index++;
            }
            return varIntBuffer;
        }

        public static byte[] RawInt32ToVarIntByZigZag(int rawValue) {
            int value = ZigZag.RawInt32ToZigZag(rawValue);
            return RawInt32ToVarInt(value);
        }

        public static int GetVarIntSize(Int32 rawValue) {
            int result = 0;
            do {
                result++;
                rawValue = LogicalRightMove(rawValue, 7);
            }
            while (rawValue != 0);

            return result;
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
                result |= (value & 0x7f) << (7 * i);
            }
            return result;
        }

        public static int GetVarIntRawValueWithZigZag(byte[] buffer) {
            int value = GetVarIntRawValue(buffer);
            return LogicalRightMove(value, 1) ^ -(value & 1);
        }

        private static int LogicalRightMove(int value, int bit) {
            // Or (int)((uint)rawValue >> 7)
            if (bit != 0) {
                value >>= 1;
                value &= 0xfffffff;
                value >>= bit - 1;
            }

            return value;
        }

        private static Int64 Int64LogicalRightMove(Int64 value, int bit) {
            // Or (int)((uint)rawValue >> 7)
            if (bit != 0) {
                value >>= 1;
                value &= 0xfffffffffffffff;
                value >>= bit - 1;
            }

            return value;
        }
    }
}