using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockValuationLibrary._2.BLL.Valuation;
using StockValuationLibrary._2.BLL;


namespace StockValuationLibrary._2.BOL
{
    public class CompanyValuationStatistics
    {
        #region PRIVATE MEMBERS

        private string _SYMBOL;
        private string _COMPANY_NAME;
        private int _YEAR;
        private int _NO_OF_YEARS;
        private IncomeStatementCollection _noGrowthProjections;
        private IncomeStatementCollection _avgGrowthProjections;
        private IncomeStatementCollection _decayGrowthProjections;
        private DiscountedCashFlowCollection _NOP_DCFS_NO_GROWTH;
        private DiscountedCashFlowCollection _NI_DCFS_NO_GROWTH;
        private DiscountedCashFlowCollection _NOP_DCFS_AVG_GROWTH;
        private DiscountedCashFlowCollection _NI_DCFS_AVG_GROWTH;
        private DiscountedCashFlowCollection _NOP_DCFS_DECAY_GROWTH;
        private DiscountedCashFlowCollection _NI_DCFS_DECAY_GROWTH;

        #endregion

        #region PUBLIC PROPERTIES

        public string Symbol
        { get { return _SYMBOL; } set { _SYMBOL = value; } }

        public string CompanyName
        { get { return _COMPANY_NAME; } set { _COMPANY_NAME = value; } }

        public int Year
        { get { return _YEAR; } set { _YEAR = value; } }

        public int NoOfYears
        { get { return _NO_OF_YEARS; } set { _NO_OF_YEARS = value; } }

        public DiscountedCashFlowCollection NopDcfsNoGrowth
        { get { return _NOP_DCFS_NO_GROWTH; } set { _NOP_DCFS_NO_GROWTH = value; } }

        public DiscountedCashFlowCollection NetIncomeDcfsNoGrowth
        { get { return _NI_DCFS_NO_GROWTH; } set { _NI_DCFS_NO_GROWTH = value; } }

        public DiscountedCashFlowCollection NopDcfsAvgGrowth
        { get { return _NOP_DCFS_AVG_GROWTH; } set { _NOP_DCFS_AVG_GROWTH = value; } }

        public DiscountedCashFlowCollection NetIncomeDcfsAvgGrowth
        { get { return _NI_DCFS_AVG_GROWTH; } set { _NI_DCFS_AVG_GROWTH = value; } }

        public DiscountedCashFlowCollection NopDcfsDecayGrowth
        { get { return _NOP_DCFS_DECAY_GROWTH; } set { _NOP_DCFS_DECAY_GROWTH = value; } }

        public DiscountedCashFlowCollection NetIncomeDcfsDecayGrowth
        { get { return _NI_DCFS_DECAY_GROWTH; } set { _NI_DCFS_DECAY_GROWTH = value; } }

        public IncomeStatementCollection NoGrowthProjections
        { get { return _noGrowthProjections; } set { _noGrowthProjections = value; } }

        public IncomeStatementCollection AvgGrowthProjections
        { get { return _avgGrowthProjections; } set { _avgGrowthProjections = value; } }

        public IncomeStatementCollection DecayGrowthProjections
        { get { return _decayGrowthProjections; } set { _decayGrowthProjections = value; } }

        #endregion

        #region CONSTRUCTORS

        public CompanyValuationStatistics()
        {
            this._NOP_DCFS_NO_GROWTH = new DiscountedCashFlowCollection();
            this._NOP_DCFS_AVG_GROWTH = new DiscountedCashFlowCollection();
            this._NOP_DCFS_DECAY_GROWTH = new DiscountedCashFlowCollection();
            this._NI_DCFS_NO_GROWTH = new DiscountedCashFlowCollection();
            this._NI_DCFS_AVG_GROWTH = new DiscountedCashFlowCollection();
            this._NI_DCFS_DECAY_GROWTH = new DiscountedCashFlowCollection();
        }

        #endregion

    }
}
