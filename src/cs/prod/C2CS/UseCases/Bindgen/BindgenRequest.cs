// Copyright (c) Lucas Girouard-Stranks (https://github.com/lithiumtoast). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

namespace C2CS.UseCases.Bindgen
{
    public class BindgenRequest
    {
        public readonly BindgenConfiguration Configuration;

        public BindgenRequest(BindgenConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}