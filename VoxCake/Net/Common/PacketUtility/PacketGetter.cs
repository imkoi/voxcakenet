using System.Collections.Generic;

namespace VoxCake.Net.Common.Utility
{
	internal static class PacketGetter
	{
		internal static Packet[] GetPacketsByData(byte[] data, Protocol protocol, ulong playerID) //TODO: MAKE THIS METHOD READABLE
		{
			var size = data.Length;
			var index = 0;
			
			Packet packet;
			var packetID = 0;
			var packetSize = 0;
			var packets = new List<Packet>();
			byte[] packetData;
			
			while (index != size)
			{
				packet = protocol.GetPacketById(data[index]);
				packetSize = packet.Size;
				packetData = new byte[packetSize];
				
				for (var i = 0; i < packetSize; i++)
				{
					packetData[i] = data[index];
					index++;
				}
				
				packet.SetPlayer(playerID);
				packet.SetData(packetData);
				packets.Add(packet);
			}
			
			return packets.ToArray();
		}

		internal static int GetPacketIndexInCollection(Packet packet, List<Packet> collection)
		{
			var packetsCount = collection.Count;
			for (var index = 0; index < packetsCount; index++)
			{
				if (collection[index].GetType() == packet.GetType())
				{
					return index;
				}
			}
			return -1;
		}
	}
}