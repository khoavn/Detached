﻿using Detached.Model;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Detached.Patch
{
    public class PatchJsonConverterFactory : JsonConverterFactory
    {
        readonly ModelOptions _options;

        public PatchJsonConverterFactory(IOptions<ModelOptions> options)
        {
            _options = options.Value;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return _options.GetTypeOptions(typeToConvert).UsePatchProxy;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(typeof(PatchJsonConverter<>).MakeGenericType(typeToConvert));
        }
    }
}