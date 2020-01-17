Example of *NetworkManager* usage
```csharp
using UnityEngine;
using UnityEngine.UI;
using VoxCake.Net;
using Code.Packets;

namespace Code.Managers
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private SessionRequestPopup _sessionRequestPopup;
		private NetworkManager _networkManager;

		private void Awake()
		{
			_networkManager = new NetworkManager(480);
			_networkManager.BindProtocol<MyProtocol>();
			_networkManager.OnSessionRequest += _sessionRequestPopup.OpenSessionRequestPopup;
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				_networkManager.SendPacket<ExamplePacket>((byte)3, 12, 123.7875f,
					new Vector2(12,13), new Vector3(123, 156.444f));
			}
			if(Input.GetKeyDown(KeyCode.LeftShift))
			{
				_networkManager.SendPacket<HelloWorldPacket>();
			}
			
			_networkManager.ReadPackets();
		}

		private void OnDestroy()
		{
			_networkManager.Dispose();
		}
	}
}
```

Example of *Protocol* usage
```csharp
using VoxCake.Net;

namespace Code.Packets
{
	public class MyProtocol : Protocol
	{
		protected override void Bindings()
		{
			BindPacket<HelloWorldPacket>(SendType.Unreliable);
			BindPacket<ExamplePacket>(SendType.Unreliable);
		}
	}
}
```

Example of *Packet* usage
```csharp
using UnityEngine;
using VoxCake.Net;

public class HelloWorldPacket : Packet, IExecutablePacket
{
	public void Execute(bool isMine)
	{
		Debug.Log("Hello World");
	}
}
```

Example of *Packet* with variables usage
```csharp
using UnityEngine;
using VoxCake.Net;

namespace Code.Packets
{
	public class ExamplePacket : Packet, IBindablePacket, IExecutablePacket
	{
		public void BindVariables()
		{
			BindVariable(VariableType.Byte);
			BindVariable(VariableType.Int);
			BindVariable(VariableType.Float);
			BindVariable(VariableType.Vector2);
			BindVariable(VariableType.Vector3);
		}

		public void SetVariables(object[] packetVariables)
		{
			SetVariable(packetVariables[0]);
			SetVariable(packetVariables[1]);
			SetVariable(packetVariables[2]);
			SetVariable(packetVariables[3]);
			SetVariable(packetVariables[4]);
		}

		public void Execute(bool isMine)
		{
			Debug.Log(PlayerID);
			
			var x = GetByteVariable();
			var y = GetIntVariable();
			var z = GetFloatVariable();
			var w = GetVector2Variable();
			var q = GetVector3Variable();
			
			Debug.Log($"{x} + {y} + {z} + {w} + {q}");
		}
	}
}
```