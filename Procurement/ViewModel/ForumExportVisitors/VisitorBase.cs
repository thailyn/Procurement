﻿using System.Collections.Generic;
using System.Text;
using POEApi.Model;
using Procurement.ViewModel.Filters;
using System.Linq;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal abstract class VisitorBase : IVisitor
    {
        private const string LINKITEM = "[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]";

        public abstract string Visit(IEnumerable<Item> items, string current);
        
        protected string runFilter<T>(IEnumerable<Item> items) where T : IFilter, new()
        {
            return runFilter(new T(), items);
        }

        protected string runFilter(IFilter filter, IEnumerable<Item> items)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in items.Where(i => filter.Applicable(i)))
                builder.Append(getLinkItem(item));

            return builder.ToString();
        }

        protected string getLinkItem<T>(T item) where T : Item
        {
            return string.Format("[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]", item.inventoryId, ApplicationState.CurrentLeague, item.X, item.Y);
        }
    }
}
