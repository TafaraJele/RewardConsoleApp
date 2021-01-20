using Microsoft.Extensions.Configuration;
using RewardConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RewardConsoleApp.SQL
{
    public class RewardFileDbContext
    {
        private string _connectionstring;
        List<RewardCustomerDetails> customers = new List<RewardCustomerDetails>();
        public RewardFileDbContext(IConfiguration iconfiguration)
        {
            _connectionstring = iconfiguration.GetConnectionString("CIFCards");

        }
        //Gets Cards from Database
        public List<RewardCustomerDetails> GetList()

        {
            //var customers = new List<RewardCustomerDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {

                    SqlCommand cmd = new SqlCommand("SELECT customer_first_name ,customer_middle_name , customer_last_name ,date_issued ,customer_title_id, CustomerId ,customer_account_id  FROM customer_account", con);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var bytes = (byte[])rdr[0];
                       // var bytes = rdr.GetValue(0);
                       var value = BitConverter.ToString(bytes);
                       // var name = System.Text.Encoding.ASCII.GetString(value);

                        ASCIIEncoding ascii = new ASCIIEncoding();
                        String decoded = ascii.GetString(bytes);

                        // var value = System.Text.Encoding.UTF8.GetString(bytes);
                        // var value = System.Text.Encoding.Unicode.GetString(bytes);
                        // var value = System.Text.Encoding.Default.GetString(bytes);
                        customers.Add(new RewardCustomerDetails
                        {
                           // CustomerFirstName = bytes,
                            CustomerFirstName = rdr[0].ToString(),
                            CustomerMiddleName = rdr[1].ToString(),
                            CustomerLastName = rdr[2].ToString(),
                            DateIssued = rdr[3].ToString(),
                            CustomerTitleId = rdr[4].ToString(),
                            CustomerId = rdr[5].ToString(),
                            CustomerAccountId = rdr[6].ToString()

                        }) ;
                    }
                    rdr.Close();
                    cmd.Dispose();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            // var todayCustomers = customers.Where(s => s.DateIssued == (DateTime.Today).ToString()).Select(s => s).ToList();
            var accountCards = new List<AccountCards>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {

                    SqlCommand cmd = new SqlCommand("SELECT customer_account_id, card_id FROM customer_account_cards", con);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        accountCards.Add(new AccountCards
                        {
                            CustomerAccountId = rdr[0].ToString(),
                            CardId = rdr[1].ToString()
                        });
                    }
                    rdr.Close();
                    cmd.Dispose();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            foreach (var customer in customers)
            {
                var accountcard = accountCards.Where(a => a.CustomerAccountId == customer.CustomerAccountId).Select(a => a).FirstOrDefault();
                if (accountcard != null)
                {
                    customer.CardId = accountcard.CardId;
                }

            }


            var cards = new List<RewardCardDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {

                    SqlCommand cmd = new SqlCommand("SELECT card_id, card_number ,product_id FROM cards", con);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        cards.Add(new RewardCardDetails
                        {
                            CardId = rdr[0].ToString(),
                            CardNumber = rdr[1].ToString(),
                            ProductId = rdr[2].ToString()
                        });
                    }
                    rdr.Close();
                    cmd.Dispose();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            foreach (var customer in customers)
            {
                var card = (cards.Where(c => c.CardId == customer.CardId)).Select(c => c).FirstOrDefault();
                if (card != null)
                {
                    customer.CardNumber = card.CardNumber;
                    customer.ProductId = card.ProductId;
                }

            }

            var issueProducts = new List<IssuerProduct>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {

                    SqlCommand cmd = new SqlCommand("SELECT product_id, product_code ,issuer_id,product_bin_code FROM issuer_product", con);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        issueProducts.Add(new IssuerProduct
                        {
                            ProductId = rdr[0].ToString(),
                            ProductCode = rdr[1].ToString(),
                            IssuerId = rdr[2].ToString(),
                            ProductBinCode = rdr[3].ToString()

                        });

                    }
                    rdr.Close();
                    cmd.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            foreach (var customer in customers)
            {
                var issueProduct = issueProducts.Where(p => p.ProductId == customers.FirstOrDefault().ProductId).Select(p => p);
                customer.ProductCode = issueProduct.FirstOrDefault().ProductCode;
                customer.ProductBinCode = issueProduct.FirstOrDefault().ProductBinCode;
                customer.IssuerId = issueProduct.FirstOrDefault().IssuerId;
            }


            var issuers = new List<Issuer>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {

                    SqlCommand cmd = new SqlCommand("SELECT issuer_id , issuer_code FROM issuer", con);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        issuers.Add(new Issuer
                        {
                            IssuerId = rdr[0].ToString(),                            
                            IssuerCode = rdr[1].ToString(),
                           
                        });
                    }
                    rdr.Close();
                    cmd.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            foreach (var customer in customers)
            {
                var issuer = issuers.Where(i => i.IssuerId == customer.IssuerId).Select(i => i).FirstOrDefault();
                if(issuer.IssuerCode != null)
                { customer.IssueBankShortName = issuer.IssuerCode; }
              
            }

            return customers;
        }


    }
   
}




