using System;
using System.Threading.Tasks;
using IGrains;

namespace Grains
{
    public class PacketRouterGrain : Orleans.Grain, IPacketRouterGrain
    {
        private IPacketObserver observer;

        public Task BindPacketObserver(IPacketObserver observer)
        {
            this.observer = observer;

            return Task.CompletedTask;
        }

        // 网关服务器调用绑定（通过IPacketRouterGrain接口远程调用RPC）
        public Task OnReceivePacket(NetPacket packet)
        {
            // 当前Grain的key
            long id = GrainReference.GrainIdentity.PrimaryKeyLong;

            Console.WriteLine($"LogicServer {id} 收到消息");

            // 将消息发回客户端

            //if (packet.ProtoID == (int)/*Proto消息号*/)
            //{
            //  反序列化 IMessage

            // 将消息再发回网关服务器
            //observer.OnReceivePacket(packet);
            //Console.WriteLine($"LogicServer {id} 发送消息");
            //}

            return Task.CompletedTask;
        }
    }
}