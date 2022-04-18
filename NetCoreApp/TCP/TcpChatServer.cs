using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using NetCoreServer;

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
            string message = "Hello from TCP chat! Please send a message or '!' to disconnect the client!";
            SendAsync(message);
        }

        protected override void OnDisconnected()
        {
            Debug.Print($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Debug.Print($"C2S: {message}({size})");

            // Multicast message to all connected sessions
            Server.Multicast(message);
            //SendAsync(message);

            // If the buffer starts with '!' the disconnect the current session
            //if (message == "!")
            //    Disconnect();

            // 解析msgId
            byte msgId = buffer[0];
            byte[] body = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, body, 0, buffer.Length - 1);

            PacketType type = (PacketType)msgId;
            Debug.Print($"msgId={msgId}");

            switch (type)
            {
                case PacketType.C2S_LoginReq:
                    HotFix.TheMsg msg = ProtobufferTool.Deserialize<HotFix.TheMsg>(body);
                    Debug.Print($"[{type}] Name={msg.Name}, Content={msg.Content}");
                    break;
                case PacketType.C2S_MatchRequest:
                    break;
            }
            //TODO: 通过委托分发出去
        }

        protected override void OnError(SocketError error)
        {
            Debug.Print($"Chat TCP session caught an error with code {error}");
        }
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
        protected static ChatServer server;

        public static void Run()
        {
            // TCP server port
            int port = 1111;

            Debug.Print($"TCP server port: {port}");

            // Create a new TCP chat server
            server = new ChatServer(IPAddress.Any, port);

            // Start the server
            Debug.Print("Server starting...");
            server.Start();
            Debug.Print("Done!");

            Debug.Print("Press Enter to stop the server or '!' to restart the server...");

            /*
            // Perform text input
            for (;;)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // Restart the server
                if (line == "!")
                {
                    Debug.Print("Server restarting...");
                    server.Restart();
                    Debug.Print("Done!");
                    continue;
                }

                // Multicast admin message to all sessions
                line = "(admin) " + line;
                server.Multicast(line);
            }

            // Stop the server
            Debug.Print("Server stopping...");
            server.Stop();
            Debug.Print("Done!");
            */
        }

        public static void Stop()
        {
            // Stop the server
            Debug.Print("Server stopping...");
            server.Stop();
            Debug.Print("Done!");
        }
    }
}