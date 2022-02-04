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
        TcpListener listener = null;
        bool run = false;

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
            Task serverStart = Task.Run(() =>
            {
                AsycnServerStart();
            });
            CheckThread = new Thread(StateCheckLoop);
            CheckThread.Start();
        }

        private void StateCheckLoop()
        {
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
            Console.WriteLine("{0}번 사용자 : {1}", callbackClient.clientNumber, readString); callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length, new AsyncCallback(DataReceived), callbackClient);
        }

        private void OnConnected(ClientData data)
        {

        }

        private void OnDisconnected(ClientData data)
        {

        }

        private void OnDataReceived(ClientData data)
        {
            byte[] packet = data.readCompleteData.ToArray();
            data.ResponseCount++;
            //data.channel.NonResponse = 0;
            data.readCompleteData.Clear();
            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //SetView(packet, data.channel);
            //}));
        }
    }


}
