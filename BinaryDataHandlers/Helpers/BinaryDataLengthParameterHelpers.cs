using BinarySerializerLibrary.Base;
using BinarySerializerLibrary.Enums;
using BinarySerializerLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BinarySerializerLibrary.BinaryDataHandlers.Helpers
{
    public static class BinaryDataLengthParameterHelpers
    {
        public const int MaxDataArrayLengthFieldSize = 4;

        public static void PackBinaryCollectionSize(ABinaryDataWriter binaryWriter)
        {
            byte[] data;

            if (binaryWriter.BytesCount <= 0x7FFD)
            {
                data = new byte[2];

                // Модификатор поля размера массива данных
                ByteVectorHandler.SetVectorParamValue(data, 1, 0, 0);
                // Размер массива данных
                ByteVectorHandler.SetVectorParamValue(data, 15, 1, (ulong)binaryWriter.BytesCount + 2);
            }
            else if (binaryWriter.BytesCount <= 0x7FFFFFFB)
            {
                data = new byte[4];
                // Модификатор поля размера массива данных
                ByteVectorHandler.SetVectorParamValue(data, 1, 0, 1);
                // Размер массива данных
                ByteVectorHandler.SetVectorParamValue(data, 31, 1, (ulong)binaryWriter.BytesCount + 4);
            }
            else
            {
                throw new ByteArrayIsOutOfMaximumLength();
            }
            // Добавление размера бинарной коллекции в объект записи
            for (int index = data.Length - 1; index >= 0; index--)
            {
                binaryWriter.AppendByteToHead(data[index]);
            }
        }
        /// <summary>
        /// Проверить, что размер коллекции может быть прочитан
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static bool CheckIfBinaryCollectionSizeCanBeRead(ABinaryDataReader? binaryReader) => UnpackBinaryCollectionSizeWithoutShifting(binaryReader) is not null;
        /// <summary>
        /// Распаковать длину массива данных с учетом длины параметра размера
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="bitLocationIndex"></param>
        /// <returns></returns>
        public static int? UnpackBinaryCollectionSizeWithoutShifting(ABinaryDataReader? binaryReader) => _UnpackCollectionSize(binaryReader, false);
        /// <summary>
        /// Распаковать длину исходного массива данных и сдвинуться на размер параметра длины
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="bitLocationIndex"></param>
        /// <returns></returns>
        public static int? UnpackOriginObjectBinaryCollectionSizeWithShifting(ABinaryDataReader? binaryReader) => _UnpackCollectionSize(binaryReader, true);
        /// <summary>
        /// Распаковать длину массива данных
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="shiftingFlag"></param>
        /// <returns></returns>
        private static int? _UnpackCollectionSize(ABinaryDataReader? binaryReader, bool shiftingFlag)
        {
            if (binaryReader is null
                || binaryReader.IsEndOfCollection)
            {
                return null;
            }
            else
            {
                long beforeInvokeReaderLocation = binaryReader.BitIndex;

                try
                {
                    // Модификатор поля размера массива данных
                    var sizeModificationBitValue = binaryReader.ReadValue(1, BinaryAlignmentTypeEnum.ByteAlignment);
                    // Размер коллекции
                    int? collectionSize = null;
                    // Значение компенсации размера
                    int sizeParamCompensation = 0;

                    if (sizeModificationBitValue == 0)
                    {
                        collectionSize = (int)binaryReader.ReadValue(15, BinaryAlignmentTypeEnum.NoAlignment);

                        sizeParamCompensation = 2;
                    }
                    else
                    {
                        collectionSize = (int)binaryReader.ReadValue(31, BinaryAlignmentTypeEnum.NoAlignment);

                        sizeParamCompensation = 4;
                    }

                    if (!shiftingFlag)
                    {
                        binaryReader.SetBitIndex(beforeInvokeReaderLocation);
                    }
                    else
                    {
                        // В случае, если осуществляется обращение к параметру со сдвигом, то вычитаем размер параметра длины вектора данных
                        collectionSize -= sizeParamCompensation;
                    }

                    return collectionSize;
                }
                catch
                {
                    if (!shiftingFlag)
                    {
                        binaryReader.SetBitIndex(beforeInvokeReaderLocation);
                    }

                    return null;
                }
            }
        }
    }
}
