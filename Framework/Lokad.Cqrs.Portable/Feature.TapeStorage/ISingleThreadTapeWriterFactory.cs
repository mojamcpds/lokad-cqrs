﻿namespace Lokad.Cqrs.Feature.TapeStorage
{
    public interface ISingleThreadTapeWriterFactory
    {

        /// <summary>
        /// Initializes this factory, if needed.
        /// </summary>
        void Init();
        ISingleThreadTapeWriter GetOrCreateWriter(string name);
        //void TryDeleteWriter(string name);
    }
}