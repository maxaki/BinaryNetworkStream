using System.Net.Sockets;

namespace BinaryNetworkStream;

/// <summary>
/// Provides reading and writing functionality over a TCP network using a binary protocol.
/// </summary>
public sealed class BinaryNetwork : ITcpReader, ITcpWriter
{
	public bool Connected => _socket?.Connected is true;
	private readonly ITcpReader _reader;
	private readonly ITcpWriter _writer;
	private readonly Socket _socket;

	public BinaryNetwork(Socket socket)
	{
		_socket = socket;
		_reader = new NetworkReader(socket);
		_writer = new NetworkWriter(socket);
	}

	/// <summary>
	/// Disconnects the underlying socket.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown if the underlying socket is null.</exception>
	public void Disconnect()
	{
		if (_socket is null)
			throw new ArgumentNullException(nameof(_socket));


		_socket.Shutdown(SocketShutdown.Both);
		_socket.Dispose();
	}

	public Socket Socket => _socket;
	
	/// <summary>
	/// Reads the specified number of bytes from the underlying socket and writes them to the provided destination stream.
	/// </summary>
	/// <param name="destination">The destination stream to which to write the bytes.</param>
	/// <param name="length">The number of bytes to read from the underlying socket.</param>
	public void Read(Stream destination, int length) => _reader.Read(destination, length);
	
	/// <summary>
	/// Reads a packet from the underlying socket by first reading a header containing the size of the packet as an integer, 
	/// and then reads the rest of the packet data into the provided buffer. If the buffer is not large enough to contain the
	/// entire packet, a new buffer of the appropriate size will be created.
	/// </summary>
	/// <param name="buffer">The buffer to store the packet data.</param>
	public void ReadPacket(byte[] buffer) => _reader.ReadPacket(buffer);

	/// <summary>
	/// Reads a packet from the underlying socket by first reading a header containing the size of the packet as an integer, 
	/// and then reads the rest of the packet data into a newly created byte array.
	/// </summary>
	/// <returns>A byte array containing the packet data.</returns>
	public byte[] ReadPacket() => _reader.ReadPacket();
	
	/// <summary>
	/// Writes a specified number of bytes from the provided stream to the underlying socket.
	/// </summary>
	/// <param name="stream">The stream whose contents to write to the underlying socket.</param>
	/// <param name="length">The number of bytes to write from the stream. If 0, the entire stream will be written.</param>
	public void Write(Stream stream, int length = 0) => _writer.Write(stream, length);
	
	/// <summary>
	/// Writes a specified span of bytes to the underlying socket.
	/// </summary>
	/// <param name="buffer">The span of bytes to write.</param>
	public void Write(ReadOnlySpan<byte> buffer) => _writer.Write(buffer);
	
	/// <summary>
	/// Writes a packet to the underlying socket by first writing a header with the size of the packet as an integer, followed by the actual packet data.
	/// </summary>
	/// <param name="buffer">The packet data to be written.</param>
	public void WritePacket(ReadOnlySpan<byte> buffer) => _writer.WritePacket(buffer);
	
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