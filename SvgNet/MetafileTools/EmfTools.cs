/*
    Copyright © 2003 RiskCare Ltd. All rights reserved.
    Copyright © 2010 SvgNet & SvgGdi Bridge Project. All rights reserved.
    Copyright © 2015-2024 Rafael Teixeira, Mojmír Němeček, Benjamin Peterson and Other Contributors

    Original source code licensed with BSD-2-Clause spirit, treat it thus, see accompanied LICENSE for more
*/

namespace SvgNet.MetafileTools.EmfTools;
// Classes in this namespace were inspired by the code in http://wmf.codeplex.com/
public interface IBinaryRecord {
    void Read(BinaryReader reader);
}

public static class BinaryReaderExtensions {
    /// <summary>
    /// Skips excess bytes. Work-around for some WMF files that contain undocumented fields.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="excess"></param>
    public static void Skip(this BinaryReader reader, int excess) {
        if (excess > 0) {
            //Skip unknown bytes
            _ = reader.BaseStream.Seek(excess, SeekOrigin.Current);
            //var dummy = reader.ReadBytes(excess);
        }
    }
}

/// <summary>
/// Implements a EMF META record
/// </summary>
public abstract class EmfBinaryRecord : IBinaryRecord {
    /// <summary>
    /// Gets or sets record length
    /// </summary>
    public uint RecordSize {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets record type (aka RecordFunction)
    /// </summary>
    public EmfPlusRecordType RecordType {
        get;
        set;
    }

    /// <summary>
    /// Reads a record from binary stream. If this method is not overridden it will skip this record and go to next record.
    /// NOTE: When overriding this method remove the base.Read(reader) line from code.
    /// </summary>
    /// <param name="reader"></param>
    public virtual void Read(BinaryReader reader) {
    }

    protected EmfBinaryRecord() {
    }
}

/// <summary>
/// Low-level EMF parser
/// </summary>
public class EmfReader(Stream stream) : IDisposable {
    public bool IsEndOfFile => stream.Length == stream.Position;

    public void Dispose() {
        if (_reader is not null) {
            _reader.Close();
            _reader = null;
        }
        GC.SuppressFinalize(this);
    }

    public IBinaryRecord Read() {
        long begin = _reader.BaseStream.Position;

        var rt = (EmfPlusRecordType)_reader.ReadUInt32();
        uint recordSize = _reader.ReadUInt32();

        var record = new EmfUnknownRecord {
            RecordType = rt,
            RecordSize = recordSize
        };
        record.Read(_reader);

        long end = _reader.BaseStream.Position;
        long rlen = end - begin; //Read length
        long excess = recordSize - rlen;
        if (excess > 0) {
            //Oops, reader did not read whole record?!
            _reader.Skip((int)excess);
        }

        return record;
    }

    private BinaryReader _reader = new(stream);
}

public class EmfUnknownRecord : EmfBinaryRecord {
    public byte[] Data {
        get;
        set;
    }

    public override void Read(BinaryReader reader) {
        int length = (int)base.RecordSize - sizeof(uint) - sizeof(uint);
        Data = length > 0 ? reader.ReadBytes(length) : _emptyData;
    }

    private static readonly byte[] _emptyData = [];
}

