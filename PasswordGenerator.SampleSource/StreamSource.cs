using System;

namespace PasswordGenerator.SampleSource
{
    public class StreamSource : IStreamSource
    {
        public StreamList GetStreams()
        {
            return new StreamList(
                GetType().Assembly.GetManifestResourceStream("PasswordGenerator.SampleSource.Resources.adjectives.txt"), 
                GetType().Assembly.GetManifestResourceStream("PasswordGenerator.SampleSource.Resources.nouns.txt")
            );
        }
    }
}
