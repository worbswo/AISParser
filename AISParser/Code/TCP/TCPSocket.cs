using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace AISParser.Code.TCP
{
 
    public class TCPSocket
    {
        #region Property

        IPHostEntry hostEntry { get; set; }
        List<IPEndPoint> Ipe { get; set; } = new List<IPEndPoint>();
        Socket Socket { get; set; }
        Thread ReceiveMessageThread { get; set; } = null;
        public ConcurrentQueue<AISData> ReceiveMessageList { get; set; } = new ConcurrentQueue<AISData>();
        #endregion

        #region Constructor
        public TCPSocket(string host, int port)
        {

            hostEntry = Dns.GetHostEntry(host);
            foreach (IPAddress address in hostEntry.AddressList)
            {
                Ipe.Add(new IPEndPoint(address, port));
            }
            ReceiveMessageThread = new Thread(receiveMessage);
            ReceiveMessageThread.Start();
        }
        #endregion

        #region Method
        public void SetupTCPClientSocket()
        {
            bool isConneted = false;
            while (true)
            {
                foreach (IPEndPoint ipe in Ipe)
                {
                    Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        tempSocket.Connect(ipe);
                    }
                    catch
                    {
                        break;
                    }
                    if (tempSocket.Connected)
                    {
                        Socket = tempSocket;
                        isConneted = true;
                        break;
                    }
                }
                if (isConneted)
                {
                    break;
                }
            }
        }
        private void receiveMessage()
        {
            SetupTCPClientSocket();
            bool isStart = false;
            bool isEnd = false;
            List<byte> tmpByte = new List<byte>();
            byte[] endByte = new byte[1];
            byte[] receiveByte = new byte[200];

            while (true)
            {
                int readByteCount = -1;
                try
                {
                    readByteCount = Socket.Receive(receiveByte, receiveByte.Length, 0);
                }
                catch (SocketException e)
                {
					
						Console.WriteLine("Error code : "+ e.NativeErrorCode.ToString() + "  =>");
		
					
						Console.WriteLine("연결이 끊겼습니다. \n 연결을 재시도 합니다.");
						Socket.Close();
						SetupTCPClientSocket();
					
                    

                }
                for (int i = 0,count=readByteCount; i < count; i++)
                {

                    if (receiveByte[i] == '!' )
                    {
                        isStart = true;
                    }
                    if (isStart)
                    {
                        if (receiveByte[i] == '\r')
                        {
                            isEnd = true;
                        }
                        else
                        {
                            tmpByte.Add((byte)receiveByte[i]);
                        }
                    }
                    if (isEnd)
                    {
                        if (tmpByte.Count != 0)
                        {
                            endByte = new byte[tmpByte.Count];
                            for (int idx = 0; idx < tmpByte.Count; idx++)
                            {
                                endByte[idx] = tmpByte[idx];
                            }
                        }
                        ReceiveMessageList.Enqueue(new AISData() { Data= (byte[])endByte.Clone() ,Length=tmpByte.Count});
                        tmpByte.Clear();

                        isStart = false;
                        isEnd = false;
                    }
                }
                Thread.Sleep(1);
                

            }
        }
        public void Close()
        {
            if (Socket != null)
            {
                Socket.Close();
            }
            ReceiveMessageThread.Abort();
        }
        #endregion
    }
}
