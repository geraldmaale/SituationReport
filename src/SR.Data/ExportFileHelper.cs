using System.Collections;
using System.Reflection;
using GreatIdeas.Extensions;
using Syncfusion.Drawing;
using Syncfusion.XlsIO;

namespace SR.Data;

/// <summary>
/// Export Helper for formatting and exporting data from collection to spreadsheet using Syncfusion XlsIO.
/// </summary>
public class ExportFileHelper
{
    /// <summary>
    /// Generate and format data from collection to spreadsheet using Syncfusion XlsIO.
    /// </summary>
    /// <param name="data">The collection of type <typeparamref name="T"/> of objects</param>
    /// <param name="headerName"></param>
    /// <param name="exportType"></param>
    /// <param name="showTitle">Whether to show a title and merge</param>
    /// <typeparam name="T">The model of the collection</typeparam>
    /// <returns></returns>
    public MemoryStream GenerateExcel<T>(List<T> data, string headerName, string exportType, bool showTitle)
        where T : class
    {
        var model = typeof(T);

        // Get properties from T
        var props = model.GetProperties();
        List<string> propNames = new();

        // Prop characters
        char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        // Syncfusion
        // var contentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        using ExcelEngine excelEngine = new();
        var application = excelEngine.Excel;

        application.DefaultVersion = ExcelVersion.Excel2016;

        //Create a workbook
        var workbook = application.Workbooks.Create(1);
        //Create named worksheet
        var worksheet = workbook.Worksheets.Create(headerName);
        // Remove worksheet (first index)
        workbook.Worksheets[0].Remove();
        //Activate worksheet
        worksheet.Activate();

        //Highlighting sheet tab
        worksheet.TabColor = ExcelKnownColors.Dark_green;

        // ALIGN HEADERS
        var headingStyle = workbook.Styles.Add("HeadingStyle");
        headingStyle.Font.Bold = true;
        headingStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;

        // Get PropertyInfo Names
        foreach (var property in props)
        {
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
                && property.PropertyType.IsGenericType
                && property.PropertyType.GetGenericArguments().Length == 1)
            {
                IList<PropertyInfo> innerProperties = property.PropertyType.GetGenericArguments()[0].GetProperties();
                //should contain properties of elements in lists
                foreach (var innerProperty in innerProperties)
                {
                    // propNames.Add(innerProperty.Name.InsertSpaceBeforeUpperCase());
                    if (typeof(IEnumerable).IsAssignableFrom(innerProperty.PropertyType)
                        && innerProperty.PropertyType.IsGenericType
                        && innerProperty.PropertyType.GetGenericArguments().Length == 1)
                    {
                        //should contain properties of elements in lists
                        IList<PropertyInfo> innerInnerProperties = innerProperty.PropertyType.GetGenericArguments()[0].GetProperties();
                        foreach (var innerInnerProperty in innerInnerProperties)
                        {
                            propNames.Add(innerInnerProperty.Name.InsertSpaceBeforeUpperCase());
                        }
                    }
                    else
                    {
                        //should contain properties of elements not in a list
                        propNames.Add(innerProperty.Name.InsertSpaceBeforeUpperCase());
                    }
                }
            }
            else
            {
                //should contain properties of elements not in a list
                propNames.Add(property.Name.InsertSpaceBeforeUpperCase());
            }

        }

        if (showTitle)
        {
            for (var i = 0; i < propNames.Count; i++)
            {
                worksheet.Range[$"{characters[i]}3"].Text = propNames[i];
            }

            //Merging Cells
            worksheet.Range[$"A1:{characters[propNames.Count - 1]}1"].Merge();
            worksheet.Range[$"A2:{characters[propNames.Count - 1]}2"].Merge();
            worksheet.Range["A1"].Text = $"{exportType}".ToUpper();
            worksheet.Range["A2"].Text = $"{headerName}".ToUpper();
            
            worksheet.Range["A1:A2"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            worksheet.Range["A1:A2"].CellStyle.Font.FontName = "Verdana";
            worksheet.Range["A1:A2"].CellStyle.Font.Bold = true;
            worksheet.Range["A1"].CellStyle.Font.Size = 12;
            worksheet.Range["A1"].CellStyle.Font.Color = ExcelKnownColors.Green;
            
            worksheet.Range[$"A3:{characters[propNames.Count - 1]}3"].CellStyle = headingStyle;
            worksheet.Range[$"A3:{characters[propNames.Count - 1]}3"].CellStyle.Font.Color = ExcelKnownColors.White;
            worksheet.Range[$"A3:{characters[propNames.Count - 1]}3"].CellStyle.Font.FontName ="Verdana";
            worksheet.Range[$"A3:{characters[propNames.Count - 1]}3"].CellStyle.Font.Size = 10;
            worksheet.Range[$"A3:{characters[propNames.Count - 1]}3"].CellStyle.Color = Color.FromArgb(58, 56, 56);

            //Auto-Fit the range
            // worksheet.Range[$"A1:{characters[propNames.Count - 1]}1"].AutofitColumns();
            
            //Import the data to worksheet
            worksheet.ImportData(data, 4, 1, false);
        }
        else
        {
            for (int i = 0; i < propNames.Count; i++)
            {
                worksheet.Range[$"{characters[i]}1"].Text = propNames[i];
            }

            worksheet.Range[$"A1:{characters[propNames.Count - 1]}1"].CellStyle = headingStyle;
            worksheet.Range[$"A1:{characters[propNames.Count - 1]}1"].CellStyle.Font.Color = ExcelKnownColors.White;
            worksheet.Range[$"A1:{characters[propNames.Count - 1]}1"].CellStyle.Color = Color.FromArgb(58, 56, 56);
            
            //Import the data to worksheet
            worksheet.ImportData(data, 2, 1, false);
        }
        // worksheet.Range["A2:A5"].AutofitRows();

        worksheet.UsedRange.AutofitColumns();

        //Saving the Excel to the MemoryStream 
        using MemoryStream stream = new();

        workbook.SaveAs(stream);

        return stream;
    }
    
}