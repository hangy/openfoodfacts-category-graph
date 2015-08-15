namespace OffCategoryGraph
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using OffLangParser;

    internal class AlphabeticalTranslationSetComparer : IComparer<TranslationSet>
    {
        private readonly LanguageByImportanceComparer languageComparer;

        private readonly Func<TranslationSet, IComparer<CultureData>, Translation> evaluateMostImportantTranslation;

        private readonly Func<Translation, string> getLabel;

        private readonly StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

        public AlphabeticalTranslationSetComparer(LanguageByImportanceComparer languageComparer, Func<TranslationSet, IComparer<CultureData>, Translation> evaluateMostImportantTranslation, Func<Translation, string> getLabel)
        {
            if (languageComparer == null)
            {
                throw new ArgumentNullException(nameof(languageComparer));
            }

            if (evaluateMostImportantTranslation == null)
            {
                throw new ArgumentNullException(nameof(evaluateMostImportantTranslation));
            }

            if (getLabel == null)
            {
                throw new ArgumentNullException(nameof(getLabel));
            }

            this.languageComparer = languageComparer;
            this.evaluateMostImportantTranslation = evaluateMostImportantTranslation;
            this.getLabel = getLabel;
        }

        public int Compare(TranslationSet x, TranslationSet y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x != null && y == null)
            {
                return 1;
            }

            if (x == null && y != null)
            {
                return -1;
            }

            if (x.Equals(y))
            {
                return 0;
            }

            var xTranslation = this.evaluateMostImportantTranslation(x, this.languageComparer);
            var yTranslation = this.evaluateMostImportantTranslation(y, this.languageComparer);

            if (xTranslation == null && yTranslation == null)
            {
                return 0;
            }

            if (xTranslation != null && yTranslation == null)
            {
                return 1;
            }

            if (xTranslation == null && yTranslation != null)
            {
                return -1;
            }

            if (xTranslation.Equals(yTranslation))
            {
                return 0;
            }

            var firstX = xTranslation.Words.First();
            var firstY = yTranslation.Words.First();
            return this.stringComparer.Compare(firstX, firstY);
        }
    }
}