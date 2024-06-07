#region usings

using System;
using System.Linq;
using System.Collections.Generic;
using OurOrdersRunner;
using System.IO;

#endregion

namespace OurOrders
{
    internal class Program
    {
        private static void Main()
        {
            SolutionChecker.Check(new Processor());
            Console.ReadKey();
        }
        /// <summary>
        ///     Реализация алгоритма задачи "Свои заявки"
        /// </summary>
        internal sealed class Processor : IOrderLogProcessor
        {
            private Dictionary<string, OrderLogEntry> activeOrders = new Dictionary<string, OrderLogEntry>();

            public void Update(OrderLogEntry orderLogEntry)
            {
                if (orderLogEntry.Deleted || orderLogEntry.Volume == 0)
                    activeOrders.Remove(orderLogEntry.Id);
                else
                    activeOrders[orderLogEntry.Id] = orderLogEntry;
            }

            public IEnumerable<Tuple<decimal, long>> GetLevels()
            {
                var groupedOrders = activeOrders.Values
                    .GroupBy(o => o.Price)
                    .Select(g => new Tuple<decimal, long>(g.Key, g.Sum(o => o.Volume)))
                    .OrderBy(t => t.Item1)
                    .ToList();
                return groupedOrders;
            }
        }
    }
}