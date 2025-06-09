## BinarySerializerLibrary
Библиотека **BinarySerializerLibrary** позволяет производить преобразование объектов в бинарное представление и обратно. Сериализация осуществляется путем последовательного преобразования содержимого объектов в массив байт, таким образом, ***последовательность декларирования свойств*** класса влияет на итоговый результат. Также, стоит отметить, что сериализуемые классы должны иметь **конструктор по умолчанию** (**без аргументов**).

### Пример использования в коде

```C#

/*
    За основу взята предметная область передачи CAN-сообщений
*/

// Класс объекта верхнего уровня
public class CANMessagesCollection
{
    [BinaryTypeObject(BinaryArgumentTypeEnum.List)]
    public List<CANMessage>? Messages { get; set; } = null;

    [BinaryTypeString(BinaryArgumentTypeEnum.Array)]
    public string[]? SomeDescriptions { get; set; } = Array.Empty<string>();
}

// Класс включаемого объекта
public class CANMessage
{
    [BinaryTypeUInt(29)]
    public UInt32 ID { get; set; }

    [BinaryTypeBool()]
    public bool IsExtended { get; set; }

    [BinaryTypeBool()]
    public bool IsRTR { get; set; }

    [BinaryTypeUInt(8, BinaryArgumentTypeEnum.Array)]
    public byte[]? Data { get; set; } = Array.Empty<byte>();
}

// Создание объекта
CANMessagesCollection originCollection = new();
// Логика заполнения коллекции
// ...........................
// ...........................
// ...........................
// Получение сериализованного массива байт
var binaryData = BinarySerializer.Serialize(obj1, (e) => Console.WriteLine(e.Message));
// Получение объекта из сериализованного массива байт
var unpackedCollection = BinarySerializer.Deserialize<CANMessagesCollection>(binaryData, (e) => Console.WriteLine(e.Message));

```

### Аттрибуты поментки свойств объектов

Сериализация объектов осуществляется только в том случае, если **свойства** этих объектов помечены следующими **атрибутами**.

|Атрибут|Описываемые типы|Размеры|
|--------|----------------|-------|
|BinaryTypeBoolAttribute|bool|1 бит|
|BinaryTypeIntAttribute|Int8, Int16, Int32, Int64|1-64 бит|
|BinaryTypeUIntAttribute|UInt8, UInt16, UInt32, UInt64|1-64 бит|
|BinaryTypeFloatAttribute|float|32 бит|
|BinaryTypeDoubleAttribute|double|64 бит|
|BinaryTypeCharAttribute|char|16 бит|
|BinaryTypeStringAttribute|string|16 бит на символ|
|BinaryTypeObjectAttribute|Атрибут-метка, что вложенный объект необходимо сериализовать|-|

> **Примечание.** Атрибуты **BinaryTypeIntAttribute** и **BinaryTypeUIntAttribute** позволяют управлять размером сериализуемого значения, что позволяет экономить место при получении двоичного представления в случаях, если не весь диапазон значений оригинального типа задействуется.

При добавлении атрибутов (в конструкторах) можно указать выравнивание сериализуемых свойств с помощью перечисления **AlignmentTypeEnum**.

|Аргумент атрибута|Описание|
|------------|--------|
|AlignmentTypeEnum.NoAlignment|Значение свойства обрабатывается без выравнивания|
|AlignmentTypeEnum.ByteAlignment|Значение свойства обрабатывается с выравниванием по байтам|


> **Примечание.** По умолчанию все свойства обрабатываются без выравнивания.

Для корректной обработки Nullable-типов необходимо сделать соответствующие пометки в конструкторах атрибутов свойств с помощью перечисления **NullableTypeEnum**.

|Аргумент атрибута|Описание|
|------------|--------|
|NullableTypeEnum.NotNullable|Значение свойства не может быть null|
|NullableTypeEnum.Nullable|Значение свойства может быть null|

> **Примечание.** По умолчанию свойства **BinaryTypeObjectAttribute**, **BinaryTypeStringAttribute**, **BinaryArgumentTypeEnum.Array**, **BinaryArgumentTypeEnum.List** всегда обрабатываются как **Nullable**, а все остальные - как **NotNullable**.

При сериализации можно использовать как свойства, представленные **атомарным базовым типом**, так и **свойства-коллекции** (**Array** или **List**), хранящие базовые типы. Для корректной работы сериализатора необходимо указывать тип коллекции при объявлении атрибута поля.

|Тип атрибута|Описание|
|------------|--------|
|BinaryArgumentTypeEnum.Single|Свойство представлено атомарным типов|
|BinaryArgumentTypeEnum.Array|Свойство представлено типом-массивом|
|BinaryArgumentTypeEnum.List|Свойство представлено типом-списком|

> **Примечание.** По умолчанию все атрибуты предполагают атомарный тип свойства.
> **Примечание.** Максимальное количество элементов строк и коллекций ограничено значением **1073741823**.

### Примечания
Используется утилита [CommitContentCreater](https://github.com/DemiEljer/CommitContentCreater) для формирования текстов описания коммитов.
