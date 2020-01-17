using System;
using System.Collections.Generic;
using UnityEngine;
using VoxCake.Net.Common.Utility;

namespace VoxCake.Net
{
	/// <summary>
	/// Example of usage - 
	/// </summary>
	public abstract class Protocol : IDisposable
	{
		public byte Size { get; private set; }
		private const string I_BINDABLE_PACKET = "IBindablePacket";
		
		private List<Packet> _packetCollection;
		private Packet[] _packets;
		protected abstract void Bindings();

		protected Protocol()
		{
			_packetCollection = new List<Packet>();
			
			Bindings();
			
			_packets = _packetCollection.ToArray();
			_packetCollection = null;
		}

		protected void BindPacket<T>(SendType sendType) where T : Packet, new()
		{
			var packet = new T();
			
#if VOXCAKE_NET_DEBUG
			if (!PacketExists(packet))
			{
#endif
				packet.SetSendType(sendType);
				
				var packetType = packet.GetType();
				if (packetType.GetInterface(I_BINDABLE_PACKET) != null)
				{
#if VOXCAKE_NET_DEBUG
					Debug.Log($"Binding to {packetType.Name}");
#endif
					PacketBinder.BindVariablesToPacket(packet, this);
				}
#if VOXCAKE_NET_DEBUG
				Debug.Log($"{packetType.Name}.Size = {packet.Size}");
#endif
				_packetCollection.Add(packet);
#if VOXCAKE_NET_DEBUG
			}
			else
			{
				Debug.LogError("Packet already exist");
			}
#endif
			
			Size += 1;
		}

		internal Packet GetPacketById(int id)
		{
			return _packets[id];
		}

		internal Packet GetPacketByType(Type packetType)
		{
			var count = _packets.Length;
			for (var i = 0; i < count; i++)
			{
				var packet = _packets[i];
				if (_packets[i].GetType() == packetType)
				{
					return packet;
				}
			}
			
			throw new Exception($"Packet \"{packetType.Name}\" not binded to Protocol \"{GetType().Name}\"");
		}

		private bool PacketExists(Packet packet)
		{
			var count = _packetCollection.Count;
			for(var i = 1; i < count; i++)
			{
				var packetType = _packetCollection[i].GetType();
				if (packet.GetType() == packetType)
				{
					return true;
				}
			}
			return false;
		}

		public void Dispose()
		{
			_packetCollection = null;
			_packets = null;
		}
	}
}
