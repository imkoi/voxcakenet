using System;
using UnityEngine;

namespace VoxCake.Net.Common.Serialization
{
	internal static class PacketSerializer
	{
		internal static void SerializeByte(object value, byte[] packetData, int index)
		{
			packetData[index] = (byte)value;
		}

		internal static void SerializeShort(object value, byte[] packetData, int index)
		{
			var valueType = (int)value;
			
			packetData[index] = (byte)valueType;
			packetData[index + 1] = (byte)(valueType >> 8);
		}

		internal static void SerializeInt(object value, byte[] packetData, int index)
		{
			var valueType = (int)value;
			
			packetData[index] = (byte)valueType;
			packetData[index + 1] = (byte)(valueType >> 8);
			packetData[index + 2] = (byte)(valueType >> 16);
			packetData[index + 3] = (byte)(valueType >> 24);
		}

		internal static void SerializeFloat(object value, byte[] packetData, int index)
		{
			var valueBytes = BitConverter.GetBytes((float)value);
			
			packetData[index] = valueBytes[0];
			packetData[index + 1] = valueBytes[1];
			packetData[index + 2] = valueBytes[2];
			packetData[index + 3] = valueBytes[3];
		}

		internal static void SerializeVector2(object value, byte[] packetData, int index)
		{
			var valueType = (Vector2)value;
			var xBytes = BitConverter.GetBytes(valueType.x);
			var yBytes = BitConverter.GetBytes(valueType.y);
			
			packetData[index] = xBytes[0];
			packetData[index + 1] = xBytes[1];
			packetData[index + 2] = xBytes[2];
			packetData[index + 3] = xBytes[3];
			
			packetData[index + 4] = yBytes[0];
			packetData[index + 5] = yBytes[1];
			packetData[index + 6] = yBytes[2];
			packetData[index + 7] = yBytes[3];
		}

		internal static void SerializeVector3(object value, byte[] packetData, int index)
		{
			var valueType = (Vector3)value;
			var xBytes = BitConverter.GetBytes(valueType.x);
			var yBytes = BitConverter.GetBytes(valueType.y);
			var zBytes = BitConverter.GetBytes(valueType.z);
			
			packetData[index] = xBytes[0];
			packetData[index + 1] = xBytes[1];
			packetData[index + 2] = xBytes[2];
			packetData[index + 3] = xBytes[3];
			
			packetData[index + 4] = yBytes[0];
			packetData[index + 5] = yBytes[1];
			packetData[index + 6] = yBytes[2];
			packetData[index + 7] = yBytes[3];
			
			packetData[index + 8] = zBytes[0];
			packetData[index + 9] = zBytes[1];
			packetData[index + 10] = zBytes[2];
			packetData[index + 11] = zBytes[3];
		}
	}
}