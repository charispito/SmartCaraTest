using SmartCaraTest.data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
                //clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, new AsyncCallback(DataReceived), clientData);
                //clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, new AsyncCallback(ReceiveData), clientData);
                //clientData.client.GetStream().Read(new byte[19], 0, 19);
                clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, new AsyncCallback(DataReceived), clientData);
                clientDic.TryAdd(clientData.TimeMills, clientData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }        

        private void DataReceived(IAsyncResult ar)
        {
            ClientData client = ar.AsyncState as ClientData;
            try
            {
                int byteLength = client.client.GetStream().EndRead(ar);
                if (byteLength == 0)
                {
                    if (client.channel.ParameterMode)
                    {
                        client.client.GetStream().BeginRead(client.readByteParameterData, 0, client.readByteParameterData.Length, new AsyncCallback(DataReceived), client);
                    }
                    else
                    {
                        client.client.GetStream().BeginRead(client.readByteData, 0, client.readByteData.Length, new AsyncCallback(DataReceived), client);
                    }
                    return;
                }
                byte[] data = null;
                if (client.channel.ParameterMode)
                {
                    data = client.readByteParameterData;
                    client.readByteParameterDataCount += byteLength;
                }
                else
                {
                    data = client.readByteData;
                    client.readByteDataCount += byteLength;
                }
                if(data.Length == 57 && data[0] == 0xCC && data[3] == 57 && data[56] == 0xEF)
                {
                    receiveStateData(client, byteLength, data);
                    Array.Clear(client.readByteData, 0, byteLength);
                    client.client.GetStream().BeginRead(client.readByteData, 0, 57, new AsyncCallback(DataReceived), client);
                }
                else if(data.Length == 70 && data[0] == 0xCC && data[3] == 70 && data[69] == 0xEF)
                {
                    receiveParameter(client, byteLength, data);
                    Array.Clear(client.readByteParameterData, 0, byteLength);
                    client.client.GetStream().BeginRead(client.readByteParameterData, 0, 70, new AsyncCallback(DataReceived), client);
                }
                else
                {
                    if (client.channel.ParameterMode)
                    {
                        receiveParameter(client, byteLength, data);
                        Array.Clear(client.readByteParameterData, 0, byteLength);
                        if(byteLength > 70)
                        {
                            Array.Clear(client.readByteParameterData, 0, 70);
                            client.readByteParameterDataCount = 0;
                            client.client.GetStream().BeginRead(client.readByteParameterData, 0, 70, new AsyncCallback(DataReceived), client);
                        }
                        else
                        {
                            client.client.GetStream().BeginRead(client.readByteParameterData, 0, 70 - client.readParameterData.Count, new AsyncCallback(DataReceived), client);
                        }
                    }
                    else
                    {

                        receiveStateData(client, byteLength, data);
                        Array.Clear(client.readByteData, 0, byteLength);
                        if(byteLength > 57)
                        {
                            Array.Clear(client.readByteData, 0, 57);
                            client.readByteDataCount = 0;
                            client.client.GetStream().BeginRead(client.readByteData, 0, 57, new AsyncCallback(DataReceived), client);
                        }
                        else
                        {
                            client.client.GetStream().BeginRead(client.readByteData, 0, 57 - client.readCompleteData.Count, new AsyncCallback(DataReceived), client);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("array length:{0} received: {1}", client.readByteParameterData.Length, client.readByteParameterDataCount);
                Console.WriteLine("array length:{0} received: {1}", client.readByteData.Length, client.readByteDataCount);
                RemoveClient(client);
            }
        }

        private void receiveStateData(ClientData client, int byteLength, byte[] data)
        {
            byte[] arr = data.Slice(byteLength);
            Console.WriteLine("State Time: {0}, Length:{1}, Data: {2} \n", DateTime.Now, byteLength, byteToString(arr));
            if (byteLength == 2 && data[0] == 0x0D && data[1] == 0x0A)
            {
                client.client.GetStream().BeginRead(client.readByteData, 0, client.readByteData.Length, new AsyncCallback(DataReceived), client);
                return;
            }
            else if (byteLength == 19 && data[0] == 02 && data[18] == 0x0A)
            {
                //string hex = "";
                //for (int i = 0; i < byteLength; i++)
                //{
                //    hex += " " + data[i].ToString("X2");
                //}
                //byte[] slice = data.Slice(byteLength - 2);
                //string res = Encoding.UTF8.GetString(slice, 0, slice.Length);
            }
            else
            {
                if (byteLength == 57)
                {
                    if (client.readCompleteData == null)
                        client.readCompleteData = new List<byte>();
                    client.readCompleteData.Clear();
                    
                    //byte[] arr = data.Slice(57);
                    client.readCompleteData.AddRange(arr);
                    OnReceived(client);
                }
                else
                {
                    if (client.readCompleteData == null)
                        client.readCompleteData = new List<byte>();
                    //byte[] arr = data.Slice(byteLength);
                    client.readCompleteData.AddRange(arr);
                    if (client.readCompleteData.Count == 57)
                    {
                        OnReceived(client);
                    }
                    else
                    {
                        if (client.readCompleteData.Count > 57)
                        {
                            if (client.readCompleteData[0] == 0xCC && client.readCompleteData[56] == 0xEF)
                            {
                                byte[] arr2 = client.readCompleteData.ToArray().Slice(57);
                                client.readCompleteData.Clear();
                                client.readCompleteData.AddRange(arr2);
                                OnReceived(client);
                            }
                            else
                            {
                                client.readCompleteData.Clear();
                            }
                        }

                    }
                }
            }
        }

        private void receiveParameter(ClientData client, int byteLength, byte[] data)
        {
            byte[] arr = data.Slice(byteLength);
            Console.WriteLine("Parameter Time: {0}, Length:{1}, Data: {2} \n", DateTime.Now, byteLength, byteToString(arr));
            if (byteLength == 70)
            {
                if (client.readParameterData == null)
                    client.readParameterData = new List<byte>();
                client.readParameterData.Clear();
                //byte[] arr = data.Slice(70);

                client.readParameterData.AddRange(arr);
                if (arr[68] == Protocol.GetCheckSum(arr, 1, 67))
                {
                    OnReceived(client);
                }
                else
                {
                    Console.WriteLine("CheckSum Error Received: {0}, Calculated: {1}", arr[68], Protocol.GetCheckSum(arr, 1, 67));
                    client.readParameterData.Clear();
                }
                //OnReceived(client);
            }
            else
            {
                if (client.readParameterData == null)
                    client.readParameterData = new List<byte>();
                //byte[] arr = data.Slice(byteLength);
                client.readParameterData.AddRange(arr);
                if (client.readParameterData.Count == 70)
                {
                    if (client.readParameterData[0] == 0xCC && client.readParameterData[3] == 70 && client.readParameterData[69] == 0xEF)
                    {
                        byte[] arr2 = client.readParameterData.ToArray();
                        if (arr2[68] == Protocol.GetCheckSum(arr2, 1, 67))
                        {
                            OnReceived(client);
                        }
                        else
                        {
                            Console.WriteLine("CheckSum Error Received: {0}, Calculated: {1}", arr2[68], Protocol.GetCheckSum(arr2, 1, 67));
                            client.readParameterData.Clear();
                        }
                        //OnReceived(client);
                    }                        
                }
                else
                {
                    if (client.readParameterData.Count > 70)
                    {
                        if (client.readParameterData[0] == 0xCC && client.readParameterData[3] == 70 && client.readParameterData[69] == 0xEF)
                        {
                            byte[] arr2 = client.readParameterData.ToArray().Slice(70);
                            client.readParameterData.Clear();
                            client.readParameterData.AddRange(arr2);
                            if(arr2[68] == Protocol.GetCheckSum(arr2,1, 67))
                            {
                                OnReceived(client);
                            }
                            else
                            {
                                Console.WriteLine("CheckSum Error Received: {0}, Calculated: {1}", arr2[68], Protocol.GetCheckSum(arr2, 1, 67));
                                client.readParameterData.Clear();
                            }
                            //Console.WriteLine($"Set From : {client.clientNumber} ReceiveCount : {client.ResponseCount}");
                            
                        }
                        else
                        {
                            Console.WriteLine("Protocol Error");
                            Console.WriteLine("Parameter Time: {0}, Length:{1}, Data: {2} \n", DateTime.Now, byteLength, byteToString(client.readParameterData.ToArray()));
                            client.readParameterData.Clear();
                        }
                    }

                }
            }
        }

        private void RemoveClient(ClientData targetClient)
        {
            ClientData client = null;
            bool remove = clientDic.TryRemove(targetClient.TimeMills, out client);
            if (remove)
            {
                OnDisconnected(targetClient);
            }
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
    }
}
