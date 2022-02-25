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

        public void AddClient(TcpClient newClient)
        {
            ClientData clientData = new ClientData(newClient);
            OnConnected(clientData);
            try
            {
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
                    client.client.GetStream().BeginRead(client.readByteData, 0, client.readByteData.Length, new AsyncCallback(DataReceived), client);
                    return;
                }
                byte[] data = client.readByteData;
                if (byteLength == 2 && data[0] == 0x0D && data[1] == 0x0A)
                {
                    client.client.GetStream().BeginRead(client.readByteData, 0, client.readByteData.Length, new AsyncCallback(DataReceived), client);
                    return;
                }
                else if (byteLength == 19 && data[0] == 02 && data[18] == 0x0A)
                {
                    string hex = "";
                    for (int i = 0; i < byteLength; i++)
                    {
                        hex += " " + data[i].ToString("X2");
                    }
                    byte[] slice = data.Slice(byteLength - 2);
                    string res = Encoding.UTF8.GetString(slice, 0, slice.Length);
                    //Console.WriteLine($"hex : {hex}\nmessage : {res}");
                }
                else
                {
                    if (byteLength == 57)
                    {
                        if (client.readCompleteData == null)
                            client.readCompleteData = new List<byte>();
                        client.readCompleteData.Clear();
                        byte[] arr = data.Slice(57);
                        client.readCompleteData.AddRange(arr);
                        OnReceived(client);
                    }
                    else
                    {
                        if (client.readCompleteData == null)
                            client.readCompleteData = new List<byte>();
                        byte[] arr = data.Slice(byteLength);
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
                                    //Console.WriteLine($"Set From : {client.clientNumber} ReceiveCount : {client.ResponseCount}");
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

                client.client.GetStream().BeginRead(client.readByteData, 0, client.readByteData.Length, new AsyncCallback(DataReceived), client);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                RemoveClient(client);
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
    }
}
