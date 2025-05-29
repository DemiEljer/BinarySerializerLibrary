using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializerLibrary.Base
{
    public class LinkedListWrapper<TElement>
    {
        /// <summary>
        /// Базовая коллекция
        /// </summary>
        public LinkedList<TElement?> List { get; } = new();
        /// <summary>
        /// Количество элементов
        /// </summary>
        public int Count => List.Count;
        /// <summary>
        /// Индекс текущего элемента
        /// </summary>
        public int ElementIndex { get; private set; }
        /// <summary>
        /// Преобразовать коллекцию в массив
        /// </summary>
        /// <returns></returns>
        public TElement?[] ToArray()
        {
            if (List.Count == 0)
            {
                return Array.Empty<TElement>();
            }
            else
            {
                return List.ToArray();
            }
        }
        /// <summary>
        /// Текущий элемент в коллекции
        /// </summary>
        private LinkedListNode<TElement?>? _CurrentNode { get; set; } = null;
        /// <summary>
        /// Очистить коллекцию
        /// </summary>
        public void Clear()
        {
            List.Clear();
            _CurrentNode = null;
            ElementIndex = 0;
        }
        /// <summary>
        /// Создать и добавить элементы в конец списка
        /// </summary>
        /// <param name="count"></param>
        public void CreateAndAppendToEnd(int count)
        {
            if (count <= 0)
            {
                return;
            }
            // В случае, если первый элемент не был проинициализирован
            if (_CurrentNode == null)
            {
                _CurrentNode = List.AddLast(default(TElement));

                count--;
            }
            // Добавление необходимого числа элементов
            foreach (var i in Enumerable.Range(0, count))
            {
                var node = List.AddLast(default(TElement));
            }
        }
        /// <summary>
        /// Взять на обработку некоторое число элементов начиная с текущего
        /// </summary>
        /// <param name="count"></param>
        /// <param name="handler">Обработчик, где первый аргумент соответсвует индекусу элемента в подмассиве</param>
        public void Take(int count, Func<int, TElement?, TElement?>? handler)
        {
            if (handler is null)
            {
                return;
            }

            var nextElement = _CurrentNode;

            for (int i = 0; i < count && nextElement != null; i++)
            {
                nextElement.Value = handler.Invoke(i, nextElement.Value);

                nextElement = nextElement.Next;
            }
        }
        /// <summary>
        /// Взять на обработку некоторое число элементов начиная с текущего
        /// </summary>
        /// <param name="count"></param>
        /// <param name="handler">Обработчик, где первый аргумент соответсвует индекусу элемента в подмассиве</param>
        public void Take(int count, Func<IEnumerable<TElement?>, IEnumerable<TElement?>>? handler)
        {
            if (handler is null || count <= 0)
            {
                return;
            }

            IEnumerable<TElement?> _GetElementsEnumeration()
            {
                var nextElement = _CurrentNode;

                for (int i = 0; i < count && nextElement != null; i++)
                {
                    yield return nextElement.Value;

                    nextElement = nextElement.Next;
                }
            };

            void _SetElementsFromEnumeration(IEnumerable<TElement?> newValues)
            {
                var nextElement = _CurrentNode;

                foreach (var value in newValues)
                {
                    if (nextElement is null)
                    {
                        break;
                    }

                    nextElement.Value = value;

                    nextElement = nextElement.Next;
                }
            };

            try
            {
                _SetElementsFromEnumeration(handler(_GetElementsEnumeration()));
            }
            catch
            {

            }
        }
        /// <summary>
        /// Сдвиг по вектору
        /// </summary>
        /// <param name="count"></param>
        public void Shift(int count)
        {
            foreach (var i in Enumerable.Range(0, count))
            {
                if (_CurrentNode is null)
                {
                    break;
                }

                _CurrentNode = _CurrentNode.Next;
                ElementIndex++;
            }
        }
        /// <summary>
        /// Сбросить текущий элемент к начальному в коллекции
        /// </summary>
        public void ResetCurrentElement()
        {
            _CurrentNode = List.First;
            ElementIndex = 0;
        }
    }
}
