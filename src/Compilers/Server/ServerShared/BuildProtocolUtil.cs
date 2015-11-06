﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.CodeAnalysis.CompilerServer
{
    internal static class BuildProtocolUtil
    {
        internal static RunRequest GetRunRequest(BuildRequest req)
        {
            string currentDirectory;
            string libDirectory;
            string[] arguments = GetCommandLineArguments(req, out currentDirectory, out libDirectory);
            string language = "";
            switch (req.Language)
            {
                case BuildProtocolConstants.RequestLanguage.CSharpCompile:
                    language = LanguageNames.CSharp;
                    break;
                case BuildProtocolConstants.RequestLanguage.VisualBasicCompile:
                    language = LanguageNames.VisualBasic;
                    break;
            }

            return new RunRequest(language, currentDirectory, libDirectory, arguments);
        }

        internal static string[] GetCommandLineArguments(BuildRequest req, out string currentDirectory, out string libDirectory)
        {
            currentDirectory = null;
            libDirectory = null;
            List<string> commandLineArguments = new List<string>();

            foreach (BuildRequest.Argument arg in req.Arguments)
            {
                if (arg.ArgumentId == BuildProtocolConstants.ArgumentId.CurrentDirectory)
                {
                    currentDirectory = arg.Value;
                }
                else if (arg.ArgumentId == BuildProtocolConstants.ArgumentId.LibEnvVariable)
                {
                    libDirectory = arg.Value;
                }
                else if (arg.ArgumentId == BuildProtocolConstants.ArgumentId.CommandLineArgument)
                {
                    int argIndex = arg.ArgumentIndex;
                    while (argIndex >= commandLineArguments.Count)
                        commandLineArguments.Add("");
                    commandLineArguments[argIndex] = arg.Value;
                }
            }

            return commandLineArguments.ToArray();
        }
    }
}
