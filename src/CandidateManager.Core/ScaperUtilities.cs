using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace CandidateManager.Core
{
    public class ScraperUtilities
    {

        public static List<string> GetTextElements(string html)
        {
            List<string> readableElements = new List<string>();
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                readableElements.InsertRange(0,GetTextOnly(doc.DocumentNode));
            }
            catch
            {
                //no-op - manage by returning 
            }
            return readableElements;
        }

        public static string GetContentValue(List<string> textElements, string propertyName)
        {
            string val = string.Empty;
            if (textElements.Contains<string>(propertyName))
            {
                int propertyIndex = textElements.FindIndex(t => t.Equals(propertyName));
                val = textElements[propertyIndex + 1].ToString();
            }
            return val;
        }


        #region
        //section 2 for getting only text 
        private static List<string> GetTextOnly(HtmlNode root)
        {
            List<string> textElements = new List<string>();
            foreach (var node in root.DescendantsAndSelf())
            {
                if (!node.HasChildNodes)
                {
                    string text = node.InnerText;
                    string sanitized = text.Trim().Replace(Core.Constants.HTML_SPACE, string.Empty).Replace(Constants.COLON, string.Empty);
                    if (!string.IsNullOrWhiteSpace(sanitized))
                    {
                        Debug.WriteLine(sanitized.Trim());
                        textElements.Add(sanitized);
                    }
                }
            }
            return textElements;
        }
        #endregion

    }

}
