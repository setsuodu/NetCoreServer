using Orleans;

namespace IGrains
{
    // 属于网关服
    public interface IPacketObserver : IGrainObserver
    {
        void OnReceivePacket(NetPacket packet);
    }
}