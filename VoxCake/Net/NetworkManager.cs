using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Steamworks;
using VoxCake.Net.Common.Utility;

namespace VoxCake.Net
{
	/// <summary>
	/// Class description
	/// </summary>
	public class NetworkManager : IDisposable
	{
		public const int MAX_UNRELIABLE_PACKET_SIZE = 255;
		public const int MAX_RELIABLE_PACKET_SIZE = 255;
		
		public uint AppID { get; private set; }
		public Client Client { get; private set; }
		public readonly int TickRate;
		private readonly int _sendingDelay;
		public event Action<string, Action, Action> OnSessionRequest;
		public event Action<ApiInitializationErrorType> OnInitializationError;
		
		private List<Packet> _unreliablePackets;
		private List<Packet> _reliablePackets;
		private uint _lastSeesionRequestID;
		private Protocol _protocol;
		private Thread _packetSenderThread;

		public NetworkManager(uint appid, int tickRate = 10)
		{
			TickRate = tickRate;
			AppID = appid;
			_sendingDelay = 1000 / tickRate;
			
			Client = new Client(appid);
			
			_unreliablePackets = new List<Packet>();
			_reliablePackets = new List<Packet>();
			
			SteamNetworking.OnP2PSessionRequest += SessionRequest;
			Client.OnInitializationError += InitializationError;
			
			_packetSenderThread = new Thread(PacketSenderJob);
			_packetSenderThread.Start();
		}

		/// <summary>
		/// DO NOT BIND PROTOCOLS AT RUNTIME!
		/// Binding of protocols is expensive operation, you really should do it on loading screens :)
		/// </summary>
		public void BindProtocol<T>() where T : Protocol, new()
		{
			_protocol?.Dispose();
			_protocol = new T();
#if VOXCAKE_NET_DEBUG
			Debug.Log($"Protocol \"{_protocol.GetType().Name}\" binded to {GetType().Name}");
#endif
		}

		/// <summary>
		/// Use this method to send packets to other clients
		/// WARNING! Be careful in sending packets with variables, be sure that you pass the right type values 
		/// </summary>
		public void SendPacket<T>(params object[] packetVariables) where T : Packet
		{
			var packet = _protocol.GetPacketByType(typeof(T));
			packet.SetPlayer(Client.steamID);
			
			if (packetVariables.Length > 0)
			{
				PacketBinder.SetVariablesToPacket(packet, packetVariables);
			}
			
			PacketExecutor.ExecutePacket(packet, true);
			
			switch (packet.SendType)
			{
				case SendType.Unreliable:
					PacketSetter.SetPacketToCollection(packet, _unreliablePackets);
					break;
				case SendType.Reliable:
					PacketSetter.SetPacketToCollection(packet, _reliablePackets);
					break;
			}
		}

		/// <summary>
		/// Use this method to recieve packets from other clients
		/// </summary>
		public void ReadPackets()
		{
			if (SteamNetworking.IsP2PPacketAvailable())
			{
				var nullableP2Packet = SteamNetworking.ReadP2PPacket();
				if (nullableP2Packet != null)
				{
					var p2PPacket = nullableP2Packet.Value;
					var playerId = p2PPacket.SteamId.Value;
					if (Client.steamID != playerId)
					{
						var packetData = p2PPacket.Data;
						var packetsToExecute = PacketGetter.GetPacketsByData(packetData, _protocol, playerId);
						var packetsCount = packetsToExecute.Length;
						for (var i = 0; i < packetsCount; i++)
						{
							PacketExecutor.ExecutePacket(packetsToExecute[i], false);
						}
					}
				}
			}
		}

		private void PacketSenderJob() //TODO: Calculate processor ticks of packet merging and recalculate sendDelay
		{
			while (true)
			{
				if (_unreliablePackets.Count > 0)
				{
					var packetData = PacketMerger.GetMergedPacketData(_unreliablePackets, MAX_UNRELIABLE_PACKET_SIZE);
					SteamNetworking.SendP2PPacket(SteamClient.SteamId,
						packetData,
						packetData.Length,
						0,
						P2PSend.UnreliableNoDelay);
					_unreliablePackets.Clear();
				}
				
				if (_reliablePackets.Count > 0)
				{
					var packetData = PacketMerger.GetMergedPacketData(_reliablePackets, MAX_RELIABLE_PACKET_SIZE);
					SteamNetworking.SendP2PPacket(SteamClient.SteamId,
						packetData,
						packetData.Length);
					_reliablePackets.Clear();
				}
				
				Thread.Sleep(_sendingDelay);
			}
		}

		private void SessionRequest(SteamId steamID)
		{
			var accountID = steamID.AccountId;
			var name = string.Empty;
			foreach (var friend in SteamFriends.GetFriends())
			{
				if (friend.Id == steamID)
				{
					name = friend.Name;
				}
			}
			
			OnSessionRequest.Invoke(name, AcceptSessionRequest, DeclineSessionRequest);
		}

		private void InitializationError(ApiInitializationErrorType errorType)
		{
			OnInitializationError.Invoke(errorType);
		}

		private void AcceptSessionRequest()
		{
			SteamNetworking.AcceptP2PSessionWithUser(_lastSeesionRequestID);
		}

		private void DeclineSessionRequest()
		{
			_lastSeesionRequestID = 0;
		}

		public void Dispose()
		{
			SteamNetworking.OnP2PSessionRequest -= SessionRequest;
			Client.OnInitializationError -= InitializationError;
			
			OnSessionRequest = null;
			OnInitializationError = null; 
			
			_unreliablePackets = null;
			_reliablePackets = null;
			_protocol = null;
			_packetSenderThread.Abort();
			_packetSenderThread = null;
			Client.Dispose();
		}
	}
}