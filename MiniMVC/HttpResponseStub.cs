﻿#region license
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

using System.Text;
using System.Web;

namespace MiniMVC {
    public class HttpResponseStub : HttpResponseBase {
        private readonly StringBuilder sb = new StringBuilder();

        public override void Write(char ch) {
            sb.Append(ch);
        }

        public override void Write(object obj) {
            sb.Append(obj);
        }

        public override void Write(string s) {
            sb.Append(s);
        }

        public override string ToString() {
            return sb.ToString();
        }
    }
}