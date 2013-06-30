﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class RecipeManager
    {
        private List<Recipe> known;
        public RecipeManager()
        {
            known = new List<Recipe>() 
            { 
                new OneChoasRecipe(), 
                new Chromatic(), 
                new GCPRecipe(), 
                new SameNameRecipe("Chance Orb - 2 Of The Same Name", 2),
                new SameNameRecipe("Alchemy Orb - 3 Of The Same Name", 3)
            };
        }

        public Dictionary<string, List<RecipeResult>> Run(IEnumerable<Item> items)
        {
            return known.SelectMany(recipe => recipe.Matches(items))
                        .GroupBy(r => r.Instance.Name)
                        .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
