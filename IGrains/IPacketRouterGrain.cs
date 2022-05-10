using System.Threading.Tasks;
using Orleans;

namespace IGrains
{
    // 属于逻辑服
    public interface IPacketRouterGrain : IGrainWithIntegerKey //Actor对象需继承IGrainWithXXX基类
    {
        Task OnReceivePacket(NetPacket packet); //从网关接收数据

        Task BindPacketObserver(IPacketObserver observer); //通过观察者向网关发数据
    }
}