namespace VoxCake.Net
{
	public interface IBindablePacket
	{
		void BindVariables();
		void SetVariables(object[] packetVariables);
	}
}