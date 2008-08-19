using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                object result = context.Locator.Get(context.BuildKey);
                if (result != null)
                {
                    context.Existing = result;
                    context.BuildComplete = true;
                }
            }
        }
    }
}
