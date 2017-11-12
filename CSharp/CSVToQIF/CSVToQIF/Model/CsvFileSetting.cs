using System;
using System.Collections.Generic;
using System.Text;

namespace CSVToQIF.Model
{
    public class CsvFileSetting
    {
        public int InitialSkipLines { get; set; } = 0;
        public int FinalSkipLines { get; set; } = 0;

        public int DateIndex { get; set; } = 0;
        public int PayeeIndex { get; set; } = 0;
        public int MemoIndex1 { get; set; } = -1;
        public int MemoIndex2 { get; set; } = -1;
        public int CreditAmountIndex { get; set; } = -1;
        public int DebitAmountIndex { get; set; } = -1;
        public int CombinedCreditDebitAmountIndex { get; set; } = -1;
        public int AmountReferenceIndex { get; set; } = -1;
        public int DebitAmountMultiplier { get; set; } = -1;

        public static CsvFileSetting Get(CSV_Type csvType)
        {
            switch (csvType)
            {
                case CSV_Type.DBS_SAVING_PLUS:
                    return new CsvFileSetting { InitialSkipLines = 5, AmountReferenceIndex = 1, DebitAmountIndex = 2, CreditAmountIndex = 3, PayeeIndex = 4, MemoIndex1 = 5, MemoIndex2 = 6 };
                case CSV_Type.SC_CHECKING:
                    return new CsvFileSetting { InitialSkipLines = 4, DebitAmountIndex = 4, CreditAmountIndex = 3, PayeeIndex = 1 };
                case CSV_Type.SC_CREDIT_CARD:
                    return new CsvFileSetting { InitialSkipLines = 3, FinalSkipLines = 6, CombinedCreditDebitAmountIndex = 3, PayeeIndex = 1 };
                case CSV_Type.CITIBANK_SMRT:
                    return new CsvFileSetting { InitialSkipLines = 0, FinalSkipLines = 0, DebitAmountIndex = 3, CreditAmountIndex = 2, PayeeIndex = 1, DebitAmountMultiplier = 1 };
            }

            throw new InvalidOperationException("Failed to get File setting");
        }
    }
}
