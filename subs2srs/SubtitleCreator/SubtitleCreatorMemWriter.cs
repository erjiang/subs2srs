//  Copyright (C) 2009-2016 Christopher Brochtrup
//
//  This file is part of subs2srs.
//
//  subs2srs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  subs2srs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with subs2srs.  If not, see <http://www.gnu.org/licenses/>.
//
//////////////////////////////////////////////////////////////////////////////

//
// This file was taken from the Subtitle Creator project and modified to fit the needs of subs2srs.
// http://sourceforge.net/projects/subtitlecreator/
//

using System;
using System.IO;

namespace SubtitleCreator
{
	/// <summary>
	/// Summary description for MemWriter.
	/// </summary>
	public class MemWriter 
	{
		private long pos;
		private byte[] buf;
		private long size;

		public long GetPosition()
		{
			return pos;
		}

		public byte[] GetBuf()
		{
			return buf;
		}

		public MemWriter(long Size)
		{
			buf  = new byte[Size];
			size = Size;
			pos  = 0;
		}

		public byte ReadByte()
		{
			return buf[pos++];
		}

		public UInt16 ReadInt16()
		{
			return (UInt16) (
				((UInt16) buf[pos++] <<  8) |
				((UInt16) buf[pos++]));
		}

		public int ReadInt32()
		{
			return (
				((int) buf[pos++] << 24) | 
				((int) buf[pos++] << 16) |
				((int) buf[pos++] <<  8) |
				((int) buf[pos++]));
		}

		public bool Seek(long position)
		{
			if (pos < size)
			{
				pos = Math.Max(0,position);
				return true;
			}
			else
				return false;
		}

		public bool GotoBegin()
		{
			pos = 0;
			return true;
		}

		public bool GotoEnd()
		{
			pos = size;
			return true;
		}

		public void WriteByte(byte val)
		{
			buf[pos++]	= val;
		}

		public void WriteInt16(ushort val)
		{
			buf[pos++]	= (byte) ((val & 0xFF00) >> 8);
			buf[pos++]	= (byte) (val & 0x00FF);
		}

		public void WriteInt32(uint val)
		{
			buf[pos++]	= (byte)  (val & 0x000000FF);
			buf[pos++]	= (byte) ((val & 0x0000FF00) >>  8);
			buf[pos++]	= (byte) ((val & 0x00FF0000) >> 16);
			buf[pos++]	= (byte) ((val & 0xFF000000) >> 24);
		}

		private byte[] BitSet = {0,0,0};
		private byte BitPointer;

		private void Write2bits(byte val)
		{
			// Write 2 bits, when full write a byte to the memory buffer
			if (BitPointer < 3)
			{
				BitSet[BitPointer++] = (byte) (val & 0x3);
			}
			else
			{
				BitPointer = 0;
				buf[pos++] = (byte) ((BitSet[0] << 6) | (BitSet[1] << 4) | (BitSet[2] << 2) | (val & 0x3));
			}
		}

		private void Write2bits(bool EndOfLine)
		{
			if (EndOfLine)
			{
				// Write all bits, and let the position pointer start at a new byte
				switch (BitPointer)
				{
					case 0:
						break;
					case 1:
						buf[pos++] = (byte) (BitSet[0] << 6);
						break;
					case 2:
						buf[pos++] = (byte) ((BitSet[0] << 6) | (BitSet[1] << 4));
						break;
					case 3:
						buf[pos++] = (byte) ((BitSet[0] << 6) | (BitSet[1] << 4) | (BitSet[2] << 2));
						break;
				}
				BitPointer = 0;
				// Start new line at next even position
				//if (pos % 2 != 0)
				//	WriteByte(0);
			}
		}

		public void WriteRLE(uint RunLength, byte Color, bool EndOfLine)
		{
			// If end of line, add a carriage (two empty bytes) after writing the last run sequence
			if (EndOfLine)
			{
				if (Color == 0)
				{
					Write2bits(true);		// Flush buffer and write
					WriteInt16(0);
				}
				else
				{
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0); // 7x
					Write2bits(Color);
					Write2bits(true);		// Flush buffer and write
				}
				return;
			}

			// Create the RLE code - use a max RL of 255
			while (RunLength > 255)
			{
				RunLength -= 255;
				WriteRLE(255, Color, false);
			}
			if (RunLength < 4)
				Write2bits((byte) RunLength);
			else
			{
				// Run length is longer than 4: Count number of 00 (2 zero bits) we need to add to the code
				ushort Counter = 0;
				while ((RunLength >> (++Counter * 2)) > 0) 
				{
					// Write 00 bits
					Write2bits((byte) 0);
				}
				// Write RunLength
				for (--Counter; Counter>0; Counter--)
				{
					Write2bits((byte) (RunLength >> (Counter*2)));
				}
				Write2bits((byte) (RunLength >> (Counter*2)));
			}
			Write2bits((byte) Color);

		}
		/*
		public void WriteRLE(uint RunLength, byte Color, bool EndOfLine)
		{
			// If end of line, add a carriage (two empty bytes) after writing the last run sequence
			if (EndOfLine)
			{
				if (Color == 0)
				{
					Write2bits(true);		// Flush buffer and write
					WriteInt16(0);
				}
				else
				{
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0);
					Write2bits(0); // 7x
					Write2bits(Color);
					Write2bits(true);		// Flush buffer and write
				}
				return;
			}

			// Create the RLE code - use a max RL of 255
			while (RunLength > 255)
			{
				RunLength -= 255;
				WriteRLE(255, Color, false);
			}
			if (RunLength < 4)
				Write2bits((byte) RunLength);
			else
			{
				// Run length is longer than 4: Count number of 00 (2 zero bits) we need to add to the code
				ushort Counter = 0;
				while ((RunLength >> (++Counter * 2)) > 0) 
				{
					// Write 00 bits
					Write2bits((byte) 0);
				}
				// Write RunLength
				for (--Counter; Counter>0; Counter--)
				{
					Write2bits((byte) (RunLength >> (Counter*2)));
				}
				Write2bits((byte) (RunLength >> (Counter*2)));
			}
			Write2bits((byte) Color);

		}
		*/
	}
}
