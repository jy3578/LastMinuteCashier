// <copyright file="GameServices.cs" company="Google Inc.">
// Copyright (C) 2014 Google Inc.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))

namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class GameServices
    {
        internal delegate void FlushCallback(
        /* from(FlushStatus_t) */ CommonErrorStatus.FlushStatus arg0,
        /* from(void *) */ IntPtr arg1);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void GameServices_Flush(
            HandleRef self,
         /* from(GameServices_FlushCallback_t) */FlushCallback callback,
         /* from(void *) */IntPtr callback_arg);

        internal delegate void FetchServerAuthCodeCallback(
            /* from(FetchServerAuthCodeResponse_t) */ IntPtr arg0,
            /* from(void *) */ IntPtr arg1);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void GameServices_FetchServerAuthCode(
            HandleRef self,
            /* from(char const *) */string server_client_id,
            /* from(GameServices_FetchServerAuthCodeCallback_t) */FetchServerAuthCodeCallback callback,
            /* from(void *) */IntPtr callback_arg);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern /* from(bool) */ bool GameServices_IsAuthorized(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void GameServices_Dispose(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void GameServices_SignOut(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void GameServices_StartAuthorizationUI(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void GameServices_FetchServerAuthCodeResponse_Dispose (
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(ResponseStatus_t) */ CommonErrorStatus.ResponseStatus GameServices_FetchServerAuthCodeResponse_GetStatus (
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(size_t) */ UIntPtr GameServices_FetchServerAuthCodeResponse_GetCode (
            HandleRef self,
            /* from(char *) */StringBuilder out_arg,
            /* from(size_t) */UIntPtr out_size);
    }
}
#endif // (UNITY_ANDROID || UNITY_IPHONE)
