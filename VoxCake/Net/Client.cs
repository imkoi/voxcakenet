using System;
using Steamworks;
using UnityEngine;

namespace VoxCake.Net
{
	public class Client : IDisposable
	{
		public byte gameID { get; private set; }
		public ulong steamID { get; private set; }
		public string steamName { get; private set; }
		public Texture2D steamAvatar { get; private set; }
		public event Action<ApiInitializationErrorType> OnInitializationError;

		public Client(uint appid)
		{
			try
			{
				SteamClient.Init(appid);
			}
			catch
			{	
				//OnInitializationError.Invoke(ApiInitializationError.SteamNotRunning);
#if VOXCAKE_NET_DEBUG
				Debug.Log("Steam not running!");
#endif
			}
			
			steamID = SteamClient.SteamId.Value;
			steamName = SteamClient.Name;
			
#if VOXCAKE_NET_DEBUG
			Debug.Log($"{steamID} : {steamName}");
#endif
		}

		private Texture2D GetAvatar()
		{
			var getAvatarTask = SteamFriends.GetSmallAvatarAsync(steamID);
			getAvatarTask.Wait();
			var steamworksTexture = getAvatarTask.Result.Value;
			
			var width = (int) steamworksTexture.Width;
			var height = (int) steamworksTexture.Height;
			var size = width * height;
			
			var colors = new Color32[width * width];
			var texture = new Texture2D(width, height);

			var x = 0;
			var y = 0;
			var steamworksColor = new Steamworks.Data.Color();
			for(var i = 0; i < size; i++)
			{
				x = i / width;
				y = i % height;
				steamworksColor = steamworksTexture.GetPixel(x, y);
				colors[i] = new Color32(steamworksColor.r, steamworksColor.g, steamworksColor.b, steamworksColor.a);
			}
			texture.SetPixels32(colors);
			texture.Apply();
			
			var sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
			
			return texture;
		}

		public void Dispose()
		{
			steamName = null;
			steamAvatar = null;
			SteamClient.Shutdown();
		}
	}
}