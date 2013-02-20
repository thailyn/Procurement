﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.ForumExportRunners
{
    internal class MapRunner : ForumExportRunnerBase<Map>
    {
        public override string GetSpoiler(IEnumerable<Map> items)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[spoiler=\"Maps\"]");

            foreach (var item in items.OrderBy(m => m.IconURL))
                builder.Append(string.Format("[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]", item.inventoryId, ApplicationState.CurrentLeague, item.X, item.Y));

            builder.AppendLine("[/spoiler]");

            return builder.ToString();
        }
    }
}