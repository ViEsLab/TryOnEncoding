using System;
using UnityEngine;
using ViEEncoding;

namespace EncodingTest {
    public class EncodingTest : MonoBehaviour {
        private void Start() {
            VarIntTest(1777777777);
        }

        private void VarIntTests() {
            VarIntTest(17);  // 00010001
            VarIntTest(1777);  // 00000110 11110001
            VarIntTest(1777777777);  // 01101001 11110110 10111100 01110001
        }

        private void VarIntTest(Int32 value) {
            int varLength = VarInt.GetRawInt32Size(value);
            Debug.Log("value length: " + varLength + ",value:" + value);
            byte[] bytesBuffer = VarInt.RawInt32ToVarInt(value);
            Int32 length = VarInt.GetVarIntRawValue(bytesBuffer);
            Debug.Log("receive length:" + length);
        }
    }
}