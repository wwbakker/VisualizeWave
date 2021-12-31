/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizeWave.io
{
    internal class WavFile
    {
        private static readonly byte[] RIFF_HEADER = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        private static readonly byte[] FORMAT_WAVE = new byte[] { 0x57, 0x41, 0x56, 0x45 };
        private static readonly byte[] FORMAT_TAG = new byte[] { 0x66, 0x6D, 0x74, 0x20 };
        private static readonly byte[] AUDIO_FORMAT = new byte[] { 0x1, 0x0 };
        private static readonly byte[] SUBCHUNK_ID = new byte[] { 0x64, 0x61, 0x74, 0x61 };
        private static readonly int HEADER_SIZE_BYTES = 40;
        public int ChunkSize
        {
            get
            {
                return BitConverter.ToInt32(Data, 4);
            }
        }

        public int ChannelCount { get; private set; }

        private readonly String path;

        private byte[]? _data = null;
        private byte[] Data
        {
            get
            {
                if (_data == null)
                {
                    LoadFromDisk();
                }
                return _data ?? Array.Empty<byte>();
            }
        }

        public WavFile(String path)
        {
            this.path = path;
        }

        public void LoadFromDisk()
        {
            _data = File.ReadAllBytes(path);
        }
    }
}
*/