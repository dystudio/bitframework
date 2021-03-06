﻿using Bit.Tooling.Core.Contracts;
using Bit.Tooling.Core.Model;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Bit.Tooling.CodeGenerator.Implementations
{
    [Serializable]
    public class BitConfigNotFoundException : Exception
    {
        public BitConfigNotFoundException(string message)
            : base(message)
        {

        }

        public BitConfigNotFoundException()
        {
        }

        public BitConfigNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BitConfigNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {

        }
    }

    public abstract class BitConfigProviderBase : IBitConfigProvider
    {
        public abstract string GetBitConfigFilePath();

        public virtual BitConfig GetConfiguration()
        {
            BitConfig bitConfig = JsonConvert
                .DeserializeObject<BitConfig>(
                    File.ReadAllText(GetBitConfigFilePath()));

            foreach (BitCodeGeneratorMapping mapping in bitConfig.BitCodeGeneratorConfigs.BitCodeGeneratorMappings)
            {
                if (string.IsNullOrEmpty(mapping.DestinationFileName))
                    throw new InvalidOperationException($"{nameof(BitCodeGeneratorMapping.DestinationFileName)} is not provided");

                if (string.IsNullOrEmpty(mapping.DestinationProject?.Name))
                    throw new InvalidOperationException($"{nameof(BitCodeGeneratorMapping.DestinationProject)} is not provided");

                if (mapping.SourceProjects == null || !mapping.SourceProjects.Any() || !mapping.SourceProjects.All(sp => !string.IsNullOrEmpty(sp?.Name)))
                    throw new InvalidOperationException($"No {nameof(BitCodeGeneratorMapping.SourceProjects)} is not provided");

                if (string.IsNullOrEmpty(mapping.Route))
                    throw new InvalidOperationException($"{nameof(BitCodeGeneratorMapping.Route)} is not provided");
            }

            return bitConfig;
        }
    }
}
