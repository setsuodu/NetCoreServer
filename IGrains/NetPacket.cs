namespace IGrains
{
    /// <summary>
    /// 网关和逻辑服之间的消息格式
    /// </summary>
    public class NetPacket
    {
        public int ProtoID;

        public byte[] bodyData;
    }
}