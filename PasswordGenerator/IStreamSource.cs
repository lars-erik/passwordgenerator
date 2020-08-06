using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PasswordGenerator
{
    public interface IStreamSource
    {
        StreamList GetStreams();
    }

    public class StreamList : IEnumerable<Stream>, IDisposable
    {
        private readonly Stream[] streams;

        public StreamList(params Stream[] streams)
        {
            this.streams = streams;
        }

        public IEnumerator<Stream> GetEnumerator()
        {
            return streams.Cast<Stream>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            foreach (var stream in streams)
            {
                stream?.Dispose();
            }
        }
    }
}