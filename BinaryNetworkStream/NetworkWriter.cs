using System.Buffers;
using System.Net.Sockets;
using System.Text;

namespace BinaryNetworkStream;

public class NetworkWriter : TcpWriter, ITcpWriter
{
	private const int DefaultCapacity = 512;
	private byte[] _buffer = new byte[DefaultCapacity];

	public NetworkWriter(Socket socket) : base(socket)
	{
	}
	public void Write(Stream stream)
	{
		WriteCore(stream);
	}
	public void Write(ReadOnlySpan<byte> buffer)
	{
		WriteCore(buffer);
	}

	public void Write(bool value)
	{
		Write((byte) (value ? 1 : 0));
	}

	public void Write(byte value)
	{
		Span<byte> buffer = stackalloc byte[1];
		buffer[0] = value;
		Write(buffer);
	}

	public void Write(short value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(short)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write short failed");

		Write(buffer);
	}

	public void Write(int value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(int)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write int failed");

		Write(buffer);
	}

	public void Write(uint value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(uint)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write uint failed");

		Write(buffer);
	}

	public void Write(long value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(long)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write long failed");

		Write(buffer);
	}

	public void Write(ulong value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(ulong)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write ulong failed");
		
		Write(buffer);
	}

	public void Write(float value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(float)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write float failed");

		Write(buffer);
	}

	public void Write(double value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(double)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write double failed");

		Write(buffer);
	}

	public void Write(TimeSpan num) => Write(num.Ticks);

	public void Write(Guid guid)
	{
		Span<byte> buffer = stackalloc byte[16];
		if (!guid.TryWriteBytes(buffer))
			throw new Exception("Write Guid failed");

		Write(buffer);
	}

	public void Write(char value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(char)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write char failed");

		Write(buffer);
	}

	public void Write(ushort value)
	{
		Span<byte> buffer = stackalloc byte[sizeof(ushort)];
		if (!BitConverter.TryWriteBytes(buffer, value))
			throw new Exception("Write ushort failed");

		Write(buffer);
	}

	public void Write(sbyte value)
	{
		Span<byte> buffer = stackalloc byte[1];
		buffer[0] = (byte) value;
		Write(buffer);
	}

	public void Write(decimal value)
	{
		var decimalBits = decimal.GetBits(value);
		Span<byte> buffer = stackalloc byte[sizeof(decimal)];
		for (var i = 0; i < decimalBits.Length; i++)
		{
			if (!BitConverter.TryWriteBytes(buffer.Slice(i * sizeof(int)), decimalBits[i]))
				throw new Exception("Write decimal failed");
		}

		Write(buffer);
	}


	public void Write(string value)
	{
		var byteCount = Encoding.UTF8.GetByteCount(value);
		var rented = ArrayPool<byte>.Shared.Rent(byteCount);
		var buffer = new Span<byte>(rented, 0, byteCount);
		try
		{
			Encoding.UTF8.GetBytes(value, buffer);
			Write(byteCount);
			Write(buffer);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(rented);
		}
	}
}