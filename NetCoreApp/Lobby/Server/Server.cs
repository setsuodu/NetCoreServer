namespace Code.Server
{
    public class Server
    {
        public ServerRoomManager m_RoomManager;
        public ServerPlayerManager m_PlayerManager; //长度为固定最大人数，不是时时在线。
    }
}