﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Framework.ConfigurationModel;

namespace PartsUnlimited.WebJobs.ProcessOrder
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

            var jobHostConfig = new JobHostConfiguration(config.Get("Data:AzureWebJobsStorage:ConnectionString"));
            var host = new JobHost(jobHostConfig);
            var methodInfo = typeof(Functions).GetMethods().First();

            host.Call(methodInfo);
            return 0;
        }
    }
}
