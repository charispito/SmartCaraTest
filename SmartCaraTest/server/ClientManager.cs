using SmartCaraTest.data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCaraTest.util
{
    public class ClientManager
    {
        public static ConcurrentDictionary<long, ClientData> clientDic = new ConcurrentDictionary<long, ClientData>();
        public delegate void ConnectionHandler(ClientData clientSocket);
        public ConnectionHandler OnConnected;
        public ConnectionHandler OnDisconnected;
        public delegate void DataReceiveHandler(ClientData client);
        public DataReceiveHandler OnReceived;
        public bool parameter = false;

        public void AddClient(TcpClient newClient)
        {
            Console.WriteLine("connected");
            ClientData clientData = new ClientData(newClient);
            OnConnected(clientData);
            try
            {
                clientData.Run = true;
                Thread check = new Thread(new ParameterizedThreadStart(CheckClient));
                check.Start(clientData);
                Thread read = new Thread(new ParameterizedThreadStart(asyncReadAsync));

                read.Start(clientData);

                //clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, new AsyncCallback(DataReceived), clientData);
                clientDic.TryAdd(clientData.TimeMills, clientData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void asyncReadAsync(object obj)
        {
            ClientData clientdata = obj as ClientData;
            while (clientdata.Run)
            {
                try
                {
                    int result = await clientdata.client.GetStream().ReadAsync(clientdata.readByteParameterData, 0, 70);
                    byte[] slice = clientdata.readByteParameterData.Slice(result);
                    Array.Clear(clientdata.readByteParameterData, 0, result);
                    if (clientdata.channel.run)
                    {
                        Console.WriteLine("ALL: {0}, Length: {1} Data: {2}", clientdata.readCompleteData.Count, result, slice.byteToString());
                    }
                    if (result > 0)
                    {
                        if (clientdata.readCompleteData.Count == 0)
                        {
                            if (clientdata.channel.IsNewVersion)
                            {
                                if (slice[0] != 0x12)
                                {
                                    Console.WriteLine("Head Error {0}", slice.byteToString());
                                }
                                else
                                {
                                    clientdata.readCompleteData.AddRange(slice);
                                    if (slice.Last() == 0x34)
                                    {
                                        if (slice.Length > 3 && clientdata.readCompleteData[3] == clientdata.readCompleteData.Count)
                                        {
                                            //완료
                                            //Console.WriteLine("complete1: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                            clientdata.channel.Dispatcher.Invoke(new Action(() => {
                                                switch (clientdata.readCompleteData[2])
                                                {
                                                    case 0xAA:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0x99:
                                                        clientdata.channel.ParameterWindow.setParameter(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xB9:
                                                        clientdata.channel.ParameterWindow.setError(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xA0:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                }
                                                clientdata.ResponseCount++;
                                                clientdata.channel.NonResponse = 0;
                                                clientdata.readCompleteData.Clear();
                                            }));
                                        }
                                        else
                                        {
                                            if (clientdata.readCompleteData.Count > 70)
                                                clientdata.readCompleteData.Clear();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (slice[0] != 0xCC)
                                {
                                    Console.WriteLine("Head Error {0}", slice.byteToString());
                                }
                                else
                                {
                                    clientdata.readCompleteData.AddRange(slice);
                                    if (slice.Last() == 0xEF)
                                    {
                                        if (slice.Length > 3 && clientdata.readCompleteData[3] == clientdata.readCompleteData.Count)
                                        {
                                            //완료
                                            //Console.WriteLine("complete2: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                            clientdata.channel.Dispatcher.Invoke(new Action(() => {
                                                switch (clientdata.readCompleteData[2])
                                                {
                                                    case 0xAA:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0x99:
                                                        clientdata.channel.ParameterWindow.setParameter(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xB9:
                                                        clientdata.channel.ParameterWindow.setError(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xA0:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                }
                                                clientdata.ResponseCount++;
                                                clientdata.channel.NonResponse = 0;
                                                clientdata.readCompleteData.Clear();
                                            }));
                                        }
                                        else
                                        {
                                            if (clientdata.readCompleteData.Count > 70)
                                                clientdata.readCompleteData.Clear();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (clientdata.channel.IsNewVersion)
                            {
                                if (clientdata.readCompleteData[0] != 0x12)
                                {
                                    Console.WriteLine("Head Error: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                    clientdata.readCompleteData.Clear();
                                }
                                else
                                {

                                    clientdata.readCompleteData.AddRange(slice);
                                    if (clientdata.readCompleteData.Count > 3 && clientdata.readCompleteData[3] == clientdata.readCompleteData.Count)
                                    {
                                        if (clientdata.readCompleteData.Last() == 0x34)
                                        {
                                            //완료
                                            //Console.WriteLine("complete3: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                            clientdata.channel.Dispatcher.Invoke(new Action(() => {
                                                switch (clientdata.readCompleteData[2])
                                                {
                                                    case 0xAA:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0x99:
                                                        clientdata.channel.ParameterWindow.setParameter(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xB9:
                                                        clientdata.channel.ParameterWindow.setError(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xA0:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                }
                                                clientdata.ResponseCount++;
                                                clientdata.channel.NonResponse = 0;
                                                clientdata.readCompleteData.Clear();
                                            }));
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                            clientdata.readCompleteData.Clear();
                                        }
                                    }
                                    else
                                    {
                                        if (clientdata.readCompleteData.Count > 70)
                                            clientdata.readCompleteData.Clear();
                                    }
                                }
                            }
                            else
                            {
                                if (clientdata.readCompleteData[0] != 0xCC)
                                {
                                    Console.WriteLine("Head Error: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                    clientdata.readCompleteData.Clear();
                                }
                                else
                                {

                                    clientdata.readCompleteData.AddRange(slice);
                                    if (clientdata.readCompleteData.Count > 3 && clientdata.readCompleteData[3] == clientdata.readCompleteData.Count)
                                    {
                                        if (clientdata.readCompleteData.Last() == 0xEF)
                                        {
                                            //완료
                                            //Console.WriteLine("complete4: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                            clientdata.channel.Dispatcher.Invoke(new Action(() => {
                                                switch (clientdata.readCompleteData[2])
                                                {
                                                    case 0xAA:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0x99:
                                                        clientdata.channel.ParameterWindow.setParameter(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xB9:
                                                        clientdata.channel.ParameterWindow.setError(clientdata.readCompleteData.ToArray());
                                                        break;
                                                    case 0xA0:
                                                        clientdata.channel.SetView(clientdata.readCompleteData.ToArray());
                                                        break;
                                                }
                                                clientdata.ResponseCount++;
                                                clientdata.channel.NonResponse = 0;
                                                clientdata.readCompleteData.Clear();
                                            }));
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error: {0}", clientdata.readCompleteData.ToArray().byteToString());
                                            clientdata.readCompleteData.Clear();
                                        }
                                    }
                                    else
                                    {
                                        if (clientdata.readCompleteData.Count > 70)
                                            clientdata.readCompleteData.Clear();
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    RemoveClient(clientdata);
                }
                
            }
        }

        private void CheckClient(Object obj)
        {
            ClientData data = obj as ClientData;
            while (data.Run)
            {
                if (data.channel.ParameterMode)
                {
                    if(data.channel.ParameterCount < 6)
                    {
                        byte[] command = Protocol.GetParameter(data.channel.IsNewVersion);
                        Console.WriteLine("[{0}]write: " + command.byteToString(), DateTime.Now.ToString("HH:mm:ss"));
                        if (data.client.Connected)
                        {
                            data.client.GetStream().Write(command, 0, command.Length);
                            data.channel.ParameterCount++;
                        }
                        else
                        {
                            RemoveClient(data);
                        }
                    }                    
                    Thread.Sleep(1000);
                    continue;
                }
                try
                {
                    byte[] command = null;
                    if (data.channel.IsNewVersion)
                    {
                        command = Protocol.GetNewCommand(1);
                    }
                    else
                    {
                        command = Protocol.GetCommand(1);
                    }
                    Console.WriteLine("[{0}]write: " + command.byteToString(), DateTime.Now.ToString("HH:mm:ss"));
                    if (data.client.Connected)
                    {
                        data.client.GetStream().Write(command, 0, command.Length);
                    }
                    else
                    {
                        RemoveClient(data);
                    }
                    //if (!data.channel.Response)
                    //{
                    //    data.channel.NonResponse++;
                    //    if (data.channel.NonResponse > 20)
                    //    {
                    //        data.channel.Dispatcher.BeginInvoke(new Action(() =>
                    //        {
                    //            data.channel.Item23.cont.Content = "응답없음";
                    //        }));
                    //    }
                    //}
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    data.Run = false;
                    RemoveClient(data);
                }
                Thread.Sleep(1000);
            }
        }

        private void RemoveClient(ClientData targetClient)
        {
            ClientData client = null;
            targetClient.Run = false;
            bool remove = clientDic.TryRemove(targetClient.TimeMills, out client);
            if (remove)
            {
                OnDisconnected(targetClient);
            }
        }
    }
}
