/* Student Name: Vimal
 * Student ID: 21236713
 * Date:12/11/2021
 * Assignment: 3
 * Assignment: Create an application called Invest4U which allows the Mad4Money Bank Corp to offer an investment 
 * related service to the customers where a client invests a principal sum of money and receive returns depending on the 
 * amount and duration of their investment.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Invest4UApp
{
    public partial class invest4UForm : Form
    {
        //Constants declared
        private const string USER_PASSWORD = "ShowMeTheMoney#";
        private const int WRONG_ATTEMPTS_COUNT = 3;

        //Constants for Adjusting Form Size
        private const int FORM_EXPANDED_WIDTH = 890;
        private const int FORM_EXPANDED_HEIGHT = 800;
        private const int FORM_INITIAL_WIDTH = 492;
        private const int FORM_INITIAL_HEIGHT = 800;

        private const decimal BASIC_THRESHOLD_AMOUNT = 100000m;

        //Constants for Interest Rates
        private const decimal BASIC_1_YEAR_INTEREST_RATE = 0.005m;
        private const decimal BASIC_3_YEARS_INTEREST_RATE = 0.00625m;
        private const decimal BASIC_5_YEARS_INTEREST_RATE = 0.007125m;
        private const decimal BASIC_10_YEARS_INTEREST_RATE = 0.01125m;

        private const decimal PREMIUM_1_YEAR_INTEREST_RATE = 0.006m;
        private const decimal PREMIUM_3_YEARS_INTEREST_RATE = 0.00725m;
        private const decimal PREMIUM_5_YEARS_INTEREST_RATE = 0.008125m;
        private const decimal PREMIUM_10_YEARS_INTEREST_RATE = 0.0125m;

        private const decimal BONUS_TERM_COMPLETION_AMOUNT = 25000m;
        private const decimal BONUS_TERM_REQUIRED_INVESTMENT = 1000000m;

        //Transaction File
        private const string TRANSACTION_FILE = "InvestmentDetails.txt";

        //Global Variables declared
        int wrongPasswordCountFlag = 0;
        private decimal investmentAmount = 0m;
        private decimal futureValueOfInvestment = 0m;
        private decimal investmentInterestRate = 0m;
        private decimal interestAccrued = 0m;
        private int investmentTenure = 0;
        private int transactionID = 0;
        private string currentDate = "";
        private string clientFullName = "";
        private string clientPhoneNumber = "";
        private string clientEmailAddress = "";
        private string stdDetails = "{0, -15}   {1, -25}{2, -25}{3, -20}{4, -20}{5, -25}{6, -25}{7, -25}{8, -25}";

        public invest4UForm()
        {
            InitializeComponent();
        }

        //Method called when is Form is loaded the first time
        private void invest4UForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(FORM_INITIAL_WIDTH, FORM_INITIAL_HEIGHT);
        }

        //Event handler to validate the password entered and the Login button is clicked
        private void loginButton_Click(object sender, EventArgs e)
        {
            if (userPasswordTextBox.Text == USER_PASSWORD)
            {
                userPasswordLabel.Visible = false;
                userPasswordTextBox.Visible = false;
                loginButton.Visible = false;
                this.expandFormSize();
                enterInvestmentAmountSectionPanel.Visible = true;
                functionalityButtonsSectionPanel.Visible = true;
                bonusOfferPanel.Visible = true;
                showPasswordButton.Visible = false;
                investmentAmountTextBox.Focus();
            }
            else
            {
                if (wrongPasswordCountFlag < WRONG_ATTEMPTS_COUNT)
                {
                    this.showErrorDialogBox("The password entered is incorrect. Please try again.\n You have " + (WRONG_ATTEMPTS_COUNT - wrongPasswordCountFlag) + " attempt(s) left", "Password Error");
                    wrongPasswordCountFlag++;
                    userPasswordTextBox.Text = "";
                    userPasswordTextBox.Focus();
                }
                else
                {
                    this.showErrorDialogBox("You have exceeded the maximum number of attempts.", "Password Error");
                    this.Close();
                }
            }
        }

        //Method to resize the form based on its contents 
        private void expandFormSize()
        {
            this.Size = new Size(FORM_EXPANDED_WIDTH, FORM_EXPANDED_HEIGHT);
        }

        //Method to show the error dialog when an invalid input data is entered
        private void showErrorDialogBox(string message, string headerTitle)
        {
            MessageBox.Show(message, headerTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Event handler called when the Display button is clicked upon entering the Principal amount
        private void displayTermPlansButton_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(investmentAmountTextBox.Text, out investmentAmount) && (investmentAmount > 0))
            {
                enterInvestmentAmountSectionPanel.Enabled = false;
                this.calculateAndShowTermPlanOptions();
            }
            else
            {
                this.showErrorDialogBox("Please enter a valid input to proceed.", "Data Entry Error");
            }
        }

        //Method to show various term plans along with their interest rates and estimated future earnings
        private void calculateAndShowTermPlanOptions()
        {
            if (investmentAmount > BASIC_THRESHOLD_AMOUNT)
            {   
                oneYearInterestRateLabel.Text = PREMIUM_1_YEAR_INTEREST_RATE.ToString();
                threeYearsInterestRateLabel.Text = PREMIUM_3_YEARS_INTEREST_RATE.ToString();
                fiveYearsInterestRateLabel.Text = PREMIUM_5_YEARS_INTEREST_RATE.ToString();
                tenYearsInterestRateLabel.Text = PREMIUM_10_YEARS_INTEREST_RATE.ToString();
                oneYearFutureValueLabel.Text = this.calculateFutureValueInvestment(PREMIUM_1_YEAR_INTEREST_RATE, 1).ToString("c");
                threeYearsFutureValueLabel.Text = this.calculateFutureValueInvestment(PREMIUM_3_YEARS_INTEREST_RATE, 3).ToString("c");
                fiveYearsFutureValueLabel.Text = this.calculateFutureValueInvestment(PREMIUM_5_YEARS_INTEREST_RATE, 5).ToString("c");
                tenYearsFutureValueLabel.Text = this.calculateFutureValueInvestment(PREMIUM_10_YEARS_INTEREST_RATE, 10).ToString("c");
            }
            else
            {
                oneYearInterestRateLabel.Text = BASIC_1_YEAR_INTEREST_RATE.ToString();
                threeYearsInterestRateLabel.Text = BASIC_3_YEARS_INTEREST_RATE.ToString();
                fiveYearsInterestRateLabel.Text = BASIC_5_YEARS_INTEREST_RATE.ToString();
                tenYearsInterestRateLabel.Text = BASIC_10_YEARS_INTEREST_RATE.ToString();
                oneYearFutureValueLabel.Text = this.calculateFutureValueInvestment(BASIC_1_YEAR_INTEREST_RATE, 1).ToString("c");
                threeYearsFutureValueLabel.Text = this.calculateFutureValueInvestment(BASIC_3_YEARS_INTEREST_RATE, 3).ToString("c");
                fiveYearsFutureValueLabel.Text = this.calculateFutureValueInvestment(BASIC_5_YEARS_INTEREST_RATE, 5).ToString("c");
                tenYearsFutureValueLabel.Text = this.calculateFutureValueInvestment(BASIC_10_YEARS_INTEREST_RATE, 10).ToString("c");
            }
            investmentTermPlansGroupBox.Visible = true;
        }

        //Method to return the future value of investments
        private decimal calculateFutureValueInvestment(decimal interestRate, int tenure)
        {
            return investmentAmount * (decimal)Math.Pow(1 + ((double)interestRate / 12), 12 * tenure);
        }

        //Method to generate a random 8-digit number
        private int generateRandomNumber() {
            Random rnd = new Random();
            return rnd.Next(10000000, 99999999);
        }

        //Method to parse the file contents to check the uniqueness of the Transaction ID/Number
        private int generateUniqueTransactionID()
        {
            int generatedTransactionNumber = this.generateRandomNumber();
            StreamReader inputFile;
            string investmentLineData = "";
            inputFile = File.OpenText(TRANSACTION_FILE);
            while (!inputFile.EndOfStream)
            {
                investmentLineData = inputFile.ReadLine();
                if (investmentLineData.Substring(0, 8).Equals(generatedTransactionNumber.ToString()))
                {
                    generatedTransactionNumber = this.generateRandomNumber();
                }
            }
            inputFile.Close();

            return generatedTransactionNumber;
        }

        //Method to fetch the current date in (dd-mm-yyyy) format
        private string getCurrentDate() {
            return DateTime.Today.ToString("dd-MM-yyyy");
        }

        //Event handler to be called when Proceed button is clicked after selecting the appropriate term plan option
        private void proceedButton_Click(object sender, EventArgs e)
        {
            this.investmentTermPlansGroupBox.Enabled = false;
            transactionID = this.generateUniqueTransactionID();
            transactionIDValueLabel.Text = transactionID.ToString();
            currentDate = this.getCurrentDate();
            transactionDateValueLabel.Text = currentDate.ToString();
            if (oneYearPlanOptionRadioButton.Checked) {
                investmentTenure = 1;
                investmentInterestRate = decimal.Parse(oneYearInterestRateLabel.Text);
            }
            if (threeYearsPlanOptionRadioButton.Checked)
            {
                investmentTenure = 3;
                investmentInterestRate = decimal.Parse(threeYearsInterestRateLabel.Text);
            }
            if (fiveYearsPlanOptionRadioButton.Checked)
            {
                investmentTenure = 5;
                investmentInterestRate = decimal.Parse(fiveYearsInterestRateLabel.Text);
            }
            if (tenYearsPlanOptionRadioButton.Checked)
            {
                investmentTenure = 10;
                investmentInterestRate = decimal.Parse(tenYearsInterestRateLabel.Text);
            }

            futureValueOfInvestment = this.calculateFutureValueInvestment(investmentInterestRate, investmentTenure);
            interestAccrued = futureValueOfInvestment - investmentAmount;
            enterClientDetailsGroupBox.Visible = true;
            clientFullNameTextBox.Focus();
        }

        //Method to check if a valid email has been entered
        private bool checkEmailValidity(string emailAddress) {
            if (clientEmailAddressTextBox.Text.Contains("@")) {
                int indexOfAt = clientEmailAddressTextBox.Text.IndexOf('@');
                string extract = clientEmailAddressTextBox.Text.Substring(indexOfAt);
                return extract.Contains(".");
            }
            return false; //Return false when the email entered is invalid
        }

        //Event handler to be called when Submit button is clicked after the client details are recorded
        private void submitFormButton_Click(object sender, EventArgs e)
        {
            //Checking the validity of Client's name
            if (clientFullNameTextBox.Text.Length > 0 && !clientFullNameTextBox.Text.Any(char.IsDigit))
            {
                clientFullName = clientFullNameTextBox.Text;
                //Checking the validity of Client's phone number
                if (clientPhoneNumberTextBox.Text.Length > 0 && clientPhoneNumberTextBox.Text.All(char.IsDigit))
                {
                    clientPhoneNumber = clientPhoneNumberTextBox.Text;
                    //Checking the validity of Client's email
                    if (clientEmailAddressTextBox.Text.Length > 0 && this.checkEmailValidity(clientEmailAddressTextBox.Text))
                    {
                        clientEmailAddress = clientEmailAddressTextBox.Text;
                        if ((investmentAmount > BONUS_TERM_REQUIRED_INVESTMENT) && (investmentTenure >= 5)) {
                            futureValueOfInvestment += BONUS_TERM_COMPLETION_AMOUNT;
                        }
                        DialogResult dialogResult = MessageBox.Show("Transaction ID:                  " + transactionID + "\n" +
                                        "Transaction Date:              " + currentDate + "\n" +
                                        "Investment Amount:        " + investmentAmount.ToString("c") + "\n" +
                                        "Interest Rate:                     " + investmentInterestRate.ToString("p") + "\n" +
                                        "Tenure:                               " + investmentTenure + "\n" +
                                        "Future Value:                      " + futureValueOfInvestment.ToString("c") + "\n" +
                                        "Client Name:                      " + clientFullName + "\n" +
                                        "Phone Number:                 " + clientPhoneNumber + "\n" +
                                        "Email:                                 " + clientEmailAddress + "\n", "Confirmation Details", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (dialogResult == DialogResult.Yes)
                        {
                            this.saveDetailsToFile();

                        }
                        summaryDetailsButton.Enabled = true;
                        searchDetailsButton.Enabled = true;
                        this.resetForm();
                    }
                    else {
                        this.showErrorDialogBox("Please enter a valid email address", "Data Entry Error");
                    }
                }
                else
                {
                    this.showErrorDialogBox("Please enter a valid phone number", "Data Entry Error");
                }
            }
            else {
                this.showErrorDialogBox("Please enter the name of the client", "Data Entry Error");
            }
        }

        //Method to reset the form to its initial state, to allow a new transaction
        private void resetForm() {
            //Reset the Visibility and Enabled property of form elements
            enterInvestmentAmountSectionPanel.Visible = true;
            enterInvestmentAmountSectionPanel.Enabled = true;
            investmentTermPlansGroupBox.Visible = false;
            investmentTermPlansGroupBox.Enabled = true;
            enterClientDetailsGroupBox.Visible = false;
            enterClientDetailsGroupBox.Enabled = true;
            summaryDetailsPanel.Visible = false;
            searchResultsSectionPanel.Visible = false;
            backFromSummaryButton.Visible = false;
            summaryDetailsButton.Enabled = true;
            bonusOfferPanel.Visible = true;
            searchDetailsButton.Enabled = true;

            //Reset the form fields to empty values
            investmentAmountTextBox.Text = "";
            investmentAmountTextBox.Focus();
            clientFullNameTextBox.Text = "";
            clientPhoneNumberTextBox.Text = "";
            clientEmailAddressTextBox.Text = "";
            transactionNumberSummaryListBox.Items.Clear();
            interestAccruedListBox.Items.Clear();
            searchResultsListBox.Items.Clear();
            searchResultsPanel.Visible = false;
            searchResultsLabel.Visible = false;
            investmentAmountsListBox.Items.Clear();
            this.Size = new Size(FORM_EXPANDED_WIDTH, FORM_EXPANDED_HEIGHT);
        }

        //Method to save the transaction and client details to a text file for persistence
        private void saveDetailsToFile(){
            try
            {
                StreamWriter outputFile;
                outputFile = File.AppendText(TRANSACTION_FILE); //Opens the file in Append mode
                string investmentLineData = transactionID + "," + currentDate + "," + investmentAmount + "," + 
                                        investmentInterestRate + "," + investmentTenure + "," + String.Format("{0:0.0#}", interestAccrued) + "," +
                                        clientFullName + "," + clientPhoneNumber + "," + clientEmailAddress;
                outputFile.WriteLine(investmentLineData); //Writes data to file
                outputFile.Close();
            }
            catch (Exception ex) {
                this.showErrorDialogBox("Unable to save details to file.", "File Error");
            }
        }

        //Event handler called when the Clear button is clicked to clear entered values
        private void clearFormButton_Click(object sender, EventArgs e)
        {
            this.resetForm();
        }

        //Event handler called when the Exit button is clicked to close the application
        private void exitAppButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Event handler called when Summary button is clicked to fetch all the summary details pertaining to the investment transactions
        private void summaryDetailsButton_Click(object sender, EventArgs e)
        {
            this.Size = new Size(980, 729);
            decimal totalInvestedAmount = 0m;
            int totalInvestors = 0;
            int termTenure = 0;
            decimal investedAmount = 0m;
            enterInvestmentAmountSectionPanel.Visible = false;
            backFromSummaryButton.Visible = true;
            summaryDetailsButton.Enabled = false;
            searchDetailsButton.Enabled = false;
            bonusOfferPanel.Visible = false;
            try {
                string investmentLineData = "";
                StreamReader inputFile;
                inputFile = File.OpenText(TRANSACTION_FILE);

                if (File.ReadLines(TRANSACTION_FILE).Count() >= 1)
                {
                    while (!inputFile.EndOfStream)
                    {
                        totalInvestors++;
                        investmentLineData = inputFile.ReadLine();
                        //Fetch and add Transaction Number values to ListBox
                        transactionNumberSummaryListBox.Items.Add("# " + investmentLineData.Substring(0, 8));

                        //Fetch Investments Amount
                        investedAmount = decimal.Parse(this.parseInvestmentDataForFields(investmentLineData, 2));
                        totalInvestedAmount += investedAmount;
                        investmentAmountsListBox.Items.Add(investedAmount.ToString("c"));

                        //Fetch Tenure
                        termTenure += int.Parse(this.parseInvestmentDataForFields(investmentLineData, 4));

                        //Fetch and add Interest Accrued values to ListBox
                        interestAccruedListBox.Items.Add(decimal.Parse(this.parseInvestmentDataForFields(investmentLineData, 5)).ToString("c"));

                    }
                    totalAmountInvestedValueLabel.Text = totalInvestedAmount.ToString("c");
                    avgAmountInvestedValueLabel.Text = (totalInvestedAmount / totalInvestors).ToString("c");
                    avgTenureOfInvestmentsValueLabel.Text = (termTenure / totalInvestors).ToString() + " year(s)";
                    summaryDetailsPanel.Visible = true;
                }
                else {
                    this.showErrorDialogBox("There are no records in the file", "File Error");
                    this.resetForm();
                }
                inputFile.Close();
            }
            catch (Exception ex)
            {
                this.showErrorDialogBox("Unable to read details from the file", "File Error");
            }
        }

        //Event handler called when the Back button is clicked to navigate back to the main screen
        private void backFromSummaryButton_Click(object sender, EventArgs e)
        {
            this.resetForm();
            this.Size = new Size(FORM_EXPANDED_WIDTH, FORM_EXPANDED_HEIGHT);
        }

        /* Event handler called when the Search button is clicked to show the 
        search view where a search query can be typed */
        private void searchDetailsButton_Click(object sender, EventArgs e)
        {
            enterInvestmentAmountSectionPanel.Visible = false;
            summaryDetailsButton.Enabled = false;
            backFromSummaryButton.Visible = true;
            bonusOfferPanel.Visible = false;
            searchDetailsButton.Enabled = false;
            searchResultsSectionPanel.Visible = true;
            searchTextBox.Focus();
            this.Size = new Size(1230, 729);
        }

        //Event handler called when the Search button is clicked upon entering a search query in the TextBox.
        private void searchResultsButton_Click(object sender, EventArgs e)
        {
            bool dataFlag = false;
            if (searchTextBox.Text.Length > 0)
            {
                try
                {
                    string investmentLineData = "";
                    searchResultsListBox.Items.Clear();
                    searchResultsListBox.Items.Add(String.Format(stdDetails, "Transaction ID", "Date", "Invested Amount", "Interest Rate", "Tenure", "Interest Accrued", "Client Name", "Phone", "Email"));
                    StreamReader inputFile;
                    inputFile = File.OpenText(TRANSACTION_FILE);
                    if (File.ReadLines(TRANSACTION_FILE).Count() >= 1)
                    {
                        while (!inputFile.EndOfStream)
                        {
                            investmentLineData = inputFile.ReadLine();
                            //Display the results when found in the file
                            if (investmentLineData.Contains(searchTextBox.Text.Trim()))
                            {
                                searchResultsListBox.Items.Add(String.Format(stdDetails, investmentLineData.Substring(0, 8),
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 1), 
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 2), 
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 3), 
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 4),
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 5),
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 6),
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 7),
                                                                                         this.parseInvestmentDataForFields(investmentLineData, 8)));


                               searchResultsLabel.Text = "Search Results:";
                                searchResultsLabel.Visible = true;
                                searchResultsPanel.Visible = true;
                                dataFlag = true;
                            }
                        }
                        //Display No Results Found when there are no matching results
                        if (!dataFlag)
                        {
                            searchResultsLabel.Text = "No Results Found.";
                            searchResultsPanel.Visible = false;
                        }
                        searchResultsLabel.Visible = true;
                    }
                    else {
                        this.showErrorDialogBox("There are no records in the file", "File Error");
                        this.resetForm();
                    }

                    searchTextBox.Text = "";
                    inputFile.Close();
                }
                catch (Exception ex)
                {
                    this.showErrorDialogBox("Unable to read details from the file", "File Error");
                }
            }
            else {
                this.showErrorDialogBox("Please enter a valid input to search", "Data Entry Error");
            }
        }

        //Method to retrieve specific data fields from the file based on their position in the line 
        private string parseInvestmentDataForFields(string investmentLine, int iter) {
            int firstIndex = 0, nextIndex = 0;
            string newStr = "";
            for (int i = 0; i < iter; i++) {
                firstIndex = investmentLine.IndexOf(",");
                newStr = investmentLine.Substring(firstIndex + 1);
                investmentLine = newStr;
            }
            nextIndex = newStr.IndexOf(",");
            if (nextIndex == -1) {
                return newStr.Substring(0);
            }
            return newStr.Substring(0, nextIndex);
        }

        //Method to show/hide the password characters in the textbox
        private void showPasswordButton_Click_1(object sender, EventArgs e)
        {
            userPasswordTextBox.UseSystemPasswordChar = !userPasswordTextBox.UseSystemPasswordChar;
            if (!userPasswordTextBox.UseSystemPasswordChar)
            {
                showPasswordButton.Text = "Hide";
            }
            else {
                showPasswordButton.Text = "Show";
            }
        }

    }
}
