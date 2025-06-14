using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Enums
{
    /// <summary>
    /// Тип конвертации в null
    /// </summary>
    public enum NullableTypeEnum
    {
        /// <summary>
        /// Не может быть null
        /// </summary>
        NotNullable = 0,
        /// <summary>
        /// Может быть null
        /// </summary>
        Nullable = 1
    }
}
