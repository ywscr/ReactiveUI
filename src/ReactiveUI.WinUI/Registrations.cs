﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using Splat;

namespace ReactiveUI.WinUI
{
    /// <summary>
    /// Registrations specific to the WPF platform.
    /// </summary>
    public class Registrations : IWantsToRegisterStuff
    {
        /// <inheritdoc/>
        public void Register(Action<Func<object>, Type> registerFunction)
        {
            if (registerFunction is null)
            {
                throw new ArgumentNullException(nameof(registerFunction));
            }

            registerFunction(() => new PlatformOperations(), typeof(IPlatformOperations));
            registerFunction(() => new ActivationForViewFetcher(), typeof(IActivationForViewFetcher));
            registerFunction(() => new DependencyObjectObservableForProperty(), typeof(ICreatesObservableForProperty));
            registerFunction(() => new BooleanToVisibilityTypeConverter(), typeof(IBindingTypeConverter));
            registerFunction(() => new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
            registerFunction(() => new ComponentModelTypeConverter(), typeof(IBindingTypeConverter));

            if (!ModeDetector.InUnitTestRunner())
            {
                // NB: On .NET Core, trying to touch DispatcherScheduler blows up :cry:
                RxApp.MainThreadScheduler = new WaitForDispatcherScheduler(() => { return DispatcherQueueScheduler.Current; });
                RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;
            }

            RxApp.SuppressViewCommandBindingMessage = true;
        }
    }
}