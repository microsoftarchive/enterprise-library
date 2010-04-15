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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> extensions to skip over exceptions produced by enumerables and,
    /// optionally, provide an <see cref="Action"/> delegate for handling those exceptions.
    /// </summary>
    public static class SafeEnumerableExtensions
    {
        /// <summary>
        /// Filters out exceptions produced by <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns>An enumerable that filters exceptions from <paramref name="source"/></returns>
        public static IEnumerable<TSource> WithNoExceptions<TSource>(this IEnumerable<TSource> source)
        {
            return new SafeIterator<TSource>(source);
        }

        /// <summary>
        /// Filters out exceptions produced by <see cref="IEnumerable{T}"/> and calls back to an <see cref="Action{Exception}"/> 
        /// for handling errors.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> WithNoExceptions<TSource>(this IEnumerable<TSource> source, Action<Exception> exceptionAction)
        {
            return new SafeIterator<TSource>(source, exceptionAction);
        }
    
        /// <summary>
        /// Filters out exceptions occurring from <paramref name="selector"/> during selection.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectSafe<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return new SelectIterator<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Filters out exceptions occurring from <paramref name="selector"/> during selection and calls back to an <see cref="Action{Exception}"/>
        /// when an error is encountered.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        /// <seealso cref="Enumerable.Select{TSource,TResult}(System.Collections.Generic.IEnumerable{TSource},System.Func{TSource,TResult})"/>
        public static IEnumerable<TResult> SelectSafe<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, Action<Exception> exceptionAction)
        {
            return new SelectIterator<TSource, TResult>(source, selector, exceptionAction);
        }

        /// <summary>
        /// Filters out exceptions occurring from <paramref name="selector"/> during SelectMany.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <seealso cref="Enumerable.SelectMany{TSource,TResult}(System.Collections.Generic.IEnumerable{TSource},System.Func{TSource,System.Collections.Generic.IEnumerable{TResult}})"/>
        public static IEnumerable<TResult> SelectManySafe<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return new SelectManyIterator<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Filters out exceptions occurring from <paramref name="selector"/> during SelectMany.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        /// <seealso cref="Enumerable.SelectMany{TSource,TResult}(System.Collections.Generic.IEnumerable{TSource},System.Func{TSource,System.Collections.Generic.IEnumerable{TResult}})"/>
        public static IEnumerable<TResult> SelectManySafe<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector, Action<Exception> exceptionAction)
        {
            return new SelectManyIterator<TSource, TResult>(source, selector, exceptionAction);
        }

        private class SelectIterator<TSource, TResult> : IEnumerable<TResult>, IEnumerator<TResult>
        {
            private readonly IEnumerable<TSource> source;
            private readonly Func<TSource, TResult> selector;
            private TResult current;
            private IEnumerator<TSource> enumerator;
            private readonly Action<Exception> exceptionAction;

            public SelectIterator(IEnumerable<TSource> source, Func<TSource, TResult> selector, Action<Exception> exceptionAction)
            {
                this.source = source;
                this.selector = selector;
                this.exceptionAction = exceptionAction;
            }

            public SelectIterator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
                :this(source, selector, (e) => {})
            {
            }

            public IEnumerator<TResult> GetEnumerator()
            {
                return new SelectIterator<TSource, TResult>(source, selector, exceptionAction);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                this.current = default(TResult);

                GC.SuppressFinalize(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            public bool MoveNext()
            {
                if (enumerator == null)
                {
                    enumerator = source.GetEnumerator();
                }

                while(enumerator.MoveNext())
                {
                    try
                    {
                        current = selector(enumerator.Current);
                        return true;
                    }
                    catch(Exception ex)
                    {
                        exceptionAction(ex);
                    }
                }

                return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public TResult Current
            {
                get { return this.current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        private class SelectManyIterator<TSource, TResult> : IEnumerable<TResult>, IEnumerator<TResult>
        {
            private readonly IEnumerable<TSource> source;
            private readonly Func<TSource, IEnumerable<TResult>> selector;
            private IEnumerator<TSource> outerEnumerator;
            private TResult current;
            private IEnumerator<TResult> innerEnumerator;
            private readonly Action<Exception> exceptionAction;

            public SelectManyIterator(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
                :this (source, selector, (e) => { })
            {
            }

            public SelectManyIterator(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector, Action<Exception> exceptionAction)
            {
                this.source = source;
                this.selector = selector;
                this.exceptionAction = exceptionAction;
            }
            
            public IEnumerator<TResult> GetEnumerator()
            {
                return new SelectManyIterator<TSource, TResult>(source, selector, exceptionAction);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Dispose()
            {
                if (outerEnumerator != null)
                {
                    outerEnumerator.Dispose();
                    outerEnumerator = null;
                }

                if (innerEnumerator != null)
                {
                    innerEnumerator.Dispose();
                    innerEnumerator = null;
                }

                this.current = default(TResult);

                GC.SuppressFinalize(this);
            }

            public bool MoveNext()
            {
                if (this.outerEnumerator == null)
                {
                    this.outerEnumerator = source.GetEnumerator();
                }

                while (true)
                {
                    if (innerEnumerator != null && innerEnumerator.MoveNext())
                    {
                        this.current = innerEnumerator.Current;
                        return true;
                    }
                    else
                    {
                        if (!MoveToNextInnerEnumerator())
                        {
                            break;
                        }
                    }
                }

                return false;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            private bool MoveToNextInnerEnumerator()
            {
                IEnumerable<TResult> innerEnumerable = null;
                while (innerEnumerable == null)
                {
                    if (!outerEnumerator.MoveNext())
                    {
                        return false;
                    }

                    if (innerEnumerator != null) innerEnumerator.Dispose();

                    try
                    {
                        innerEnumerable = selector(outerEnumerator.Current);
                        innerEnumerator = new SafeIterator<TResult>(innerEnumerable, exceptionAction);
                    }
                    catch(Exception ex)
                    {
                        exceptionAction(ex);
                    }
                }

                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public TResult Current
            {
                get { return current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        private class SafeIterator<T> : IEnumerable<T>, IEnumerator<T>
        {
            private readonly IEnumerable<T> source;
            private IEnumerator<T> enumerator;
            private T current;
            private Action<Exception> actionException;

            public SafeIterator(IEnumerable<T> source, Action<Exception> actionException)
            {
                this.source = source;
                this.actionException = actionException;
            }
            
            
            public SafeIterator(IEnumerable<T> source) : this(source, (e) => { })
            {
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.Clone();
            }

            private IEnumerator<T> Clone()
            {
                return new SafeIterator<T>(source, actionException);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                this.current = default(T);

                GC.SuppressFinalize(this);
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            public bool MoveNext()
            {

                if (enumerator == null)
                {
                    enumerator = source.GetEnumerator();
                }

                bool notFinished = true;
                while (notFinished)
                {
                    try
                    {
                        if (enumerator.MoveNext())
                        {
                            this.current = enumerator.Current;
                            return true;
                        }
                        else
                        {
                            notFinished = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        actionException(ex);
                    }
                }

                return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public T Current
            {
                get { return current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
