using PTZ.HomeManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PTZ.HomeManagement
{
    public class CoreService : ICoreService
    {
        public List<string> GetLastNMessages(int qtdOfMessages, string filename = null)
        {
            List<string> list = new List<string>();
            using (TextReader reader = File.OpenText(filename))
            {
                list.AddRange(Tail(reader, qtdOfMessages));
            }

            return list;
        }

        private IEnumerable<string> Tail(TextReader reader, int lineCount)
        {
            var buffer = new List<string>(lineCount);
            string line;
            for (int i = 0; i < lineCount; i++)
            {
                line = reader.ReadLine();
                if (line == null) return buffer.ToArray();
                buffer.Add(line);
            }

            int lastLine = lineCount - 1;           //The index of the last line read from the buffer.  Everything > this index was read earlier than everything <= this indes

            while (null != (line = reader.ReadLine()))
            {
                lastLine++;
                if (lastLine == lineCount) lastLine = 0;
                buffer[lastLine] = line;
            }

            if (lastLine == lineCount - 1) return buffer.ToArray();
            var retVal = new string[lineCount];
            buffer.CopyTo(lastLine + 1, retVal, 0, lineCount - lastLine - 1);
            buffer.CopyTo(0, retVal, lineCount - lastLine - 1, lastLine + 1);
            return retVal;
        }
    }
}
