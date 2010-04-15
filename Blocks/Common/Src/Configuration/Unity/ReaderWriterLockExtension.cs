//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System.Threading;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
    ///<summary>
    /// Provides Reader/Writer lock logic to allow reconfiguring of the container.
    ///</summary>
    public class ReaderWriterLockExtension : UnityContainerExtension
    {
        private ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Initial the container with this extension's functionality.
        /// </summary>
        /// <remarks>
        /// When overridden in a derived class, this method will modify the given
        ///             <see cref="T:Microsoft.Practices.Unity.ExtensionContext"/> by adding strategies, policies, and so forth. to
        ///             install it's functions into the container.
        /// </remarks>
        protected override void Initialize()
        {
            Context.Strategies.Add(new ReaderWriterLockStrategy(readerWriterLock), UnityBuildStage.Setup);    
        }

        ///<summary>
        ///</summary>
        public void EnterWriteLock()
        {
            readerWriterLock.EnterWriteLock();
        }

        ///<summary>
        ///</summary>
        public void ExitWriteLock()
        {
            readerWriterLock.ExitWriteLock();
        }

        class ReaderWriterLockStrategy : BuilderStrategy
        {
            private ReaderWriterLockSlim internalLock;

            public ReaderWriterLockStrategy(ReaderWriterLockSlim readerWriterLock)
            {
                internalLock = readerWriterLock;
            }

            /// <summary>
            /// Called during the chain of responsibility for a build operation. The
            ///             PreBuildUp method is called when the chain is being executed in the
            ///             forward direction.
            /// </summary>
            /// <param name="context">Context of the build operation.</param>
            public override void PreBuildUp(IBuilderContext context)
            {
                context.RecoveryStack.Add(new ReaderWriterLockCompensator(internalLock));
                internalLock.EnterReadLock();
            }

            /// <summary>
            /// Called during the chain of responsibility for a build operation. The
            ///             PostBuildUp method is called when the chain has finished the PreBuildUp
            ///             phase and executes in reverse order from the PreBuildUp calls.
            /// </summary>
            /// <param name="context">Context of the build operation.</param>
            public override void PostBuildUp(IBuilderContext context)
            {
                internalLock.ExitReadLock();
            }

            class ReaderWriterLockCompensator : IRequiresRecovery
            {
                private readonly ReaderWriterLockSlim _internalLock;

                public ReaderWriterLockCompensator(ReaderWriterLockSlim internalLock)
                {
                    _internalLock = internalLock;
                }

                public void Recover()
                {
                    _internalLock.ExitReadLock();
                }
            }
        }
    }

    
}
