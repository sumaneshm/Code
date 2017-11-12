using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVToQIF.Model
{
    public class CsvQifConverter
    {
        public void Convert(string fileName)
        {
            var nonEmptyLines = File.ReadAllLines(fileName).Where(f => !string.IsNullOrWhiteSpace(f)).Select(l => l.Trim());
            DetermineCsvType(nonEmptyLines.First());
            CsvFileSetting fileSetting = CsvFileSetting.Get(csvType);

            var transactionLines = nonEmptyLines
                .Skip(fileSetting.InitialSkipLines)
                .Take(nonEmptyLines.Count() - fileSetting.InitialSkipLines - fileSetting.FinalSkipLines).ToList();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("!Type:Bank\n^");
            foreach (var line in transactionLines)
            {
                Transaction trans = new Transaction(line, fileSetting);
                builder.AppendLine(trans.Record);
            }
            var inputFileInfo = new FileInfo(fileName);
            string outputFileName = Path.Combine(inputFileInfo.Directory.FullName, inputFileInfo.Name + ".qif");
            File.WriteAllText(outputFileName, builder.ToString());
        }

        private void DetermineCsvType(string line)
        {
            if (line.Contains("DBS Savings Account"))
            {
                csvType = CSV_Type.DBS_SAVING_PLUS;
            }
            else if (line.StartsWith("Account transactions shown"))
            {
                csvType = CSV_Type.SC_CHECKING;
            }
            else if (line.StartsWith("UNLIMITED CASHBACK CREDIT CARD"))
            {
                csvType = CSV_Type.SC_CREDIT_CARD;
            }
            else if (line.StartsWith("\""))
            {
                csvType = CSV_Type.CITIBANK_SMRT;
            }
            else
                throw new InvalidOperationException("Not recognized CSV file");
        }

        private CSV_Type csvType;
    }
}
