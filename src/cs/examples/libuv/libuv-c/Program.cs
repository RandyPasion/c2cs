﻿// Copyright (c) Lucas Girouard-Stranks (https://github.com/lithiumtoast). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the Git repository root directory for full license information.

using System;
using System.IO;
using C2CS;

internal static class Program
{
    private static void Main()
    {
        var rootDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../../.."));
        GenerateLibraryBindings(rootDirectory);
        // BuildLibrary(rootDirectory);
    }

    private static void BuildLibrary(string rootDirectory)
    {
        var cMakeDirectoryPath = Path.Combine(rootDirectory, "ext/libuv");
        var targetLibraryDirectoryPath = $"{rootDirectory}/src/cs/examples/libuv/libuv-cs/";
        var isSuccess = Shell.CMake(rootDirectory, cMakeDirectoryPath, targetLibraryDirectoryPath);
        if (!isSuccess)
        {
            Environment.Exit(1);
        }
    }

    private static void GenerateLibraryBindings(string rootDirectory)
    {
        var arguments = @$"
-i
{rootDirectory}/ext/libuv/include/uv.h
-s
{rootDirectory}/ext/libuv/include
-o
{rootDirectory}/src/cs/examples/libuv/libuv-cs/uv.cs
-u
-f
-t
-l
uv
-c
uv
--opaqueTypes
uv_loop_t
uv_handle_t
uv_dir_t
uv_stream_t
uv_tcp_t
uv_udp_t
uv_pipe_t
uv_tty_t
uv_poll_t
uv_timer_t
uv_prepare_t
uv_check_t
uv_idle_t
uv_async_t
uv_process_t;
uv_fs_event_t
uv_fs_poll_t
uv_signal_t
uv_req_t
uv_random_t
uv_getaddrinfo_t
uv_getnameinfo_t
uv_shutdown_t
uv_write_t
uv_connect_t
uv_udp_send_t
uv_fs_t
uv_work_t
uv_key_t
uv_once_t
uv_mutex_t
uv_rwlock_t
uv_sem_t
uv_cond_t
uv_barrier_t
_opaque_pthread_t
FILE
";

        var argumentsArray =
            arguments.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        C2CS.Program.Main(argumentsArray);
    }
}