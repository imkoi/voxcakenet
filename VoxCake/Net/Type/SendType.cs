namespace VoxCake.Net
{
	/// <summary>
	/// Handle a type of packet sending
	/// <para/> Unreliable(UDP) - fast, but packets could be recieved out of order.
	/// <para/> Reliable(TCP) - slow, but packets recieved in order.
	/// </summary>
	public enum SendType
	{
		Unreliable,
		Reliable
	}
}
