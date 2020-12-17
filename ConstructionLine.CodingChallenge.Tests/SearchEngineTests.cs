using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        private readonly List<Shirt> _shirts = new List<Shirt>
        {
            new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
            new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
            new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            new Shirt(Guid.NewGuid(), "Black - Large", Size.Large, Color.Black),
            new Shirt(Guid.NewGuid(), "Yellow - Small", Size.Small, Color.Yellow),
            new Shirt(Guid.NewGuid(), "Yellow - Medium", Size.Medium, Color.Yellow)
        };

        [Test]
        public void GivenISearchForRedAndSmallShirts_ThenOnlyRedAndSmallShirtsAreReturned()
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
                Sizes = new List<Size> {Size.Small}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenISearchForBlackWhiteYellowMediumAndSmallShirts_ThenOnlyBlackWhiteYellowMediumAndSmallShirtsAreReturned()
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Black, Color.White, Color.Yellow },
                Sizes = new List<Size> { Size.Small, Size.Medium }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenISearchForBlackWhiteYellowMediumAndSmallShirtsInEmptyShirtsCollection_ThenNoShirtsAreReturned()
        {
            var shirts = new List<Shirt>();
            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Black, Color.White, Color.Yellow },
                Sizes = new List<Size> { Size.Small, Size.Medium }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenISearchForBlueAndAnySizeShirts_ThenOnlyBlueShirtsAreReturned()
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Blue },
                Sizes = new List<Size>()
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenISearchForLargeAndAnyColorShirts_ThenOnlyLargeShirtsAreReturned()
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color>(),
                Sizes = new List<Size> {Size.Large}
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenISearchForAnyShirts_ThenAllShirtsAreReturned()
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color>(),
                Sizes = new List<Size>()
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void GivenNullSearchOptionIsPassed_ThenArgumentNullExceptionIsThrown()
        {
            var searchEngine = new SearchEngine(_shirts);

            SearchOptions searchOptions = null;

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(searchOptions));
        }

        [Test]
        public void GivenNullSizesArePassed_ThenArgumentNullExceptionIsThrown()
        {
            var searchEngine = new SearchEngine(_shirts);

            SearchOptions searchOptions = new SearchOptions()
            {
                Sizes = null,
                Colors = new List<Color>()
            };

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(searchOptions));
        }

        [Test]
        public void GivenNullColorsArePassed_ThenArgumentNullExceptionIsThrown()
        {
            var searchEngine = new SearchEngine(_shirts);

            SearchOptions searchOptions = new SearchOptions()
            {
                Sizes = new List<Size>(),
                Colors = null
            };

            Assert.Throws<ArgumentNullException>(() => searchEngine.Search(searchOptions));
        }
    }
}
