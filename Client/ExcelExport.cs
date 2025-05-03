using ClosedXML.Excel;
namespace Client;

public static class ExcelExport
{
    public static void Create(ICollection<Tuple<bool, double, string, int>> data, IList<Tuple<bool, double, int>> expected, string llm, string checkType)
    {
        using (var workbook = new XLWorkbook())
        {
            // Add a worksheet
            var worksheet = workbook.Worksheets.Add("Sheet1");

            // Add headers
            worksheet.Cell(1, 1).Value = "Test";
            worksheet.Cell(1, 2).Value = "Execution";
            worksheet.Cell(1, 3).Value = "Pass";
            worksheet.Cell(1, 4).Value = "Score";
            worksheet.Cell(1, 5).Value = "Reason";
            worksheet.Cell(1, 6).Value = "Statements";
            worksheet.Cell(1, 7).Value = "Expected Pass";
            worksheet.Cell(1, 8).Value = "Expected Score";
            worksheet.Cell(1, 9).Value = "Expected Statements";


            var currentRow = 2; // Start bei Zeile 2 (nach den Überschriften)

            for (var i = 0; i < 10; i++) // Schleife für Tests
            {
                for (var j = 0; j < 10; j++) // Schleife für Durchläufe
                {
                    worksheet.Cell(currentRow, 1).Value = i + 1; // Spalte A: Testnummer
                    worksheet.Cell(currentRow, 2).Value = j + 1; // Spalte B: Durchlaufnummer
                    
                    worksheet.Cell(currentRow, 7).Value = expected[i].Item1 ? 1 : 0; // Spalte G: Erwartetes Ergebnis (Boolean)
                    worksheet.Cell(currentRow, 8).Value = expected[i].Item2;
                    worksheet.Cell(currentRow, 9).Value = expected[i].Item3;
                    currentRow++; // Gehe zur nächsten Zeile
                }
            }

            // Populate data
            var row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 3).Value = item.Item1 ? 1 : 0; // Boolean value
                worksheet.Cell(row, 4).Value = item.Item2; // Double value
                worksheet.Cell(row, 5).Value = item.Item3;
                worksheet.Cell(row, 6).Value = item.Item4;
                row++;
            }

            // Save the workbook to a file
            workbook.SaveAs(@$"C:\Users\linus\Documents\github.com\ldehner\MindCheck\{llm}-{checkType}.xlsx");
        }

        Console.WriteLine("Excel file created successfully!");
    }
}