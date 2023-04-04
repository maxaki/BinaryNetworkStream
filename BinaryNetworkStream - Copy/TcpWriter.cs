using System.Net.Sockets;

namespace BinaryNetworkStream;

public class TcpWriter
{
	private bool Connected => _socket?.Connected is true;
	private Socket _socket;
	protected TcpWriter(Socket socket)
	{
		_socket = socket;
	}
	protected void WriteCore(ReadOnlySpan<byte> buffer)
	{
		try
		{
			_socket.Send(buffer);
		}
		catch (Exception ex)
		{
			HandleException(ex);
		}
	}
	private void HandleException(Exception ex)
	{
		if (ex is IOException or SocketException)
		{
			Dispose();
		}

		throw ex;
	}
	public void Dispose()
	{
		if (!Connected)
			return;
		
		try
		{
			_socket.Shutdown(SocketShutdown.Both);
			_socket.Dispose();
		}
		catch
		{
			// ignored
		}
		
		_socket = null!;
	}
}