using AISParser.Code.Message;
using AISParser.Code.TCP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AISParser.Code.Parser
{

    public class MessageParser
    {
        #region Property
        internal int Count { get; set; }
        internal int Index { get; set; }
        internal string Identifystr { get; set; }
        internal bool IsContinue { get; set; }
        internal int NumofMessege { get; set; }
        internal int Num { get; set; }
        internal int SubMessageidx { get; set; } 
        internal byte MessageID { get; set; }


        internal byte[] SixBitMessage { get; set; } = new byte[100];
        internal List<LongMessageTemplete> LongMessageTempletes { get; set; } = new List<LongMessageTemplete>();
        internal List<AISMessageBase> AISMessages { get; set; }
        internal TCPSocket Tcp { get; set; }

        #endregion

        #region Constroctor
        public MessageParser()
        {
            initAISMessage();
            Tcp = new TCPSocket("155.155.4.173", 8020);
          //  Tcp = new TCPSocket("155.155.4.79", 4005);

        }
        #endregion

        #region Method
        public void Close()
        {
            Tcp.Close();
        }
        /// <summary>
        /// 메세지를 받아서 알맞은 MessageType으로 보낸다.
        /// </summary>
        /// <param name="message"></param>
        public AISMessageBase Parsing()
        {

            Count = 0;
            Index = 0;
            Identifystr = "";


            if (Tcp.ReceiveMessageList.IsEmpty) //ReceiveMessageList에 속한 자료가 없을 때 null 반환
            {
                return null;
            }
           
            if(!Tcp.ReceiveMessageList.TryDequeue(out AISData message))
            {
                return null;
            }
           // Console.WriteLine(Tcp.ReceiveMessageList.Count.ToString());
            IsContinue = false;
            NumofMessege = 0;
            Num = 0;
            SubMessageidx = -1;
            MessageID = 0;
            if (message == null) return null;

            for (int k=0;k<message.Length;k++)
            {
                byte item = message[k];
                int sixBits = SixBitConvert(item);
                if (item == '$') return null;
                if (item == ',')
                {
                    Count++;
                }
                else
                {
                    if(Count == 0)
                    {
                        Identifystr += (char)item;
                    }
                    else if (Count == 1)
                    {
                        if(Identifystr.Contains("RES")||Identifystr.Contains("SLT"))
                        {
                            return null;
                        }
                        if (sixBits >= 2)
                        {
                            IsContinue = true;
                            NumofMessege = sixBits;
                        }
                    }
                    else if (Count == 2 && IsContinue)
                    {
                        Num = sixBits - 1;
                    }
                    else if (Count == 3 && IsContinue)
                    {
                        bool isExist = false;
                        SubMessageidx = -1;
                        for (int i = 0; i < LongMessageTempletes.Count; i++)
                        {
                            if (LongMessageTempletes[i].SubMessageId == sixBits)
                            {
                                if (Identifystr == LongMessageTempletes[i].Identify)
                                {
                                    SubMessageidx = i;
                                    isExist = true;
                                    if (Num >= 0 && Num < LongMessageTempletes[SubMessageidx].SixBitMessage.Count)
                                    {
                                        LongMessageTempletes[SubMessageidx].isMessagesChecked[Num] = false;
                                        LongMessageTempletes[SubMessageidx].SixBitMessageCount[Num] = 0;
                                    }
                                    if (Num == 0)
                                    {
                                        for(int j = 0; j < NumofMessege; j++)
                                        {
                                            LongMessageTempletes[SubMessageidx].isMessagesChecked[j] = false;
                                            LongMessageTempletes[SubMessageidx].SixBitMessageCount[j] = 0;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        if (!isExist)
                        {
                            LongMessageTemplete longMessage = new LongMessageTemplete();
                            
                                longMessage.SubMessageId = sixBits;
                                longMessage.SixBitMessages = new byte[200];
                                longMessage.isMessagesChecked = new bool[NumofMessege];
                                for (int i = 0; i < NumofMessege; i++)
                                {
                                    longMessage.SixBitMessage.Add(new byte[100]);
                                    longMessage.SixBitMessageCount.Add(0);
                                }
                                longMessage.Identify = Identifystr;
                                LongMessageTempletes.Add(longMessage);
                                SubMessageidx = LongMessageTempletes.Count - 1;
                            
                            
                        }
                    }
                    else if (Count == 5 && item != '\r' && item != '\n')
                    {
                        if (IsContinue)
                        {          
                            LongMessageTempletes[SubMessageidx].SixBitMessage[Num][Index++] = (byte)sixBits;
                            LongMessageTempletes[SubMessageidx].isMessagesChecked[Num] = true;
                            LongMessageTempletes[SubMessageidx].SixBitMessageCount[Num]++;
                        }
                        else
                        {
                            SixBitMessage[Index++] = (byte)sixBits;
                        }
                    }
                }
            }
            if (!IsContinue)
            {
                MessageID = AISMessageBase.ParsingMessageID(SixBitMessage);
                if (MessageID <= 8 || MessageID == 11 || MessageID == 15 || MessageID == 18 || MessageID == 20 || MessageID == 21 || MessageID == 24)
                {
                    int idx = (int)MessageID;
                    AISMessages[idx].MessageId = MessageID;
                    AISMessages[idx].Parsing(SixBitMessage);
                    return AISMessages[idx];
                }
            }
            else
            {
                int tmpIndex = -1;
                for (int i = 0; i < LongMessageTempletes.Count; i++)
                {
                    bool isAllCheck = true;
                    foreach (var check in LongMessageTempletes[i].isMessagesChecked)
                    {
                        if (!check)
                        {
                            isAllCheck = false;
                            break;
                        }
                    }
                    if (isAllCheck)
                    {
                        int idx = 0;

                        for (int k = 0; k < LongMessageTempletes[i].SixBitMessage.Count; k++)
                        {
                            int countIdx = 0;

                            foreach (var mess in LongMessageTempletes[i].SixBitMessage[k])
                            {
                                if (countIdx >=LongMessageTempletes[i].SixBitMessageCount[k])
                                {
                                    countIdx = 0;
                                    break;
                                }
                                LongMessageTempletes[i].SixBitMessages[idx++] = mess;
                                countIdx++;
                            }
                            LongMessageTempletes[i].SixBitMessages[idx] = 65;
                        }

                        MessageID = AISMessageBase.ParsingMessageID(LongMessageTempletes[i].SixBitMessages);
                        if (MessageID <= 6 || MessageID == 21)
                        {
                            AISMessages[(int)MessageID].MessageId = MessageID;
                            AISMessages[(int)MessageID].Parsing(LongMessageTempletes[i].SixBitMessages); 
                            tmpIndex = i;
                            break;
                        }
                    }
                }
                if (tmpIndex != -1)
                {
                    LongMessageTempletes.RemoveAt(tmpIndex);
                    return AISMessages[(int)MessageID];

                }


            }
            return null;
        }
        /// <summary>
        /// 수신한 메시지를 6비트 아스키코드로 변환
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int SixBitConvert(byte message)
        {
            int sixBits = ((int)message - 48);
            if (sixBits > 40)
            {
                sixBits -= 8;
            }
            return sixBits;
        }

        public void initAISMessage()
        {
            AISMessages = new List<AISMessageBase>();
            for (int i = 0; i < 28; i++)
            {
                if (i == 1 || i == 2 || i == 3)
                {
                    AISMessages.Add(new PositionReportMessage());
                }
                else if (i == 4 || i == 11)
                {
                    AISMessages.Add(new BaseStationMessage());
                }
                else if (i == 5)
                {
                    AISMessages.Add(new ShipStaticAndVoyageRelatedDataMessage());
                }
                else if (i == 6)
                {
                    AISMessages.Add(new AddressedBinaryMessage());

                }
                else if (i == 8) 
                {
                    AISMessages.Add(new BinaryBroadcastMessage());

                }else if (i == 9) 
                {
                    AISMessages.Add(new StandardSearchAndResuceAircraftMessage());
                }
                else if (i == 15)
                {
                    AISMessages.Add(new InterrogationMessage());
                }
                else if (i == 18)
                {
                    AISMessages.Add(new StandardClassBEquipment());
                }
                else if (i == 20)
                {
                    AISMessages.Add(new DataLinkManagementMessage());
                }
                else if (i == 21)
                {
                    AISMessages.Add(new AidsToNavigationMessage());
                }
                else if (i == 24)
                {
                    AISMessages.Add(new StaticDataMessage());
                }
                else
                {
                    AISMessages.Add(new AISMessageBase());
                }
            }
        }
        #endregion
    }
}
