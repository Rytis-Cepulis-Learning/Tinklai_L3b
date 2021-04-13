﻿using System;
using System.Collections;

namespace DNS_simple_server
{
    public class ResponseMessage
    {
        private const int maxLen = 513;
        public byte[] Buffer { get; set; } = new byte[maxLen];

        public ResponseMessage()
        {
        }

        public void Build(QueryMessage queryMsg)
        {
            var offset = 0;

            CopyToBuffer(queryMsg.MessageId, ref offset);
            CopyToBuffer(BuildStatusBlock(queryMsg), ref offset);
            CopyToBuffer(queryMsg.NmQuestions, ref offset);
            CopyToBuffer(new byte[2] { 0, 1 }, ref offset);
            CopyToBuffer(queryMsg.NameServerRec, ref offset);
            CopyToBuffer(queryMsg.AddServerRec, ref offset);
        }

        private byte[] BuildStatusBlock(QueryMessage queryMsg)
        {
            //1
            //4bits copied
            //1
            //0
            //1bit copied
            //0
            //0000
            //4bits
            //0 - no error
            //1 - cant format
            //2 - problem with dns server
            //3 - does not exist

            var statusBlock = new byte[queryMsg.Status.Length];
            Array.Copy(queryMsg.Status, 0, statusBlock, 0, statusBlock.Length);

            statusBlock = SetBitInByteArr(statusBlock, 0, true);
            statusBlock = SetBitInByteArr(statusBlock, 5, true);
            statusBlock = SetBitInByteArr(statusBlock, 6, false);
            statusBlock = SetBitInByteArr(statusBlock, 8, false);
            statusBlock = SetBitInByteArr(statusBlock, 9, false);
            statusBlock = SetBitInByteArr(statusBlock, 10, false);
            statusBlock = SetBitInByteArr(statusBlock, 11, false);
            //fix 11th bit 0 - no error, 1 - cant format, 2 - problem with dns server, 3 - does not exist
            statusBlock = SetBitInByteArr(statusBlock, 12, false);
            statusBlock = SetBitInByteArr(statusBlock, 13, false);
            statusBlock = SetBitInByteArr(statusBlock, 14, false);
            statusBlock = SetBitInByteArr(statusBlock, 15, false);

            return statusBlock;
        }

        private byte[] SetBitInByteArr(byte[] byteArr, int bitIndex, bool setVal)
        {
            var bitArray = new BitArray(byteArr);
            bitArray.Set(bitIndex, setVal);
            bitArray.CopyTo(byteArr, 0);

            return byteArr;
        }

        public void CopyToBuffer(byte[] source, ref int offset)
        {
            Array.Copy(source, 0, Buffer, offset, source.Length);
            offset += source.Length;
        }
    }
}
