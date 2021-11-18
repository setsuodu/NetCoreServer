﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Diagnostics;
using LiteNetLib;

namespace LibSample
{
    public class WaitPeer
    {
        public IPEndPoint InternalAddr { get; }
        public IPEndPoint ExternalAddr { get; }
        public DateTime RefreshTime { get; private set; }

        public void Refresh()
        {
            RefreshTime = DateTime.UtcNow;
        }

        public WaitPeer(IPEndPoint internalAddr, IPEndPoint externalAddr)
        {
            Refresh();
            InternalAddr = internalAddr;
            ExternalAddr = externalAddr;
        }
    }

    public class HolePunchServerTest : IExample, INatPunchListener
    {
        private const int ServerPort = 50010;
        private const string ConnectionKey = "test_key";
        private static readonly TimeSpan KickTime = new TimeSpan(0, 0, 6);

        private readonly Dictionary<string, WaitPeer> _waitingPeers = new Dictionary<string, WaitPeer>();
        private readonly List<string> _peersToRemove = new List<string>();
        private NetManager _puncher;
        private NetManager _c1;
        private NetManager _c2;

        void INatPunchListener.OnNatIntroductionRequest(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, string token)
        {
            if (_waitingPeers.TryGetValue(token, out var wpeer))
            {
                if (wpeer.InternalAddr.Equals(localEndPoint) &&
                    wpeer.ExternalAddr.Equals(remoteEndPoint))
                {
                    wpeer.Refresh();
                    return;
                }

               Debug.Print("Wait peer found, sending introduction...");

                //found in list - introduce client and host to eachother
               Debug.Print(
                    "host - i({0}) e({1})\nclient - i({2}) e({3})",
                    wpeer.InternalAddr,
                    wpeer.ExternalAddr,
                    localEndPoint,
                    remoteEndPoint);

                _puncher.NatPunchModule.NatIntroduce(
                    wpeer.InternalAddr, // host internal
                    wpeer.ExternalAddr, // host external
                    localEndPoint, // client internal
                    remoteEndPoint, // client external
                    token // request token
                    );

                //Clear dictionary
                _waitingPeers.Remove(token);
            }
            else
            {
               Debug.Print("Wait peer created. i({0}) e({1})", localEndPoint, remoteEndPoint);
                _waitingPeers[token] = new WaitPeer(localEndPoint, remoteEndPoint);
            }
        }

        void INatPunchListener.OnNatIntroductionSuccess(IPEndPoint targetEndPoint, NatAddressType type, string token)
        {
            //Ignore we are server
        }

        public void Run()
        {
           Debug.Print("=== HolePunch Test ===");

            EventBasedNetListener clientListener = new EventBasedNetListener();
            EventBasedNatPunchListener natPunchListener1 = new EventBasedNatPunchListener();
            EventBasedNatPunchListener natPunchListener2 = new EventBasedNatPunchListener();

            clientListener.PeerConnectedEvent += peer =>
            {
               Debug.Print("PeerConnected: " + peer.EndPoint);
            };

            clientListener.ConnectionRequestEvent += request =>
            {
                request.AcceptIfKey(ConnectionKey);
            };

            clientListener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            {
               Debug.Print("PeerDisconnected: " + disconnectInfo.Reason);
                if (disconnectInfo.AdditionalData.AvailableBytes > 0)
                {
                   Debug.Print("Disconnect data: " + disconnectInfo.AdditionalData.GetInt());
                }
            };

            natPunchListener1.NatIntroductionSuccess += (point, addrType, token) =>
            {
                var peer = _c1.Connect(point, ConnectionKey);
               Debug.Print($"NatIntroductionSuccess C1. Connecting to C2: {point}, type: {addrType}, connection created: {peer != null}");
            };

            natPunchListener2.NatIntroductionSuccess += (point, addrType, token) =>
            {
                var peer = _c2.Connect(point, ConnectionKey);
               Debug.Print($"NatIntroductionSuccess C2. Connecting to C1: {point}, type: {addrType}, connection created: {peer != null}");
            };

            _c1 = new NetManager(clientListener)
            {
                IPv6Mode = IPv6Mode.DualMode,
                NatPunchEnabled = true
            };
            _c1.NatPunchModule.Init(natPunchListener1);
            _c1.Start();

            _c2 = new NetManager(clientListener)
            {
                IPv6Mode = IPv6Mode.DualMode,
                NatPunchEnabled = true
            };
            _c2.NatPunchModule.Init(natPunchListener2);
            _c2.Start();

            _puncher = new NetManager(clientListener)
            {
                IPv6Mode = IPv6Mode.DualMode,
                NatPunchEnabled = true
            };
            _puncher.Start(ServerPort);
            _puncher.NatPunchModule.Init(this);

            _c1.NatPunchModule.SendNatIntroduceRequest("localhost", ServerPort, "token1");
            _c2.NatPunchModule.SendNatIntroduceRequest("localhost", ServerPort, "token1");

           Debug.Print($"c1:{_c1.IsRunning}/{_c1.ConnectedPeersCount}/{_c1.ConnectedPeerList.Count}");

            // keep going until ESCAPE is pressed
           Debug.Print("Press ESC to quit");

            while (true)
            {
                //if (Console.KeyAvailable)
                //{
                //    var key = Console.ReadKey(true).Key;
                //    if (key == ConsoleKey.Escape)
                //    {
                //        break;
                //    }
                //    if (key == ConsoleKey.A)
                //    {
                //       Debug.Print("C1 stopped");
                //        _c1.DisconnectPeer(_c1.FirstPeer, new byte[] {1,2,3,4});
                //        _c1.Stop();
                //    }
                //}

                DateTime nowTime = DateTime.UtcNow;

                _c1.NatPunchModule.PollEvents();
                _c2.NatPunchModule.PollEvents();
                _puncher.NatPunchModule.PollEvents();
                _c1.PollEvents();
                _c2.PollEvents();

                //check old peers
                foreach (var waitPeer in _waitingPeers)
                {
                    if (nowTime - waitPeer.Value.RefreshTime > KickTime)
                    {
                        _peersToRemove.Add(waitPeer.Key);
                    }
                }

                //remove
                for (int i = 0; i < _peersToRemove.Count; i++)
                {
                   Debug.Print("Kicking peer: " + _peersToRemove[i]);
                    _waitingPeers.Remove(_peersToRemove[i]);
                }
                _peersToRemove.Clear();

                Thread.Sleep(10);
            }

            _c1.Stop();
            _c2.Stop();
            _puncher.Stop();
        }
    }
}
