namespace Lazy
{
    using System;
    using System.Threading;

    /// <summary>
    /// Формирует объект со стратегией ленивого вычисления 
    /// для многопоточного использования
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения вычисления</typeparam>
    public class MultipleLazy<T> : ILazy<T>
    {
        /// <summary>
        /// Конструктор, задающий вычисления для формирования многопоточного ленивого объекта
        /// </summary>
        /// <param name="supplier">Поставщик вычисления</param>
        public MultipleLazy(Func<T> supplier)
        {
            calculation = supplier;
        }

        private Func<T> calculation;
        private bool isFirstCalculation = true;
        private T resultOfCalculation = default;
        private object locker = new object();

        /// <summary>
        /// Проверка: проводилось ли уже вычисление
        /// </summary>
        public bool IsFirstCalculation() => isFirstCalculation;

        /// <summary>
        /// Возвращает результат вычисления
        /// </summary>
        /// <returns></returns>
        public T ResultOfCalculation() => resultOfCalculation;

        /// <summary>
        /// Производит вычисление
        /// </summary>
        /// <remarks>
        /// Если метод запускается впервые - проводится вычисление. 
        /// Иначе возвращается вычисленный ранее результат.
        /// </remarks>
        /// <returns>Результат вычисление типа T</returns>
        public T Get()
        {
            if (!isFirstCalculation)
            {
                return resultOfCalculation;
            }

            lock (locker)
            {
                if (!isFirstCalculation)
                {
                    return resultOfCalculation;
                }                

                resultOfCalculation = calculation.Invoke();                
                Volatile.Write(ref isFirstCalculation, !isFirstCalculation);
                calculation = null;               
                
                return resultOfCalculation;
            }            
        }        
    }
}