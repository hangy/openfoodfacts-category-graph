namespace OffCategoryGraph
{
    using OffLangParser;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ParentsComparer : IComparer<IReadOnlyList<TranslationSet>>
    {
        private AlphabeticalTranslationSetComparer alphabeticalTranslationSetComparer;

        public ParentsComparer(AlphabeticalTranslationSetComparer alphabeticalTranslationSetComparer)
        {
            if (alphabeticalTranslationSetComparer == null)
            {
                throw new ArgumentNullException(nameof(alphabeticalTranslationSetComparer));
            }

            this.alphabeticalTranslationSetComparer = alphabeticalTranslationSetComparer;
        }

        public int Compare(IReadOnlyList<TranslationSet> x, IReadOnlyList<TranslationSet> y)
        {
            if (x == null && y == null || x.Count == 0 && y.Count == 0)
            {
                return 0;
            }

            if (x != null && y == null || x.Count != 0 && y.Count == 0)
            {
                return 1;
            }

            if (x == null && y != null || x.Count == 0 && y.Count != 0)
            {
                return -1;
            }

            if (x.Equals(y))
            {
                return 0;
            }

            return this.alphabeticalTranslationSetComparer.Compare(x.First(), y.First());
        }
    }
}