using System;
using System.Collections.Generic;
using VoxCake.Net.Common;
using VoxCake.Net.Common.Serialization;

namespace VoxCake.Net
{
	/// <summary>
	/// Class description
	/// </summary>
	public abstract partial class Packet : IPostBindablePacket, IPostExecutablePacket, IDisposable
	{
		private const int HEADER_SIZE = 1;
		public byte ID { get; private set; }
		protected ulong PlayerID { get; private set; }
		public SendType SendType { get; private set; }
		public int Size { get; private set; }
		
		private List<VariableType> _variableTypeCollection;
		private VariableType[] _variableTypes;
		private List<int> _variableIndexCollection;
		private int[] _variableIndexes;
		
		public byte[] Data { get; private set; }
		private int _currentSetterIndex;
		private int _currentGetterIndex;

		/// <summary>
		/// DONT MAKE OBJECTS OF PACKETS! Bind their your Protocol instead.
		/// </summary>
		protected Packet()
		{
			_variableTypeCollection = new List<VariableType>();
			_variableIndexCollection = new List<int>();
			
			Size += HEADER_SIZE;
		}
		
		/// <summary>
		/// Use this method to pass some params into your packet.
		/// </summary>
		protected void BindVariable(VariableType type)
		{
			_variableTypeCollection.Add(type);
			_variableIndexCollection.Add(Size);
			
			switch(type)
			{
				case VariableType.Byte:
					Size += 1;
					break;
				case VariableType.Short:
					Size += 2;
					break;
				case VariableType.Int:
					Size += 4;
					break;
				case VariableType.Float:
					Size += 4;
					break;
				case VariableType.Vector2:
					Size += 8;
					break;
				case VariableType.Vector3:
					Size += 12;
					break;
			}
		}

		/// <summary>
		/// Use this method to pass some params into your packet.
		/// </summary>
		protected void SetVariable(object value)
		{
			var type = _variableTypes[_currentSetterIndex];
			var index = _variableIndexes[_currentSetterIndex];
			
			switch(type)
			{
				case VariableType.Byte:
					PacketSerializer.SerializeByte(value, Data, index);
					break;
				case VariableType.Short:
					PacketSerializer.SerializeShort(value, Data, index);
					break;
				case VariableType.Int:
					PacketSerializer.SerializeInt(value, Data, index);
					break;
				case VariableType.Float:
					PacketSerializer.SerializeFloat(value, Data, index);
					break;
				case VariableType.Vector2:
					PacketSerializer.SerializeVector2(value, Data, index);
					break;
				case VariableType.Vector3:
					PacketSerializer.SerializeVector3(value, Data, index);
					break;
			}
			
			_currentSetterIndex++;
			if (_currentSetterIndex == _variableTypes.Length)
			{
				_currentSetterIndex = 0;
			}
		}

		public void Dispose()
		{
			DisposeCollections();
			_variableTypes = null;
			Data = null;
			_variableIndexes = null;
			_currentSetterIndex = 0;
		}

		internal void SetData(byte[] packetData)
		{
			Data = packetData;
		}

		internal void SetPlayer(ulong playerID)
		{
			PlayerID = playerID;
		}

		internal void SetSendType(SendType sendType)
		{
			SendType = sendType;
		}

		private void DisposeCollections()
		{
			_variableTypeCollection = null;
			_variableIndexCollection = null;
		}

		void IPostBindablePacket.PostBinding(Protocol protocol)
		{
			Data = new byte[Size];
			Data[0] = protocol.Size;
			
			_variableTypes = _variableTypeCollection.ToArray();
			_variableIndexes = _variableIndexCollection.ToArray();
			
			DisposeCollections();
		}

		void IPostExecutablePacket.PostExecuting()
		{
			_currentGetterIndex = 0;
		}
	}
}

