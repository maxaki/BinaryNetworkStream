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
	protected void WriteCore(Stream stream, int length)
	{
		var bufferLength = length > 0 ? length : (int)stream.Length;
		var buffer = ArrayPool<byte>.Shared.Rent(bufferLength);
		try
		{
			int bytesRead;
			var totalBytesRead = 0;
			while (totalBytesRead < bufferLength && (bytesRead = stream.Read(buffer, totalBytesRead, bufferLength - totalBytesRead)) != 0)
			{
				totalBytesRead += bytesRead;
			}

			if (totalBytesRead != bufferLength)
			{
				throw new ArgumentException("Stream did not contain enough data to write", nameof(stream));
			}

			WriteCore(buffer.AsSpan(0, totalBytesRead));
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