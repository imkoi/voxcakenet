namespace VoxCake.Net.Common.Utility
{
	internal static class PacketExecutor
	{
		internal static void ExecutePacket(Packet packet, bool isMine)
		{
			var executablePacket = (IExecutablePacket)packet;
			executablePacket.Execute(isMine);
			var postExecutablePacket = (IPostExecutablePacket)packet;
			postExecutablePacket.PostExecuting();
		}
	}
}