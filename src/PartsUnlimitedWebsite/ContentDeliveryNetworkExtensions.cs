﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNet.Mvc.Rendering;
using PartsUnlimited.WebsiteConfiguration;
using System;
using System.Text;

namespace PartsUnlimited
{
    public static class ContentDeliveryNetworkExtensions
    {
        public static IContentDeliveryNetworkConfiguration Configuration { get; set; }

        public static HtmlString Image(this IHtmlHelper helper, string src, string alt = null)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentOutOfRangeException(nameof(src), src, "Must not be null or whitespace");
            }

            var img = new TagBuilder("img");

            img.MergeAttribute("src", GetCdnSource(src));

            if (!string.IsNullOrWhiteSpace(alt))
            {
                img.MergeAttribute("alt", alt);
            }

            return img.ToHtmlString(TagRenderMode.SelfClosing);
        }

        public static HtmlString ImageBackground(this IHtmlHelper helper, string src)
        {
            var cdnSource = GetCdnSource(src);
            return new HtmlString($"style = \"background-image: url('{cdnSource}')\"");
        }

        public static HtmlString Script(this IHtmlHelper helper, string contentPath)
        {
            if (string.IsNullOrWhiteSpace(contentPath))
            {
                throw new ArgumentOutOfRangeException(nameof(contentPath), contentPath, "Must not be null or whitespace");
            }

            var sb = new StringBuilder();
            var paths = Configuration == null ? new[] { contentPath } : Configuration.Scripts[contentPath];

            foreach (var path in paths)
            {
                var script = new TagBuilder("script");

                script.MergeAttribute("type", "text/javascript");
                script.MergeAttribute("src", path);

                sb.AppendLine(script.ToString());
            }

            return new HtmlString(sb.ToString());
        }

        public static HtmlString Styles(this IHtmlHelper helper, string contentPath)
        {
            if (string.IsNullOrWhiteSpace(contentPath))
            {
                throw new ArgumentOutOfRangeException(nameof(contentPath), contentPath, "Must not be null or whitespace");
            }

            var sb = new StringBuilder();
            var paths = Configuration == null ? new[] { contentPath } : Configuration.Styles[contentPath];

            foreach (var path in paths)
            {
                var script = new TagBuilder("link");

                script.MergeAttribute("rel", "stylesheet");
                script.MergeAttribute("href", path);

                sb.AppendLine(script.ToString());
            }

            return new HtmlString(sb.ToString());
        }

        private static string GetCdnSource(string src)
        {
            if (Configuration == null || string.IsNullOrWhiteSpace(Configuration.Images))
            {
                return src;
            }

            return string.Format("{0}/{1}", Configuration.Images, src);
        }
    }
}
