using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser.Filter;

using iText.Kernel.Pdf.Canvas.Parser;

namespace PdfPrime.Services
{
    public class PdfService
    {
        private readonly ReaderProperties _readerProperties = new ReaderProperties();
        private readonly DocumentProperties _documentProperties = new DocumentProperties();

        private readonly List<(PdfPage, string[])> lstPdfPage = new List<(PdfPage, string[])>();
        //public PdfService(ReaderProperties readerProperties, DocumentProperties documentProperties)
        //{
        //    _readerProperties = readerProperties;
        //    _documentProperties = documentProperties;
        //}

        public PdfService()
        {

        }

        public IEnumerable<(PdfPage, string[])> Read(string fileName, string? password = null)
        {
            if (password != null)
            {
                _readerProperties.SetPassword(Encoding.UTF8.GetBytes(password));
            }

            //var document = new PdfDocument(new PdfReader(fileName, _readerProperties), _documentProperties);
            var document = new PdfDocument(new PdfReader(fileName));

            for (int i = 1; i <= document.GetNumberOfPages(); i++)
            {
                var page = document.GetPage(i);
                ITextExtractionStrategy textExtractionStrategy = new SimpleTextExtractionStrategy();
                var sectionHeadingFontFilter = new TextRegionEventFilter(page.GetArtBox());
                var chapterHeadingFontFilter = new TextRegionEventFilter(page.GetArtBox());

                var listener = new FilteredEventListener();

                // Create text extraction renderers
                var sectionHeadingExtractionStrategy = listener.AttachEventListener(new LocationTextExtractionStrategy(), sectionHeadingFontFilter);
                var chapterHeadingExtractionStrategy = listener.AttachEventListener(new LocationTextExtractionStrategy(), chapterHeadingFontFilter);

                new PdfCanvasProcessor(listener).ProcessPageContent(page);

                var pageContent = PdfTextExtractor.GetTextFromPage(page, textExtractionStrategy)?.Split('\n');

                //// Get the content that is menu size
                //var sectionHeadingContent = sectionHeadingExtractionStrategy.GetResultantText()?.Split('\n');

                //// Get the Chapter Headings, if any
                //var chapterHeadingContent = chapterHeadingExtractionStrategy.GetResultantText()?.Split('\n');

                //// Find pdf page number
                //var lastLine = pageContent != null ? pageContent[pageContent.Length - 1] : string.Empty;

                //if (!int.TryParse(lastLine.Substring(0, 3), out var pdfPageNum))
                //{
                //    int.TryParse(lastLine.Substring(lastLine.Length - 3, 3), out pdfPageNum);
                //}
                lstPdfPage.Add((page, pageContent)!);

                //lstPdfPage.add
            }

            return lstPdfPage;

        }

    }
}
