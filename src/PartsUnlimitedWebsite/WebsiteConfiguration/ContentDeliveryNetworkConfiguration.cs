﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Framework.ConfigurationModel;
using System.Linq;

namespace PartsUnlimited.WebsiteConfiguration
{
    public class ContentDeliveryNetworkConfiguration : IContentDeliveryNetworkConfiguration
    {
        public ContentDeliveryNetworkConfiguration(IConfiguration config)
        {
            Images = config.Get("images");
            Scripts = config.GetSubKey("Scripts").ToLookup();
            Styles = config.GetSubKey("Styles").ToLookup();
        }

        public string Images { get; }
        public ILookup<string, string> Scripts { get; }
        public ILookup<string, string> Styles { get; }
    }
}