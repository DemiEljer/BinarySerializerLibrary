using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Base
{
    public class ObjectTypeCodePair
    {
        public Type ObjectType { get; }

        public int ObjectTypeCode { get; private set; }

        public ObjectTypeCodePair(Type objectType, int objectTypeCode)
        {
            ObjectType = objectType;
            ObjectTypeCode = objectTypeCode;
        }

        public void SetTypeCode(int objectTypeCode)
        {
            ObjectTypeCode = objectTypeCode;
        }
    }
}
