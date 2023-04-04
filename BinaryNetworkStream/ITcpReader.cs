using System;
using System.IO;

namespace BinaryNetworkStream;

public interface ITcpReader : IDisposable
{
	void Read(Stream destination, int length);
	Guid ReadGuid();
	int ReadInt32();
	uint ReadUInt32();
	long ReadInt64();
	ulong ReadUInt64();
	bool ReadBoolean();
	double ReadDouble();
	float ReadFloat();
	char ReadChar();
	ushort ReadUInt16();
	sbyte ReadSByte();
	byte ReadByte();
	short ReadInt16();
	decimal ReadDecimal();
	string ReadString();
}