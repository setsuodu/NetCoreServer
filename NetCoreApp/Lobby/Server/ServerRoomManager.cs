﻿using System.Diagnostics;
using System.Collections.Generic;

namespace Code.Server
{
    public class ServerRoomManager
    {
        const int MIN_INDEX = 1;
        protected Dictionary<int, ServerRoom> dic_rooms;
        public int Count => dic_rooms.Count;

        public ServerRoomManager()
        {
            dic_rooms = new Dictionary<int, ServerRoom>();
        }
    }
}