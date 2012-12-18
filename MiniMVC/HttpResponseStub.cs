#region license

// Copyright (c) 2009 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.IO;
using System.Text;
using System.Web;

namespace MiniMVC {
    public class HttpResponseStub : HttpResponseBase {
        private readonly MemoryStream output = new MemoryStream();
        private readonly StreamWriter writer;

        public HttpResponseStub() {
            writer = new StreamWriter(output);
        }

        public override void Write(char ch) {
            writer.Write(ch);
        }

        public override void Write(object obj) {
            writer.Write(obj);
        }

        public override void Write(string s) {
            writer.Write(s);
        }

        public override string ToString() {
            Flush();
            output.Position = 0;
            return Encoding.UTF8.GetString(output.ToArray());
        }

        public override TextWriter Output {
            get { return writer; }
        }

        public override Stream OutputStream {
            get { return output; }
        }

        public override void Flush() {
            writer.Flush();
            output.Flush();
        }

        public override Encoding ContentEncoding { get; set; }

        public override bool Buffer { get; set; }

        public override bool BufferOutput { get; set; }

        public override string CacheControl { get; set; }

        public override string Charset { get; set; }

        public override string ContentType { get; set; }

        public override int Expires { get; set; }

        public override DateTime ExpiresAbsolute { get; set; }

        public override Stream Filter { get; set; }

        private bool clientConnected;

        public void SetClientConnected(bool connected) {
            clientConnected = connected;
        }

        public override bool IsClientConnected {
            get { return clientConnected; }
        }

        public override Encoding HeaderEncoding { get; set; }

        public override string RedirectLocation { get; set; }

        public override bool TrySkipIisCustomErrors { get; set; }

        public override bool SuppressContent { get; set; }

        public override string Status { get; set; }

        public override int StatusCode { get; set; }

        public override string StatusDescription { get; set; }

        public override int SubStatusCode { get; set; }
    }
}