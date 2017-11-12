using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVToQIF.Model
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }

        private string FormattedDate { get { return Date.ToString("dd/MM/yyyy"); } }

        public static List<string> SplitCSV(string input)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            List<string> list = new List<string>();
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length)
                {
                    list.Add("");
                }

                list.Add(curr.TrimStart(',').Replace("\"", ""));
            }

            return list;
        }

        public Transaction(string transactionLine, CsvFileSetting fileSetting)
        {
            var listOfTransactionCodes = SplitCSV(transactionLine);
            Func<int, string> get = (index) => index >= 0 ? listOfTransactionCodes[index] : string.Empty;
            Date = DateTime.Parse(listOfTransactionCodes[fileSetting.DateIndex]);
            Payee = get(fileSetting.PayeeIndex);
            Memo = get(fileSetting.MemoIndex1) + " " + get(fileSetting.MemoIndex2);

            if (fileSetting.CombinedCreditDebitAmountIndex != -1)
            {
                string data = listOfTransactionCodes[fileSetting.CombinedCreditDebitAmountIndex].Replace("SGD", "").Trim();
                var multiplier = data.EndsWith("DR") ? -1 : 1;
                data = data.Replace("CR", "").Replace("DR", "").Trim();
                Amount = Decimal.Parse(data) * multiplier;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(listOfTransactionCodes[fileSetting.CreditAmountIndex]))
                {
                    Amount = Decimal.Parse(listOfTransactionCodes[fileSetting.CreditAmountIndex]);
                }
                else
                {
                    Amount = Decimal.Parse(listOfTransactionCodes[fileSetting.DebitAmountIndex]) * fileSetting.DebitAmountMultiplier;
                }

                if (fileSetting.AmountReferenceIndex != -1)
                {
                    var refCode = get(fileSetting.AmountReferenceIndex);
                    switch (refCode)
                    {
                        case "AWL":
                            Payee = "[ATM Withdrawal]";
                            return;
                        case "INT":
                            Payee = "Interest";
                            Memo = "Interest";
                            return;
                    }
                }
            }
        }

        public string Record
        {
            get
            {
                return $"D{FormattedDate}\nP{Payee}\nM{Memo}\nT{Amount:0.00}\n^";
            }
        }
    }
}
