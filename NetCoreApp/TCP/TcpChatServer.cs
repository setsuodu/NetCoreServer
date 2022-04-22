using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using NetCoreServer;
using ET;

namespace TcpChatServer
{
    /* 一个客户端连接单位 */
    public class ChatSession : TcpSession
    {
        public ChatSession(TcpServer server) : base(server) {}

        protected override void OnConnected()
        {
            Debug.Print($"Chat TCP session with Id {Id} connected!");

            // Send invite message
            //string message = "Hello from TCP chat! Please send a message or '!' to disconnect the client!";
            //SendAsync(message);

            SendAsync("hello, you're connected");
        }

        protected override void OnDisconnected()
        {
            Debug.Print($"Chat TCP session with Id {Id} disconnected!");

            TCPChatServer.m_PlayerManager.RemovePlayer(Id);
        }

        // 注意这里是线程中
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            //Debug.Print($"OnReceived: length={buffer.Length}, offset={offset}, size={size}");
            //byte[] realBuffer = new byte[size];
            //Array.Copy(buffer, 0, realBuffer, 0, size);
            Array.Resize<byte>(ref buffer, (int)size); //8192裁剪

            //Debug.Print($"测试TheMsgList解析");
            //FileHelper.WriteBytes(buffer);
            //try
            //{
            //    byte[] clientData = buffer;
            //    Debug.Print($"clientData: length={clientData.Length}");
            //    MemoryStream ms = new MemoryStream(clientData, 0, clientData.Length);
            //    var obj = ProtobufHelper.FromStream(typeof(TheMsgList), ms) as TheMsgList;
            //    Debug.Print($"反序列化: obj={obj.Content[1]}");
            //}
            //catch (Exception e)
            //{
            //    Debug.Print($"error: {e.ToString()}");
            //}
            //return;

            // 解析msgId
            byte msgId = buffer[0];
            byte[] body = new byte[size - 1];
            Array.Copy(buffer, 1, body, 0, size - 1);
            PacketType type = (PacketType)msgId;
            Debug.Print($"msgType={type}, from {Id}");

            switch (type)
            {
                //case PacketType.Connected:
                //    break;
                case PacketType.C2S_LoginReq:
                    OnLoginReq(body);
                    break;
                //case PacketType.C2S_Chat:
                //    OnChat(body);
                //    break;
            }
        }

        protected override void OnError(SocketError error)
        {
            Debug.Print($"Chat TCP session caught an error with code {error}");
        }

        protected async void OnLoginReq(byte[] body)
        {
            Debug.Print($"OnLoginReq: length={body.Length}");
            MemoryStream ms = new MemoryStream(body, 0, body.Length);
            var obj = ProtobufHelper.FromStream(typeof(TheMsgList), ms) as TheMsgList;
            Debug.Print($"反序列化: Id={obj.Id}, count={obj.Content.Count}");
        }
        protected void OnChat(byte[] body) { }
    }

    public class ChatServer : TcpServer
    {
        public ChatServer(IPAddress address, int port) : base(address, port) {}

        protected override TcpSession CreateSession() { return new ChatSession(this); }

        protected override void OnError(SocketError error)
        {
            Debug.Print($"Chat TCP server caught an error with code {error}");
        }
    }

    public class TCPChatServer
    {
        protected const int port = 1111;
        public static ChatServer server;

        public static ServerRoomManager m_RoomManager;
        public static ServerPlayerManager m_PlayerManager;

        public static void Run()
        {
            m_RoomManager = new ServerRoomManager();
            m_PlayerManager = new ServerPlayerManager();
            Debug.Print("TCPChatServer Init...");

            // Create a new TCP chat server
            server = new ChatServer(IPAddress.Any, port);

            // Start the server
            Debug.Print("Server starting...");
            server.Start();
            Debug.Print("Done!");
        }
        public static void Stop()
        {
            // Stop the server
            Debug.Print("Server stopping...");
            server?.Stop();
            Debug.Print("Done!");
        }
        public static void Restart()
        {
            // Restart the server
            Debug.Print("Server restarting...");
            server.Restart();
            Debug.Print("Done!");
        }
    }
}