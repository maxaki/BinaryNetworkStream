using System.Buffers;
using System.Net.Sockets;

namespace BinaryNetworkStream;

public class TcpReader
{
	private bool Connected => _socket?.Connected is true;
	private Socket _socket;

	protected TcpReader(Socket socket)
	{
		_socket = socket;
	}

	private void ReadBytesExact(Span<byte> buffer)
	{
		var bytesRead = 0;
		var length = buffer.Length;
		while (bytesRead < length)
		{
			var read = _socket.Receive(buffer.Slice(bytesRead, length - bytesRead));
			if (read is 0 or -1)
			{
				throw new SocketException(10054);
			}

			bytesRead += read;
		}
	}

	private void ReadExactBytesToStream(Stream stream, int length)
	{
		var rented = ArrayPool<byte>.Shared.Rent(length);
		var buffer = new Span<byte>(rented, 0, length);
		var bytesRead = 0;
		try
		{
			while (bytesRead < length)
			{
				var read = _socket.Receive(buffer.Slice(bytesRead, length - bytesRead));
				if (read is 0 or -1)
				{
					throw new SocketException(10054);
				}

				bytesRead += read;
			}

			stream.Write(buffer[..bytesRead]);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(rented);
		}
	}

	protected void ReadBytesCore(Span<byte> buffer)
	{
		try
		{
			ReadBytesExact(buffer);
		}
		catch (Exception ex)
		{
			HandleException(ex);
		}
	}

	protected void ReadToStream(Stream destination, int size)
	{
		try
		{
			ReadExactBytesToStream(destination, size);
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