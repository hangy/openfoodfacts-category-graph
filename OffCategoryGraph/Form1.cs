namespace OffCategoryGraph
{
    using Microsoft.Msagl.Drawing;
    using Microsoft.Msagl.Layout.MDS;
    using OffLangParser;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        private readonly Lazy<LinkedLangFileParser> parser = new Lazy<LinkedLangFileParser>(ParserFactory.GetParser, LazyThreadSafetyMode.PublicationOnly);

        public Form1()
        {
            this.InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.ViewGraph(this.RenderGraph(this.ReadFile(this.openFileDialog1.FileName)));
            }
            else
            {
                this.Close();
            }
        }

        private LangFile ReadFile(string fileName)
        {
            return this.parser.Value.Parse(fileName);
        }

        private Graph RenderGraph(LangFile langFile)
        {
            var graph = new Graph("OpenFoodFacts Category Taxonomy");

            var languageComparer = new LanguageByImportanceComparer(langFile.TranslationSets);
            var parentsComparer = new ParentsComparer(new AlphabeticalTranslationSetComparer(languageComparer, EvaluateMostImportantTranslation, GetLabel));

            foreach (var translationSet in langFile.TranslationSets.OrderBy(ts => ts.Parents, parentsComparer))
            {
                if (!translationSet.Translations.Any())
                {
                    continue;
                }

                var translation = EvaluateMostImportantTranslation(translationSet, languageComparer);
                var translationLabel = GetLabel(translation);

                if (translationSet.Parents.Any())
                {
                    foreach (var parent in translationSet.Parents)
                    {
                        var parentTranslation = EvaluateMostImportantTranslation(parent, languageComparer);
                        var parentTranslationLabel = GetLabel(parentTranslation);
                        graph.AddEdge(parentTranslationLabel, translationLabel);
                    }
                }
                else
                {
                    graph.AddNode(translationLabel);
                }
            }

            return graph;
        }

        private void ViewGraph(Graph graph)
        {
            this.gViewer1.Graph = graph;
        }

        private static Translation EvaluateMostImportantTranslation(TranslationSet ts, IComparer<CultureData> languageComparer)
        {
            return EvaluateMostImportantTranslation(ts.Translations, languageComparer);
        }

        private static Translation EvaluateMostImportantTranslation(IEnumerable<Translation> translations, IComparer<CultureData> languageComparer)
        {
            return translations.Where(t => t.Words.Count > 0).OrderByDescending(t => t.Language, languageComparer).FirstOrDefault();
        }

        private static string GetLabel(Translation translation)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", translation.Language.Name, translation.Words.First());
        }
    }
}
