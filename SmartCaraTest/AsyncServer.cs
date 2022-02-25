using SmartCaraTest.controls;
using SmartCaraTest.data;
using SmartCaraTest.util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SmartCaraTest
{
    public delegate void MessageHandler(byte[] message, int index);

    public class AsyncServer
    {
        ClientManager _clientManager = null;
        ConcurrentBag<byte[]> receiveLog = null;
        Thread CheckThread = null;
        public TcpListener listener = null;
        public bool run = false;
        MultiWindow1 window;

        public AsyncServer(MultiWindow1 window)
        {
            this.window = window;
        }

        public void Start()
        {
            run = true;
            _clientManager = new ClientManager();
            _clientManager.OnConnected += OnConnected;
            _clientManager.OnDisconnected += OnDisconnected;
            _clientManager.OnReceived += OnDataReceived;
            if (listener != null)
                listener = null;
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, 7755));
            listener.Start();
            Console.WriteLine("start");
            Task serverStart = Task.Run(() =>
            {
                AsycnServerStart();
            });
            CheckThread = new Thread(StateCheckLoop);
            CheckThread.Start();
        }

        private void StateCheckLoop()
        {
            while (run)
            {
                foreach(var item in ClientManager.clientDic)
                {
                    try
                    {
                        //if(item.Value.ResponseCount > 2 && !item.Value.Start)
                        //{
                        //    //item.Value.Start = true;
                        //    byte[] sendByteData = Protocol.GetCommand(2);
                        //    item.Value.client.GetStream().Write(sendByteData, 0, sendByteData.Length);
                        //}
                        //else
                        //{
                        byte[] sendByteData = Protocol.GetCommand(1);
                        item.Value.client.GetStream().Write(sendByteData, 0, sendByteData.Length);
                        if (!item.Value.channel.Response)
                        {
                            item.Value.channel.NonResponse++;
                            if (item.Value.channel.NonResponse > 20)
                            {
                                window.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    item.Value.channel.Item23.cont.Content = "응답없음";
                                }));
                                //item.Value.channel
                            }
                        }
                        //}
                    }
                    catch (Exception ex)
                    {

                    }
                }
                Thread.Sleep(1000);
            }
        }


        private void AsycnServerStart()
        {
            Console.WriteLine("Start");
            while (run)
            {
                try
                {
                    Task<TcpClient> acceptTask = listener.AcceptTcpClientAsync();
                    acceptTask.Wait();
                    TcpClient newClient = acceptTask.Result;
                    _clientManager.AddClient(newClient);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"here : {e}");
                    if (!run)
                        break;
                }
            }
            Console.WriteLine("Server Closed");
        }

        private void DataReceived(IAsyncResult ar)
        {
            ClientData callbackClient = ar.AsyncState as ClientData;
            int bytesRead = callbackClient.client.GetStream().EndRead(ar);
            string readString = Encoding.Default.GetString(callbackClient.readByteData, 0, bytesRead);
            Console.WriteLine("{0}번 사용자 : {1}", callbackClient.clientNumber, readString); 
            callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length, new AsyncCallback(DataReceived), callbackClient);
        }

        private void OnConnected(ClientData data)
        {
            Console.WriteLine("connected");
            int index = window.GetChannelIndex();
            Console.WriteLine("Channel {0}", index);
            if(index > -1)
            {
                ChannelItem channel = window.GetChannelItem(index);
                if(channel != null)
                {
                    channel.Channel = data.clientNumber;
                    data.channel = channel;
                    channel.client = data.client;
                }
            }
        }

        private void OnDisconnected(ClientData data)
        {

        }        

        private string byteToString(byte[] data)
        {
            string hex = "";
            foreach (byte b in data)
            {
                hex += " " + b.ToString("X2");
            }
            return hex;
        }

        private void OnDataReceived(ClientData data)
        {
            byte[] packet = data.readCompleteData.ToArray();
            data.ResponseCount++;
            data.channel.NonResponse = 0;
            data.readCompleteData.Clear();
            window.Dispatcher.BeginInvoke(new Action(() =>
            {
                //Console.WriteLine("123123");
                window.SetView(packet, data.channel);
            }));
        }
    }


}
