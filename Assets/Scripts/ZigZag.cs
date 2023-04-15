using System;

namespace ViEEncoding {
    public static class ZigZag {
        public static int RawInt32ToZigZag(Int32 rawValue) {
            return (rawValue << 1) ^ (rawValue >> 31);
        }

        public static long RawInt64ToZigZag(Int64 rawValue) {
            return (rawValue << 1) ^ (rawValue >> 63);
        }
    }
}