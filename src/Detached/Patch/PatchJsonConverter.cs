﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Detached.Patch
{
    public class PatchJsonConverter<TModel> : JsonConverter<TModel>
    {
        readonly PatchProxyTypeFactory _factory = new PatchProxyTypeFactory();

        public override TModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (TModel)JsonSerializer.Deserialize(ref reader, _factory.Create<TModel>(), options);
        }

        public override void Write(Utf8JsonWriter writer, TModel value, JsonSerializerOptions options)
        {
            JsonConverter<TModel> converter = (JsonConverter<TModel>)options.GetConverter(typeof(TModel));
            converter.Write(writer, value, options);
        }
    }
}