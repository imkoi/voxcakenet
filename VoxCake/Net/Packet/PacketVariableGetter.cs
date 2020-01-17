using System;
using UnityEngine;
using VoxCake.Net.Common.Serialization;

namespace VoxCake.Net
{
	public abstract partial class Packet
	{
		protected byte GetByteVariable()
		{
			var index = _variableIndexes[_currentGetterIndex];
			var value = PacketDeserializer.DeserializeByte(Data, index);
			
			_currentGetterIndex++;
#if VOXCAKE_NET_DEBUG
			HandleOutOfVariablesException();
#endif
			
			return value;
		}

		protected int GetShortVariable()
		{
			var index = _variableIndexes[_currentGetterIndex];
			var value = PacketDeserializer.DeserializeShort(Data, index);
			
			_currentGetterIndex++;
#if VOXCAKE_NET_DEBUG
			HandleOutOfVariablesException();
#endif
			
			return value;
		}

		protected int GetIntVariable()
		{
			var index = _variableIndexes[_currentGetterIndex];
			var value = PacketDeserializer.DeserializeInt(Data, index);
			
			_currentGetterIndex++;
#if VOXCAKE_NET_DEBUG
			HandleOutOfVariablesException();
#endif
			
			return value;
		}

		protected float GetFloatVariable()
		{
			var index = _variableIndexes[_currentGetterIndex];
			var value = PacketDeserializer.DeserializeFloat(Data, index);
			
			_currentGetterIndex++;
#if VOXCAKE_NET_DEBUG
			HandleOutOfVariablesException();
#endif
			
			return value;
		}

		protected Vector2 GetVector2Variable()
		{
			var index = _variableIndexes[_currentGetterIndex];
			var value = PacketDeserializer.DeserializeVector2(Data, index);
			
			_currentGetterIndex++;
#if VOXCAKE_NET_DEBUG
			HandleOutOfVariablesException();
#endif
			
			return value;
		}

		protected Vector3 GetVector3Variable()
		{
			var index = _variableIndexes[_currentGetterIndex];
			var value = PacketDeserializer.DeserializeVector3(Data, index);
			
			_currentGetterIndex++;
#if VOXCAKE_NET_DEBUG
			HandleOutOfVariablesException();
#endif
			
			return value;
		}

		private void HandleOutOfVariablesException()
		{
			if (_currentGetterIndex > _variableTypes.Length)
			{
				throw new Exception("OUT OF VARIABLES");
			}
		}
	}
}