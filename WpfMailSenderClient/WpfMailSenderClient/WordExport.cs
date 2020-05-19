using System;
using System.IO;
using Xceed.Words.NET;
using Xceed.Document.NET;

namespace WpfMailSenderClient
{
	public class WordExport
	{
		private DocX _document;
		private Font _fontFamily = new Font("Times New Roman");
		private double _fontSizeText = 12;
		private double _fontSizeTitle = 14;
		private double _spacing = 1.5;
		private string _filename;

		public WordExport(string filename)
		{
			_filename = filename;
			try
			{
				using (DocX document = DocX.Create(_filename))
				{
					document.Save();
				}
            }
			catch (Exception e)
			{
				throw new Exception(@"Произошла ошибка при создании документа. Подробнее: " + e.Message);
			}
		}

		public void Export(string caption, string[] collection)
		{
			using (DocX document = DocX.Load(_filename))
			{
				_document = document;
				_document.MarginLeft = 85; // 3 cm
				_document.MarginRight = 28.3f;// 1 cm
				_document.MarginTop = 28.3f;
				_document.MarginBottom = 28.3f;
				AddTitle(caption);
				Paragraph par = CreateParagraph();
				foreach (string str in collection)
					AddTextLineToParagraph(par, str); 
				PageBreak();
				document.Save();
			}
		}

		private void PageBreak()
		{
			if (_document == null) 
				return;
			Paragraph pageBreak = _document.InsertParagraph();
			pageBreak.InsertPageBreakAfterSelf();
		}

		private void AddTitle(string vale)
		{
			if (_document == null) 
				return;
			Paragraph title = _document.InsertParagraph();
			title.Append(vale).Font(_fontFamily).FontSize(_fontSizeTitle).Bold().Alignment = Alignment.center;
		}

		private void AddTextLineToParagraph(Paragraph paragraph, string vale)
		{
			if (_document == null) 
				return;
			paragraph.AppendLine(vale).Font(_fontFamily).FontSize(_fontSizeText);
		}

		private void AddTextAppendToParagraph(Paragraph paragraph, string vale)
		{
			if (_document == null) 
				return;
			paragraph.Append(vale).Font(_fontFamily).FontSize(_fontSizeText);
		}

		private Paragraph CreateParagraph()
		{
			Paragraph paragraph = _document.InsertParagraph();
			paragraph.Spacing(_spacing);
			return paragraph;
		}
	}
}