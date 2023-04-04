namespace BinaryNetworkStream;

public interface ITcpWriter : IDisposable
{
	void Write(ReadOnlySpan<byte> buffer);
	void Write(bool value);
	void Write(byte value);
	void Write(short value);
	void Write(int value);
	void Write(uint value);
	void Write(long value);
	void Write(ulong value);
	void Write(float value);
	void Write(double value);
	void Write(TimeSpan num);
	void Write(Guid guid);
	void Write(char value);
	void Write(ushort value);
	void Write(sbyte value);
	void Write(decimal value);
	void Write(string value);
}