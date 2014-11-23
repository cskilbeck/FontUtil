//////////////////////////////////////////////////////////////////////

using System;
using System.Runtime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;

//////////////////////////////////////////////////////////////////////

namespace FontUtil
{
    //////////////////////////////////////////////////////////////////////

    class TTFParser
    {
        //////////////////////////////////////////////////////////////////////

        static UInt32 TagFromString(string tag)
        {
            UInt32 a = (UInt32)(tag[0] & 0xff);
            UInt32 b = (UInt32)(tag[1] & 0xff << 8);
            UInt32 c = (UInt32)(tag[2] & 0xff << 16);
            UInt32 d = (UInt32)(tag[3] & 0xff << 24);
            return a | b | c | d;
        }

        //////////////////////////////////////////////////////////////////////

        static string StringFromTag(UInt32 tag)
        {
            char[] c = {
                        (char)((tag >> 24) & 0xff),
                        (char)((tag >> 16) & 0xff),
                        (char)((tag >> 8) & 0xff),
                        (char)(tag & 0xff)
                       };
            return new string(c);
        }

        //////////////////////////////////////////////////////////////////////

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OffsetTable
        {
            public UInt16 uMajorVersion;
            public UInt16 uMinorVersion;
            public UInt16 uNumOfTables;
            public UInt16 uSearchRange;
            public UInt16 uEntrySelector;
            public UInt16 uRangeShift;
        }

        //////////////////////////////////////////////////////////////////////
        //Tables in TTF file and there placement and name (tag)

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TableDirectory
        {
            public UInt32 szTag;
            public UInt32 uCheckSum; //Check sum
            public UInt32 uOffset; //Offset from beginning of file
            public UInt32 uLength; //length of the table in bytes
        };

        //////////////////////////////////////////////////////////////////////
        //Header of names table

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NameTableHeader
        {
            public UInt16 uFSelector; //format selector. Always 0
            public UInt16 uNRCount; //Name Records count
            public UInt16 uStorageOffset; //Offset for strings storage, 
                                   //from start of the table
        }

        //////////////////////////////////////////////////////////////////////
        //Record in names table

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NameRecord
        {
            public UInt16 uPlatformID;
            public UInt16 uEncodingID;
            public UInt16 uLanguageID;
            public UInt16 uNameID;
            public UInt16 uStringLength;
            public UInt16 uStringOffset; //from start of storage area
        }

        //////////////////////////////////////////////////////////////////////

        public abstract class ChunkHandler
        {
            //////////////////////////////////////////////////////////////////////

            public static Dictionary<UInt32, ChunkHandler> handlers = new Dictionary<UInt32, ChunkHandler>();

            //////////////////////////////////////////////////////////////////////

            public static void Register(UInt32 id, ChunkHandler chunkHandler)
            {
                handlers.Add(id, chunkHandler);
            }

            //////////////////////////////////////////////////////////////////////

            public ChunkHandler()
            {
            }

            //////////////////////////////////////////////////////////////////////

            public static void Handle(UInt32 id, byte[] data)
            {
                if (handlers.ContainsKey(id))
                {
                    handlers[id].Read(data);
                }
                else
                {
                    Debug.WriteLine(string.Format("Unknown chunk {0}", StringFromTag(id)));
                }
            }

            //////////////////////////////////////////////////////////////////////

            public abstract void Read(byte[] data);
        }

        //////////////////////////////////////////////////////////////////////

        public class NameTableHandler : ChunkHandler
        {
            public override void Read(byte[] data)
            {
            }
        }

        //////////////////////////////////////////////////////////////////////

        public class NameRecordHandler : ChunkHandler
        {
            public override void Read(byte[] data)
            {
            }
        }

        //////////////////////////////////////////////////////////////////////

        public TTFParser()
        {
            ChunkHandler.Register(TagFromString("ABCD"), new NameTableHandler());
            ChunkHandler.Register(TagFromString("DEFG"), new NameTableHandler());
            ChunkHandler.Register(TagFromString("HIJK"), new NameTableHandler());

            FileStream f = new FileStream("d:\\test.ttf", FileMode.Open);
            BinaryReader r = new BinaryReader(f);
            OffsetTable n = Reader.Read<OffsetTable>(r);
            for (int i = 0; i < n.uNumOfTables; ++i)
            {
                TableDirectory t = Reader.Read<TableDirectory>(r);
                Debug.WriteLine("{0:X8}, Length={1:X8}, Offset={2:X8}", StringFromTag(t.szTag), t.uLength, t.uOffset);
                long pos = r.BaseStream.Position;
                r.BaseStream.Seek(t.uOffset, SeekOrigin.Begin);
                byte[] data = r.ReadBytes((int)t.uLength);
                ChunkHandler.Handle(t.szTag, data);
                r.BaseStream.Seek(pos, SeekOrigin.Begin);
            }
        }
    }
}
