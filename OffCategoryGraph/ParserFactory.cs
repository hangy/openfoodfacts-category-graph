namespace OffCategoryGraph
{
    using OffLangParser;
    using System.Collections.Generic;

    internal static class ParserFactory
    {
        public static LinkedLangFileParser GetParser()
        {
            return new LinkedLangFileParser(
             new LangFileParser(
                 new StopWordsParser(),
                 new SynonymsParser(),
                 new TranslationSetParser(
                     new TranslationParser(),
                     new LinkedDataParser(new List<PrefixOnlyParser<LinkedData>>
                     {
                            new WikidataParser(),
                            new WikidataCategoryParser(),
                            new WikipediaCategoryParser(),
                            new PnnsGroupParser(1),
                            new PnnsGroupParser(2)
                     }))));
        }
    }
}
