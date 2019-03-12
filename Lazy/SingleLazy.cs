namespace Lazy
{
    using System;

    /// <summary>
    /// Формирует объект со стратегией ленивого вычисления 
    /// для использования в одном потоке
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения вычисления</typeparam>
    public class SingleLazy<T> : ILazy<T>
    {
        /// <summary>
        /// Конструктор, задающий вычисления для формирования однопоточного ленивого объекта
        /// </summary>
        /// <param name="supplier">Поставщик вычисления</param>
        public SingleLazy(Func<T> supplier)
        {
            calculation = supplier;
        }
        
        private Func<T> calculation;  
        private bool isFirstCalculation = true;
        private T resultOfCalculation = default;

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
        /// Производит вычисление. Возвращается результат
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
            
            resultOfCalculation = calculation.Invoke();
            isFirstCalculation = false;
            calculation = null;

            return resultOfCalculation;
        }               
    }
}