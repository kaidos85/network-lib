using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class BaseNetWork
    {
        #region Property
        protected NetworkStream stream;
        protected Byte[] data = null;
        protected byte[] buffer;
        protected int buffer_size = 256;
        public event NetworkReadHandler handler;
        #endregion

        public BaseNetWork()
        {
            buffer = new byte[buffer_size];
        }

        protected virtual void Init()
        {
            throw new Exception("Not overrided.");
        }

        public void Write(string Text)
        {
            Init();
            if (stream != null && stream.CanRead)
            {
                UTF8Encoding encoding = new UTF8Encoding();
                data = encoding.GetBytes(Text);

                if (stream.CanWrite)
                    stream.BeginWrite(data, 0, data.Length,
                        new AsyncCallback(AsyncWriteResult), stream);
            }          
        }

        public void Read()
        {
            Init();
            if (stream != null && stream.CanRead)
            {
                stream.BeginRead(buffer, 0, buffer.Length,
                    new AsyncCallback(ASyncReadResult), stream);
            }
        }

        public void ASyncReadResult(IAsyncResult result)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            NetworkStream streamRead = (NetworkStream)result.AsyncState;

            if (result == null)
                return;
            string text = String.Empty;
            int read = 0;
            if (streamRead.CanRead)
                read = streamRead.EndRead(result);
            if (read > 0)
            {
                text += encoding.GetString(buffer, 0, read);
                streamRead.BeginRead(buffer, 0, buffer_size, ASyncReadResult, streamRead);
            }
            OnRaiseTextChanged(text);
        }

        public void AsyncWriteResult(IAsyncResult result)
        {
            NetworkStream streamWrite = (NetworkStream)result.AsyncState;
            streamWrite.EndWrite(result);
        }

        protected virtual void OnRaiseTextChanged(string text)
        {
            if (handler != null)
                handler(this, new NetworkReadEventArgs(text));
        }

    }
}
