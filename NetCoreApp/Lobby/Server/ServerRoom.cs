using System.Collections.Generic;
using Code.Shared;

namespace Code.Server
{
    /* 远程房间 */
    public class ServerRoom : BaseRoom
    {
        #region 房间数据

        public ServerRoom(int roomId, ServerPlayer host, ServerPlayer guest) : base(roomId, host, guest)
        {
            m_PlayerList = new ServerPlayer[] { host, guest };
        }

        public override BasePlayer[] m_PlayerList { get; protected set; }
        public override void Dispose() { }
        public ServerPlayer GetOtherPlayer(short peerId)
        {
            if (m_PlayerList[0].PeerId == peerId && m_PlayerList[1].PeerId != peerId)
            {
                return m_PlayerList[1] as ServerPlayer;
            }
            else if (m_PlayerList[0].PeerId != peerId && m_PlayerList[1].PeerId == peerId)
            {
                return m_PlayerList[0] as ServerPlayer;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}