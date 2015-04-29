﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Framework.ConfigurationModel;

namespace PartsUnlimited.WebJobs.UpdateProductInventory
{
    public class Program
    {
        public int Main(string[] args)
        {
            var config = new Configuration().AddJsonFile("config.json");
            var webjobsConnectionString = config.Get("Data:AzureWebJobsStorage:ConnectionString");
            var dbConnectionString = config.Get("Data:DefaultConnection:ConnectionString");

            if (string.IsNullOrWhiteSpace(webjobsConnectionString))
            {
                Console.WriteLine("The configuration value for Azure Web Jobs Connection String is missing.");
                return 10;
            }

            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                Console.WriteLine("The configuration value for Database Connection String is missing.");
                return 10;
            }

            var jobHostConfig = new JobHostConfiguration(webjobsConnectionString);
            var host = new JobHost(jobHostConfig);

            host.RunAndBlock();
            return 0;
        }
    }
}
