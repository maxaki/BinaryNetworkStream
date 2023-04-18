using System.Net.Sockets;

namespace BinaryNetworkStream;

public class NetworkStreamHandler : ITcpReader, ITcpWriter
{
	public bool Connected => _socket?.Connected is true;
	private readonly ITcpReader _reader;
	private readonly ITcpWriter _writer;
	private readonly Socket _socket;

	public NetworkStreamHandler(Socket socket)
	{
		_socket = socket;
		_reader = new NetworkReader(socket);
		_writer = new NetworkWriter(socket);
	}
	
	// Implement ITcpReader methods by forwarding calls to the _reader instance
	public void Read(Stream destination, int length) => _reader.Read(destination, length);
	public Span<byte> ReadPacket(Span<byte> buffer) => _reader.ReadPacket(buffer);

	public Guid ReadGuid() => _reader.ReadGuid();
	public int ReadInt32() => _reader.ReadInt32();
	public uint ReadUInt32() => _reader.ReadUInt32();
	public long ReadInt64() => _reader.ReadInt64();
	public ulong ReadUInt64() => _reader.ReadUInt64();
	public bool ReadBoolean() => _reader.ReadBoolean();
	public double ReadDouble() => _reader.ReadDouble();
	public float ReadFloat() => _reader.ReadFloat();
	public char ReadChar() => _reader.ReadChar();
	public ushort ReadUInt16() => _reader.ReadUInt16();
	public sbyte ReadSByte() => _reader.ReadSByte();
	public byte ReadByte() => _reader.ReadByte();

	public short ReadInt16() => _reader.ReadInt16();
	public decimal ReadDecimal() => _reader.ReadDecimal();
	public string ReadString() => _reader.ReadString();

	// Implement ITcpWriter methods by forwarding calls to the _writer instance


	public void Write(Stream stream) => _writer.Write(stream);
	public void WritePacket(ReadOnlySpan<byte> buffer) => 	_writer.WritePacket(buffer);
	public void Write(ReadOnlySpan<byte> buffer) => _writer.Write(buffer);
	public void Write(bool value) => _writer.Write(value);
	public void Write(byte value) => _writer.Write(value);
	public void Write(short value) => _writer.Write(value);
	public void Write(int value) => _writer.Write(value);
	public void Write(uint value) => _writer.Write(value);
	public void Write(long value) => _writer.Write(value);
	public void Write(ulong value) => _writer.Write(value);
	public void Write(float value) => _writer.Write(value);
	public void Write(double value) => _writer.Write(value);
	public void Write(TimeSpan num) => _writer.Write(num);
	public void Write(Guid guid) => _writer.Write(guid);
	public void Write(char value) => _writer.Write(value);
	public void Write(ushort value) => _writer.Write(value);
	public void Write(sbyte value) => _writer.Write(value);
	public void Write(decimal value) => _writer.Write(value);
	public void Write(string value) => _writer.Write(value);

	public void Dispose()
	{
		_reader.Dispose();
		_writer.Dispose();
	}
}