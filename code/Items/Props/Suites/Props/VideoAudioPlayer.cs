using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public interface IPlayableDevice
{
	void PlayAudio( string id );
	void PauseAudio();
	void StopAudio();

	void RequestNewVideo( string url );
	void PauseVideo();
	void EndVideo();
}

public partial class VideoAudioPlayer : PHSuiteProps, IPlayableDevice
{
	public string URLToWebSocket { get; } = "";

	public WebSocket WebSocket { get; private set; }

	public async Task CreateWebSocket()
	{
		WebSocket = new WebSocket();
		
		try
		{
			await WebSocket.Connect( URLToWebSocket );
		}
		catch ( Exception )
		{
			Log.Error( "Couldn't connect to P&C Backend! Retrying connection in 5 seconds..." );
			await RetryConnection();
		}

		if ( WebSocket.IsConnected )
		{
			Log.Info( "Connected to P&C Backend!" );
		}
		else
		{
			return;
		}
	}

	private async Task RetryConnection()
	{
		int retryNum = 1;

		while ( !WebSocket.IsConnected )
		{
			await Task.Delay( 5000 );

			Log.Info( $"Attempting reconnect to P&C Backend, Attempt: {retryNum}" );
			try
			{
				WebSocket = new WebSocket();
				await WebSocket.Connect( URLToWebSocket );
			}
			catch ( Exception )
			{
				
			}

			retryNum++;
		}

		Log.Info( "Successfully reconnected to Cinema Backend!" );
	}

	public void EndVideo()
	{
		
	}

	public void PauseAudio()
	{
		
	}

	public void PauseVideo()
	{
		
	}

	public void PlayAudio( string id )
	{
		
	}

	public void RequestNewVideo( string url )
	{
		if ( !WebSocket.IsConnected )
		{
			Log.Error( "Websocket is not connected" );
			return;
		}
		WebSocket.Send( $"stream_request {url}" );
	}

	public void StopAudio()
	{
		
	}
}
