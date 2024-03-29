// <copyright file="AchievementManager.cs" company="Google Inc.">
// Copyright (C) 2014 Google Inc. All Rights Reserved.
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

namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using GooglePlayGames.OurUtils;
    using GooglePlayGames.Native.Cwrapper;
    using C = GooglePlayGames.Native.Cwrapper.AchievementManager;

    internal class AchievementManager
    {

        private readonly GameServices mServices;

        internal AchievementManager(GameServices services)
        {
            mServices = Misc.CheckNotNull(services);
        }

        internal void ShowAllUI(Action<CommonErrorStatus.UIStatus> callback)
        {
            Misc.CheckNotNull(callback);
            C.AchievementManager_ShowAllUI(mServices.AsHandle(),
                Callbacks.InternalShowUICallback, Callbacks.ToIntPtr(callback));
        }

        internal void FetchAll(Action<FetchAllResponse> callback)
        {
            Misc.CheckNotNull(callback);

            C.AchievementManager_FetchAll(mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK,
                InternalFetchAllCallback,
                Callbacks.ToIntPtr<FetchAllResponse>(callback, FetchAllResponse.FromPointer));
        }

        [AOT.MonoPInvokeCallback(typeof(C.FetchAllCallback))]
        private static void InternalFetchAllCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("AchievementManager#InternalFetchAllCallback",
                Callbacks.Type.Temporary, response, data);
        }

        internal void Fetch(string achId, Action<FetchResponse> callback)
        {
            Misc.CheckNotNull(achId);
            Misc.CheckNotNull(callback);
            C.AchievementManager_Fetch(mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK,
                achId, InternalFetchCallback,
                Callbacks.ToIntPtr<FetchResponse>(callback, FetchResponse.FromPointer));
        }

        [AOT.MonoPInvokeCallback(typeof(C.FetchCallback))]
        private static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("AchievementManager#InternalFetchCallback",
                Callbacks.Type.Temporary, response, data);
        }

        internal void Increment(string achievementId, uint numSteps)
        {
            Misc.CheckNotNull(achievementId);

            C.AchievementManager_Increment(mServices.AsHandle(),
                achievementId, numSteps);
        }

        internal void SetStepsAtLeast(string achivementId, uint numSteps)
        {
            Misc.CheckNotNull(achivementId);

            C.AchievementManager_SetStepsAtLeast(mServices.AsHandle(),
                achivementId, numSteps);
        }

        internal void Reveal(string achievementId)
        {
            Misc.CheckNotNull(achievementId);

            C.AchievementManager_Reveal(mServices.AsHandle(), achievementId);
        }

        internal void Unlock(string achievementId)
        {
            Misc.CheckNotNull(achievementId);

            C.AchievementManager_Unlock(mServices.AsHandle(), achievementId);
        }

        internal class FetchResponse : BaseReferenceHolder
        {
            internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            internal CommonErrorStatus.ResponseStatus Status()
            {
                return C.AchievementManager_FetchResponse_GetStatus(SelfPtr());
            }

            internal NativeAchievement Achievement()
            {
                IntPtr p =  C.AchievementManager_FetchResponse_GetData(SelfPtr());
                return new NativeAchievement(p);
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                C.AchievementManager_FetchResponse_Dispose(selfPointer);
            }

            internal static FetchResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new FetchResponse(pointer);
            }
        }

        internal class FetchAllResponse : BaseReferenceHolder, IEnumerable<NativeAchievement>
        {

            internal FetchAllResponse(IntPtr selfPointer)
                : base(selfPointer)
            {
            }

            internal CommonErrorStatus.ResponseStatus Status()
            {
                return C.AchievementManager_FetchAllResponse_GetStatus(SelfPtr());
            }

            private UIntPtr Length()
            {
                return C.AchievementManager_FetchAllResponse_GetData_Length(SelfPtr());
            }

            private NativeAchievement GetElement(UIntPtr index)
            {
                if (index.ToUInt64() >= Length().ToUInt64())
                {
                    throw new ArgumentOutOfRangeException();
                }

                return new NativeAchievement(
                    C.AchievementManager_FetchAllResponse_GetData_GetElement(SelfPtr(), index));
            }

            public IEnumerator<NativeAchievement> GetEnumerator()
            {
                return PInvokeUtilities.ToEnumerator(
                    C.AchievementManager_FetchAllResponse_GetData_Length(SelfPtr()),
                    (index) => GetElement(index));
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                C.AchievementManager_FetchAllResponse_Dispose(selfPointer);
            }

            internal static FetchAllResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }

                return new FetchAllResponse(pointer);
            }
        }
    }
}


#endif
