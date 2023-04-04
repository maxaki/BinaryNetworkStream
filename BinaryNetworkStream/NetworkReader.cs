using System.Buffers;
using System.Net.Sockets;
using System.Text;

namespace BinaryNetworkStream;

public class NetworkReader : TcpReader, ITcpReader
{
	public NetworkReader(Socket socket) : base(socket)
	{
	}
	public void Read(Stream destination, int size)
	{
		ReadToStream(destination, size);
	}
	private Span<byte> Read(Span<byte> buffer)
	{
		ReadBytesCore(buffer);
		return buffer;
	}

	public Guid ReadGuid() => new Guid(Read(stackalloc byte[16]));
	public int ReadInt32() => BitConverter.ToInt32(Read(stackalloc byte[sizeof(int)]));
	public uint ReadUInt32() => BitConverter.ToUInt32(Read(stackalloc byte[sizeof(uint)]));
	public long ReadInt64() => BitConverter.ToInt64(Read(stackalloc byte[sizeof(long)]));
	public ulong ReadUInt64() => BitConverter.ToUInt64(Read(stackalloc byte[sizeof(ulong)]));
	public bool ReadBoolean() => Read(stackalloc byte[sizeof(bool)])[0] != 0;
	public double ReadDouble() => BitConverter.ToDouble(Read(stackalloc byte[sizeof(double)]));
	public float ReadFloat() => BitConverter.ToSingle(Read(stackalloc byte[sizeof(float)]));
	public char ReadChar() => BitConverter.ToChar(Read(stackalloc byte[sizeof(char)]));
	public ushort ReadUInt16() => BitConverter.ToUInt16(Read(stackalloc byte[sizeof(ushort)]));
	public sbyte ReadSByte() => (sbyte) Read(stackalloc byte[1])[0];
	public byte ReadByte() => Read(stackalloc byte[1])[0];
	public short ReadInt16() => BitConverter.ToInt16(Read(stackalloc byte[sizeof(short)]));

	public decimal ReadDecimal()
	{
		Span<byte> buffer = stackalloc byte[sizeof(decimal)];
		Read(buffer);

		Span<int> decimalBits = stackalloc int[4];
		for (var i = 0; i < decimalBits.Length; i++)
		{
			decimalBits[i] = BitConverter.ToInt32(buffer.Slice(i * sizeof(int)));
		}

		return new decimal(decimalBits);
	}

	public string ReadString()
	{
		var size = ReadInt32();
		var rented = ArrayPool<byte>.Shared.Rent(size);
		var buffer = new Span<byte>(rented, 0, size);
		try
		{
			Read(buffer);
			return Encoding.UTF8.GetString(buffer);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(rented);
		}
	}
}