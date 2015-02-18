# Stock-DCF-Valuation-Program
Retrieves financial data from XBRL / Yahoo / Quandl and conducts DCF Valuation
Last Updated - October 2013

Program retrieves data from 3 sources, cleans it, and stores it into a local MySQL instance:
 1. XBRL documents located on the SEC website
 2. Yahoo! Finance Web Scraping
 3. Quandl API

After data retrieval is finished, the user can run the Stock Valuation Test Class to conduct DCF analysis for all companies 
that have valid information. The DCF analysis is briefly explained below:
- Used the last 3 years of data
- Calculated Operating Profit from top down approach (Revenues – COGS – Operating Costs) and bottom up approach (Net Income + Depreciation + Taxes )
- Subtracted Working Capital (Current Assets – Current Liabilities) and Net Fixed Assets (Current PPE – Last Year PPE + depreciation) from Net Operating Profit to arrive at Free Cash Flows
- Calculated revenue growth for the next 5 years using 3 types of growth rates
   - Average growth for the past 3 years
   - Decaying growth from the Average growth rate of the past 3 years to -5% growth in the 5th projection year
   - No Revenue growth, using the average revenue growth for the past 3 years
- Projections were made using a percentage of sales
- Instead of calculating the Weighted Average Cost of Capital I used an array of WACCs from 6% to 20%
- Used terminal values ranging from -4% to 4%
- Discounted the free cash flows for the future 5 years
- Calculated the terminal value: Nop * (1-(terminalGrowth/Roic)) /(WACC – terminalGrowth)
- Because hundreds of valuations were performed for each company, I averaged the valuations for each company based on how revenue growth was calculated. 
   - I subtracted the average value from the current value of the stock price to determine how undervalued/overvalued the stock was.
   - DCF is highly subjective, so I tried to make it more objective through this method of averaging as each company was analyzed and compared using the same method. 

Printed the results to a 2 excel documents:
1. August_4_company_financial_rank_table.xlsx (Contains the DCF Valuations)
2. QuandlYearlyData_2013_10_27_2240.xlsx (Uses Quandl data to compare stock fundamentals - no DCF analysis)
