using TcpChatServer;
using HotFix;
using ET;

namespace NetCoreServer
{
    /* Զ���û� */
    public class ServerPlayer : BasePlayer
    {
        public readonly TcpSession Session; //����ֱ��Send��Ϣ

        public ServerPlayer(string userName, System.Guid peerid) : base(userName, peerid)
        {
            Session = TCPChatServer.server.FindSession(PeerId);
        }

        public void SendAsync(PacketType msgId, object cmd)
        {
            byte[] header = new byte[1] { (byte)msgId };
            //byte[] body = ProtobufferTool.Serialize(cmd);
            byte[] body = ProtobufHelper.ToBytes(cmd);
            byte[] buffer = new byte[header.Length + body.Length];
            System.Array.Copy(header, 0, buffer, 0, header.Length);
            System.Array.Copy(body, 0, buffer, header.Length, body.Length);
            Session.SendAsync(buffer);
        }
    }
}