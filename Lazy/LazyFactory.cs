namespace Lazy
{
    using System;

    public interface ILazy<T>
    {
        /// <summary>
        /// Проводит ленивое вычисление. Возвращает результат
        /// </summary>
        /// <remarks>
        /// Если вычисление проводилось ранее - возвращает только результат. 
        /// Иначе - проводит вычисление и возвращает результат.
        /// </remarks>
        /// <returns>Результат вычисления</returns>
        T Get();
    }

    /// <summary>
    /// Фабрика объектов со стратегией ленивого вычисления
    /// </summary>
    public class LazyFactory   
    {
        /// <summary>
        /// Однопоточный ленивый объект
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения вычисления</typeparam>
        /// <param name="supplier">Поставщик вычисления</param>
        /// <returns>Однопоточный ленивый объект</returns>
        public static SingleLazy<T> SingleThreadLazy<T>(Func<T> supplier)
        {                                            
            var lazy = new SingleLazy<T>(supplier);                       
            return lazy;
        }

        /// <summary>
        /// Многопоточный ленивый объект
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения вычисления</typeparam>
        /// <param name="supplier">Поставщик вычисления</param>
        /// <returns>Многопоточный ленивый объект</returns>
        public static MultipleLazy<T> MultipleThreadLazy<T>(Func<T> supplier)
        {
            var lazy = new MultipleLazy<T>(supplier);
            return lazy;
        }                   
    }
}