using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HGServer.Network.Packet
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Message
    {
        public int Size
        {
            get;
            set;
        }

        public int MessageNo
        {
            get;
            set;
        }
    }
}