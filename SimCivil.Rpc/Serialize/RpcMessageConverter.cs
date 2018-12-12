// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Rpc - RpcMessageConverter.cs
// Create Date: 2018/01/05
// Update Date: 2018/12/11

using System;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimCivil.Rpc.Serialize
{
    class RpcMessageConverter : JsonConverter
    {
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObject = new JObject();
            switch (value)
            {
                case RpcRequest request:
                    jObject.Add("Sequence", request.Sequence);
                    jObject.Add("MethodName", request.MethodName);
                    jObject.Add("ServiceName", request.ServiceName);
                    jObject.Add("TimeStamp", JToken.FromObject(request.TimeStamp, serializer));
                    jObject.Add("Arguments", JToken.FromObject(request.Arguments, serializer));
                    jObject.Add("$type", "SimCivil.Rpc.RpcRequest, SimCivil.Rpc");

                    break;
                case RpcResponse response:
                    jObject.Add("Sequence", response.Sequence);
                    jObject.Add("TimeStamp", JToken.FromObject(response.TimeStamp, serializer));
                    if (response.ErrorInfo != null)
                    {
                        jObject.Add("ErrorInfo", response.ErrorInfo);
                    }
                    else if (response.ReturnValue != null)
                    {
                        jObject.Add("ReturnValue", JToken.FromObject(response.ReturnValue, serializer));
                    }

                    jObject.Add("$type", "SimCivil.Rpc.RpcResponse, SimCivil.Rpc");

                    break;
                default:

                    throw new NotSupportedException();
            }

            jObject.WriteTo(writer);
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            JObject o = JObject.Load(reader);
            if (objectType == typeof(RpcRequest))
            {
                var request = new RpcRequest
                {
                    Sequence = o.Value<long>("Sequence"),
                    Arguments = o["Arguments"].Cast<object>().ToArray(),
                    MethodName = o.Value<string>("MethodName"),
                    ServiceName = o.Value<string>("ServiceName"),
                    TimeStamp = o.GetValue("TimeStamp").ToObject<DateTime>(serializer)
                };

                return request;
            }
            else if (objectType == typeof(RpcResponse))
            {
                var response = new RpcResponse()
                {
                    Sequence = o.Value<long>("Sequence"),
                    TimeStamp = o.GetValue("TimeStamp").ToObject<DateTime>(serializer)
                };
                if (o.ContainsKey("ErrorInfo"))
                {
                    response.ErrorInfo = o.Value<string>("ErrorInfo");
                }

                if (o.ContainsKey("ReturnValue"))
                {
                    response.ReturnValue = o["ReturnValue"];
                }

                return response;
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(RpcRequest) == objectType || typeof(RpcResponse) == objectType;
        }
    }
}