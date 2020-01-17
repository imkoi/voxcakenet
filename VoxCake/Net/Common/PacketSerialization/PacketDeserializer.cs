using System;
using UnityEngine;

namespace VoxCake.Net.Common.Serialization
{
	internal static class PacketDeserializer
	{
		internal static byte DeserializeByte(byte[] packetData, int index)
		{
			return packetData[index];
		}

		internal static short DeserializeShort(byte[] packetData, int index)
		{
			return (short)(packetData[index] | packetData[index + 1] << 8);
		}

		internal static int DeserializeInt(byte[] packetData, int index)
		{
			return packetData[index] | packetData[index + 1] << 8 |
				packetData[index + 2] << 16 | packetData[index + 3] << 24;
		}

		internal static float DeserializeFloat(byte[] packetData, int index)
		{
			return BitConverter.ToSingle(packetData, index);
		}

		internal static Vector2 DeserializeVector2(byte[] packetData, int index)
		{
			return new Vector2(BitConverter.ToSingle(packetData, index),
				BitConverter.ToSingle(packetData, index + 4));
		}

		internal static Vector3 DeserializeVector3(byte[] packetData, int index)
		{
			return new Vector3(BitConverter.ToSingle(packetData, index),
				BitConverter.ToSingle(packetData, index + 4),
				BitConverter.ToSingle(packetData, index + 8));
		}
	}
}