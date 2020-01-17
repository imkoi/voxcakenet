using System.Collections.Generic;

namespace VoxCake.Net.Common.Utility
{
	internal static class PacketMerger
	{
		internal static byte[] GetMergedPacketData(List<Packet> packets, int packetSizeLimit)
		{
			var packetIndex = 0;
			var resultedPacketSize = 0;
			var packetsToMerge = new List<Packet>();
			var packetsCount = packets.Count;
			Packet packet;
			
			while (packetSizeLimit > 0 && packetIndex < packetsCount)
			{
				packet = packets[packetIndex];
				if (packet.Size <= packetSizeLimit)
				{
					packetsToMerge.Add(packet);
					resultedPacketSize += packet.Size;
					packetSizeLimit -= packet.Size;
				}
				packetIndex++;
			}
			
			return GetMergedDataFromPackets(packetsToMerge, resultedPacketSize);
		}

		private static byte[] GetMergedDataFromPackets(List<Packet> packets, int resultedPacketSize) //TODO: MAKE THIS METHOD READABLE
		{
			var packetData = new byte[resultedPacketSize];
			var packetCount = packets.Count;
			
			Packet packet;
			int packetSize;
			
			var index = 0;
			for (var i = 0; i < packetCount; i++)
			{
				packet = packets[i];
				packetSize = packet.Size;
				if (packet.Size > 1)
				{
					for (var j = 0; j < packetSize; j++)
					{
						packetData[index] = packet.Data[j];
						index++;
					}
				}
			}
			
			return packetData;
		}
	}
}