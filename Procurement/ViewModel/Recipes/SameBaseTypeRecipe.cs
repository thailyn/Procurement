using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    class SameBaseTypeRecipe : Recipe
    {
        public SameBaseTypeRecipe()
            : base(60)
        { }

        public override string Name
        {
            get { return "1 Orb of Augmentation - Same base type with normal, magic, rare"; }
        }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<POEApi.Model.Item> items)
        {
            List<Gear> allGear = items.OfType<Gear>().ToList();
            Dictionary<string, List<Gear>> baseTypeBuckets = allGear.Where(g => !string.IsNullOrWhiteSpace(g.BaseType))
                                                                    .GroupBy(g => g.BaseType)
                                                                    .ToDictionary(g => g.Key.ToString(), g => g.ToList());

            //while (result.IsMatch)
            while (baseTypeBuckets.Keys.Count > 0)
            {
                RecipeResult result = getNextResult(baseTypeBuckets);
                if (result.IsMatch)
                    yield return result;
            }
        }

        private RecipeResult getNextResult(Dictionary<string, List<Gear>> buckets)
        {
            RecipeResult result = new RecipeResult();
            result.MatchedItems = new List<Item>();
            result.Missing = new List<string>();
            result.PercentMatch = 0;
            result.Instance = this;
            //result.Missing = "Missing item(s) with quality: ";
            //List<Enum> missingRarities = new List<Enum>();

            if (buckets.Keys.Count == 0)
                return result;

            var firstBucketPair = buckets.FirstOrDefault();
            List<Gear> bucket = buckets.FirstOrDefault().Value;
            if (bucket == null)
                return result; // no match

            if (bucket.Count == 0)
            {
                buckets.Remove(firstBucketPair.Key);
            }


            Dictionary<Rarity, Gear> set = new Dictionary<Rarity, Gear>();
            set.Add(Rarity.Normal, bucket.FirstOrDefault(g => g.Rarity == Rarity.Normal));
            set.Add(Rarity.Magic, bucket.FirstOrDefault(g => g.Rarity == Rarity.Magic));
            set.Add(Rarity.Rare, bucket.FirstOrDefault(g => g.Rarity == Rarity.Rare));

            decimal numKeys = set.Keys.Count;
            foreach(var pair in set)
            {
                Rarity rarity = pair.Key;
                Gear gear = pair.Value;
                if (gear != null)
                {
                    result.PercentMatch += (decimal)100.0 / numKeys;
                    result.MatchedItems.Add(gear);
                    bucket.Remove(gear);
                }
                else
                {
                    result.Missing.Add(string.Format("Item with quality: {0}", rarity.ToString()));
                }
            }

            result.IsMatch = result.PercentMatch > base.ReturnMatchesGreaterThan;

            if (result.Missing.Count > 0 || bucket.Count == 0)
                buckets.Remove(firstBucketPair.Key);

            return result;
        }
    }
}
