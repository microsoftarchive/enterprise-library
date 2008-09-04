using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
    /// <summary>
    /// A small class that looks for the currently requested
    /// object in the locator, and if found, returns it,
    /// short circuiting the rest of the chain.
    /// </summary>
    /// <remarks>This is essentially the same as the
    /// old OB1 SingletonStrategy. Note that this
    /// strategy doesn't actually put anything in the
    /// locator, which is part of why the name changed.
    /// </remarks>
    public class LocatorLookupStrategy : BuilderStrategy
    {
        /// <summary>
        /// Execute the pre-buildup operation. For this strategy,
        /// this means looking up the current build key in the locator.
        /// If found, abort the build.
        /// </summary>
        /// <param name="context">Current <see cref="IBuilderContext"/>.</param>
        public override void PreBuildUp(IBuilderContext context)
        {
            if (context.Locator != null)
            {
                Monitor.Enter(context.Locator);
                context.RecoveryStack.Add(new LockReleaser(context.Locator));
                object result = context.Locator.Get(context.BuildKey);
                if (result != null)
                {
                    context.Existing = result;
                    context.BuildComplete = true;
                    Monitor.Exit(context.Locator);
                }
            }
        }

        /// <summary>
        ///             Called during the chain of responsibility for a build operation. The
        ///             PostBuildUp method is called when the chain has finished the PreBuildUp
        ///             phase and executes in reverse order from the PreBuildUp calls.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PostBuildUp(IBuilderContext context)
        {
            if(context.Locator != null)
            {
                Monitor.Exit(context.Locator);
            }
        }

        private class LockReleaser : IRequiresRecovery
        {
            private readonly object lockToRelease;

            public LockReleaser(object lockToRelease)
            {
                this.lockToRelease = lockToRelease;
            }

            public void Recover()
            {
                try
                {
                    Monitor.Exit(lockToRelease);
                }
                catch (SynchronizationLockException)
                {
                    // Ok if this happens, means lock was exitted early
                }
            }
        }
    }
}
