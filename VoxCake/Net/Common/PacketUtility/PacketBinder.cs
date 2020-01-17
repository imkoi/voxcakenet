namespace VoxCake.Net.Common.Utility
{
	internal static class PacketBinder
	{
		internal static void BindVariablesToPacket(Packet packet, Protocol protocol)
		{
			var bindablePacket = (IBindablePacket)packet;
			bindablePacket.BindVariables();
			var postBindablePacket = (IPostBindablePacket)packet;
			postBindablePacket.PostBinding(protocol);
		}

		internal static void SetVariablesToPacket(Packet packet, object[] packetVariables)
		{
			var bindablePacket = (IBindablePacket)packet;
			bindablePacket.SetVariables(packetVariables);
		}
	}
}