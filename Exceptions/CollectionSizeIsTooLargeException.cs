using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Exceptions
{
    public class CollectionSizeIsTooLargeException : Exception
    {
        public int ActualCollectionSize { get; }

        public int MaximumCollectionSize { get; }

        public CollectionSizeIsTooLargeException(int actualCollectionSize, int maximumCollectionSize) : base($"Превышен максимальный размер коллекции: {actualCollectionSize} > {maximumCollectionSize}") 
        {
            ActualCollectionSize = actualCollectionSize;
            MaximumCollectionSize = maximumCollectionSize;
        }
    }
}
