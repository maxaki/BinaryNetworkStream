using System;
using System.IO;

namespace BinaryNetworkStream;

public interface ITcpReader : IDisposable
{
	void Read(Stream destination, int length);
	void Read(byte[] destination, int length);
	byte[] ReadBytes(int length);
	void ReadPacket(byte[] buffer);
	byte[] ReadPacket();
	Guid ReadGuid();
	int ReadInt32();
	uint ReadUInt32();
	long ReadInt64();
	ulong ReadUInt64();
	ulong ReadUInt64Reverse();
	bool ReadBoolean();
	double ReadDouble();
	float ReadFloat();
	char ReadChar();
	ushort ReadUInt16();
	short ReadInt16Reverse();
	sbyte ReadSByte();
	byte ReadByte();
	short ReadInt16();
	decimal ReadDecimal();
	string ReadString();
}