using HtmlAgilityPack;

using System;

namespace MineLib.Protocol.Generator
{
    // credits to http://mycodefunda.blogspot.com/2014/02/cnet-code-to-normalize-htmltable.html
    // Many thanks for the ready-to-work-after-copypasta code!
    internal static class HtmlTableNormalizer
    {
        public static string FormatAllTables(string htmlDocumentString)
        {
            var objHTMLdoc = new HtmlDocument();
            try
            {
                htmlDocumentString = htmlDocumentString.Replace("  ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                objHTMLdoc.LoadHtml(htmlDocumentString);

                if (objHTMLdoc.DocumentNode.SelectNodes("//table") is HtmlNodeCollection tableList)
                {
                    for (int tableIndex = 0; tableIndex < tableList.Count; tableIndex++)
                    {
                        try
                        {
                            var tableCopy = tableList[tableIndex];
                            tableCopy.InnerHtml = NormalizeTable(tableCopy);
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception) { }
            return objHTMLdoc.DocumentNode.OuterHtml; //OutPut
        }
        private static string NormalizeTable(HtmlNode table)
        {
            try
            {
                HtmlNode trnode = null, tdnode = null;
                int mRowSpan = 0, m = 0, mColSpan = 0;
                for (var trindex = 0; trindex < table.ChildNodes.Count; trindex++)
                {
                    trnode = table.ChildNodes[trindex];

                    for (var tdindex = 0; tdindex < trnode.ChildNodes.Count; tdindex++)
                    {
                        tdnode = trnode.ChildNodes[tdindex];
                        mRowSpan = 0; mColSpan = 0;
                        #region For rowspan
                        if (tdnode.Attributes["rowspan"] != null && tdnode.Attributes["rowspan"].Value != "1")
                        {
                            mRowSpan = Convert.ToInt32(tdnode.Attributes["rowspan"].Value);
                            InsertCellForRowSpan(ref table, trindex, tdindex, mRowSpan, tdnode);
                            tdnode.Attributes["rowspan"].Value = "1";
                            tdnode.Attributes.Add("OriginalRowspan", $"{mRowSpan}");
                        }
                        #endregion
                        #region For colspan
                        if (tdnode.Attributes["colspan"] != null && tdnode.Attributes["colspan"].Value != "1")
                        {
                            mColSpan = Convert.ToInt32(tdnode.Attributes["colspan"].Value);
                            for (m = 0; m < mColSpan - 1; m++)
                            {
                                var newNode = HtmlNode.CreateNode("<td></td>");
                                trnode.InsertAfter(newNode, tdnode);
                            }
                            tdnode.Attributes["colspan"].Value = "1";
                            tdnode.Attributes.Add("OriginalColspan", $"{mColSpan}");
                        }
                        #endregion
                    }
                }
            }
            catch (Exception) { }
            return table.InnerHtml;
        }
        private static void InsertCellForRowSpan(ref HtmlNode table, int rowIndex, int cellIndex, int rowspan, HtmlNode Maintdnode)
        {
            var trIndex = 0;
            try
            {
                foreach (var trnode in table.ChildNodes)
                {
                    if (trIndex > rowIndex && rowspan - 1 > 0)
                    {
                        var tdIndex = 0;

                        if (trnode.ChildNodes.Count > 0)
                        {
                            var mNormalizedCellCountIndex = GetNormalizedCellCountIndex(trnode);
                            foreach (var tdnode in trnode.ChildNodes)
                            {
                                if (tdnode.Attributes["rowspan"] != null && tdnode.Attributes["rowspan"].Value != "1")
                                {
                                    int mRecursiveRowSpan = Convert.ToInt32(tdnode.Attributes["rowspan"].Value);
                                    InsertCellForRowSpan(ref table, trIndex, tdIndex, mRecursiveRowSpan, tdnode);
                                    tdnode.Attributes["rowspan"].Value = "1";
                                    tdnode.Attributes.Add("Rowspanremoved", "true");
                                    tdnode.Attributes.Add("OriginalRowspan", $"{mRecursiveRowSpan}");
                                }
                                if (mNormalizedCellCountIndex < cellIndex || tdIndex == cellIndex)
                                {
                                    var newNode = HtmlNode.CreateNode("<td style=\"white-space:nowrap;padding-right:5px;padding-left:5px;\" row-span-cell=\"true\" ></td>");
                                    if (Maintdnode.Attributes["style"] != null)
                                    {
                                        newNode.Attributes["style"].Value = $"{Maintdnode.Attributes["style"].Value};{newNode.Attributes["style"].Value}";
                                        if (!(newNode.Attributes["style"].Value.Trim().Contains("border-top-style:solid") && newNode.Attributes["style"].Value.Trim().Contains("border-bottom-style:solid")))
                                            newNode.Attributes["style"].Value = newNode.Attributes["style"].Value.Replace("border-top", "");
                                    }

                                    if (Maintdnode.Attributes["colspan"] != null)
                                        newNode.Attributes.Add("colspan", Maintdnode.Attributes["colspan"].Value);
                                    if (mNormalizedCellCountIndex < cellIndex)
                                        trnode.InsertAfter(newNode, trnode.LastChild);
                                    else
                                        trnode.InsertBefore(newNode, tdnode);

                                    if (rowspan < 1) return;
                                    rowspan--;
                                    break;
                                }
                                if (tdnode.Attributes["colspan"] != null)
                                    tdIndex += Convert.ToInt16(tdnode.Attributes["colspan"].Value);
                                else
                                    tdIndex++;
                            }
                        }
                        else
                        {
                            var newNode = HtmlNode.CreateNode("<td style=\"white-space:nowrap;padding-right:5px;padding-left:5px;\" row-span-cell=\"true\" ></td>");
                            if (Maintdnode.Attributes["style"] != null)
                                newNode.Attributes["style"].Value = $"{Maintdnode.Attributes["style"].Value};{newNode.Attributes["style"].Value}";

                            if (Maintdnode.Attributes["colspan"] != null)
                                newNode.Attributes.Add("colspan", Maintdnode.Attributes["colspan"].Value);

                            trnode.AppendChild(newNode);

                            if (rowspan < 1) return;
                            rowspan--;
                        }
                    }
                    else if (rowspan - 1 == 0)
                    {
                        break;
                    }
                    trIndex++;
                }
            }
            catch (Exception) { }
        }
        private static int GetNormalizedCellCountIndex(HtmlNode trnode)
        {
            try
            {
                if (trnode != null)
                {
                    var mCellCountSum = 0;
                    for (var mCell = 0; mCell < trnode.ChildNodes.Count; mCell++)
                    {
                        if (trnode.ChildNodes[mCell].Attributes["colspan"] != null)
                            mCellCountSum += Convert.ToInt32(trnode.ChildNodes[mCell].Attributes["colspan"].Value);
                        else
                            mCellCountSum++;
                    }
                    return mCellCountSum - 1;
                }
            }
            catch (Exception) { }
            return trnode.ChildNodes.Count - 1;
        }
    }
}
