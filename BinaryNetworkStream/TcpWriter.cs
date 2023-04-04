using System.Buffers;
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
	protected void WriteCore(Stream stream)
	{
		var buffer = ArrayPool<byte>.Shared.Rent((int)stream.Length);
		try
		{
			int bytesRead;
			while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
			{
				WriteCore(buffer.AsSpan(0, bytesRead));
			}
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(buffer);
		}
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