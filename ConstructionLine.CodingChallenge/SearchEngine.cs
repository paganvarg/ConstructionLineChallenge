using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly Dictionary<byte, List<Shirt>> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = GroupResultsByColorAndSize(shirts);
        }

        public SearchResults Search(SearchOptions options)
        {
            if (options?.Colors == null || options.Sizes == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var colors = options.Colors.Any() ? options.Colors : Color.All;
            var sizes = options.Sizes.Any() ? options.Sizes : Size.All;
            var allSearchedCombinationsOfColorAndSize =
                new HashSet<byte>(
                from c in colors
                from s in sizes
                select (byte)(c.GetColorAsByte() + s.GetSizeAsByte()));

            var searchResults = new SearchResults
            {
                Shirts = new List<Shirt>(),
                ColorCounts = Color.All.Select(c => new ColorCount() { Color = c, Count = 0 }).ToList(),
                SizeCounts = Size.All.Select(s => new SizeCount() { Size = s, Count = 0 }).ToList()
            };

            return _shirts.Where(s => allSearchedCombinationsOfColorAndSize.Contains(s.Key))
                .Aggregate(searchResults, (aggregateResult, item) =>
                {
                    var color = item.Value.First().Color;
                    var size = item.Value.First().Size;
                    var colorCount = aggregateResult.ColorCounts.First(c => c.Color == color);
                    var sizeCount = aggregateResult.SizeCounts.First(s => s.Size == size);

                    aggregateResult.Shirts.AddRange(item.Value);
                    colorCount.Count += item.Value.Count;
                    sizeCount.Count += item.Value.Count;

                    return aggregateResult;
                });
        }

        private static Dictionary<byte, List<Shirt>> GroupResultsByColorAndSize(List<Shirt> shirts)
        {
            return shirts.AsParallel().WithMergeOptions(ParallelMergeOptions.FullyBuffered).WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(s => new { ColorSizeValue = s.GetShirtDetailsAsByte(), Shirt = s })
                .GroupBy(s => s.ColorSizeValue).ToDictionary(s => s.Key, s => s.Select(g => g.Shirt).ToList());
        }
    }
}