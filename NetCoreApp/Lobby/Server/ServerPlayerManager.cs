using System.Collections.Generic;

namespace NetCoreServer
{
    public class ServerPlayerManager
    {
        public ServerPlayerManager()
        {
            playerList = new List<ServerPlayer>();
        }

        private List<ServerPlayer> playerList;
        public int Count => playerList.Count;

        // ��¼�ɹ�
        public void AddPlayer(ServerPlayer player)
        {
            playerList.Add(player);
            player.ResetToLobby();
        }
        // �ǳ�/����/����
        public void RemovePlayer(int peerId)
        {
            var player = playerList.Find(x => x.PeerId == peerId);
            if (player == null) return;
            playerList.Remove(player);
        }
        // �ط�
        public void RemoveAll()
        {
            for (int i = playerList.Count - 1; i >= 0; i--)
            {
                playerList[i] = null;
                playerList.RemoveAt(i);
            }
        }

        // ��ȡָ�����
        public ServerPlayer GetPlayerByPeerId(short peerId)
        {
            var player = playerList.Find(x => x.PeerId == peerId);
            return player;
        }
        // ��ȡ�������
        public ServerPlayer[] GetPlayersAll()
        {
            return playerList.ToArray();
        }
        // ��ȡ���������
        public ServerPlayer[] GetPlayersByLobby()
        {
            return playerList.FindAll(x => x.Status == PlayerStatus.AtLobby).ToArray();
        }
    }
}