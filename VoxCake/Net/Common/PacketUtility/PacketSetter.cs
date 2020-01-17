using System.Collections.Generic;

namespace VoxCake.Net.Common.Utility
{
	internal class PacketSetter
	{
		internal static void SetPacketToCollection(Packet packet, List<Packet> collection)
		{
			var packetIndex = PacketGetter.GetPacketIndexInCollection(packet, collection);
			if (packetIndex != -1)
			{
				collection[packetIndex] = packet;
			}
			else
			{
				collection.Add(packet);
			}
		}
	}
}