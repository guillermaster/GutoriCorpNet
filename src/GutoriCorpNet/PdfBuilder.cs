using GutoriCorp.Models.BusinessViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using RazorEngine;
using System;
using System.IO;
using System.Text;

namespace GutoriCorp.Controllers
{
    public class PdfBuilder
    {
        private readonly ContractViewModel _contract;
        private readonly string _file;
        public PdfBuilder(ContractViewModel contract, string file)
        {
            _contract = contract;
            _file = file;
        }
        public FileContentResult GetPdf()
        {
            var html = GetHtml();
            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        try
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                            {
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance()
                                    .ParseXHtml(writer, doc, msHtml, System.Text.Encoding.UTF8);
                            }
                        }
                        finally
                        {
                            doc.Close();
                        }
                    }
                }
                bytes = ms.ToArray();
            }
            return new FileContentResult(bytes, "application/pdf");
        }
        private string GetHtml()
        {
            var html = File.ReadAllText(_file);
            return Razor.Parse(html, _contract);
        }
    }
}