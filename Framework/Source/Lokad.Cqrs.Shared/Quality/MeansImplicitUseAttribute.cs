#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Should be used on attributes and causes ReSharper to not mark symbols marked with such attributes as unused (as well as by other usage inspections)
    /// </summary>
    /// <remarks>This attribute helps R# in code analysis</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [NoCodeCoverage]
    public sealed class MeansImplicitUseAttribute : Attribute
    {
        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <value>The flags.</value>
        [UsedImplicitly]
        public ImplicitUseFlags Flags { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeansImplicitUseAttribute"/> class with <see cref="ImplicitUseFlags.STANDARD"/>.
        /// </summary>
        [UsedImplicitly]
        public MeansImplicitUseAttribute()
            : this(ImplicitUseFlags.STANDARD)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeansImplicitUseAttribute"/> class.
        /// </summary>
        /// <param name="flags">The flags.</param>
        [UsedImplicitly]
        public MeansImplicitUseAttribute(ImplicitUseFlags flags)
        {
            Flags = flags;
        }
    }
}