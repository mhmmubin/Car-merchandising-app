using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AssignmentP3
{
   

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        //Hamburger Button
        private void Hamburgerbutton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

       
        const decimal GST = 0.1m;
        const decimal BELOW_25_INSURANCE_COST = 0.2m;
        const decimal ABOVE_25_INSURANCE_COST = 0.1m;

        // Warrenty method
        private decimal calcVehicleWarrenty(decimal carPrice)
        {
            decimal warrentyCost =0;
            if (RadioButton1.IsChecked == true)
            {
                warrentyCost = 0;
            }
            if (RadioButton2.IsChecked == true)
            {
                warrentyCost = carPrice * 0.05m;
            }
            if (RadioButton3.IsChecked == true)
            {
                warrentyCost = carPrice * 0.1m;
            }
            if (RadioButton5.IsChecked == true)
            {
                warrentyCost = carPrice * 0.2m;
            }
            return warrentyCost;
        }

        // Method for Optional Extra
        private decimal calcOptionalExtras()
        {
            decimal feautresCost1 =150;
            decimal feautresCost2 = 300;
            decimal feautresCost3 = 250;
            decimal feautresCost4 = 350;
            decimal feautresCost5 = 50;
            decimal feautresCost6 = 200;
            decimal totalFeautrecost = 0;

            if (checkBox1.IsChecked == true)
            {
                totalFeautrecost += feautresCost1;
            }
            if (checkBox2.IsChecked == true)
            {
                totalFeautrecost += feautresCost2;
            }
            if (checkBox3.IsChecked == true)
            {
                totalFeautrecost += feautresCost3;
            }
            if (checkBox4.IsChecked == true)
            {
                totalFeautrecost += feautresCost4;
            }
            if (checkBox5.IsChecked == true)
            {
                totalFeautrecost += feautresCost5;
            }
            if (checkBox6.IsChecked == true)
            {
                totalFeautrecost += feautresCost6;
            }

            return totalFeautrecost;
        }

        // method for Insurance calculation
        private decimal calcAccidentInsurance(decimal carPrice, decimal feautres)
        {
            decimal insuranceCost = 0;

            if (age25Radiobutton.IsChecked == true)
            {
                insuranceCost = (carPrice + feautres) * BELOW_25_INSURANCE_COST;
            }
            if (ageAbove25Radiobutton.IsChecked == true)
            {
                insuranceCost = (carPrice + feautres) * ABOVE_25_INSURANCE_COST;
            }

            return insuranceCost;
        }



        //Save button
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {

            if (customernameTextbox.Text == "")
            {
                var dialog = new MessageDialog("Customer Name can't be empty! ");
                await dialog.ShowAsync();
                customernameTextbox.Focus(FocusState.Programmatic);
            }
            else if (customerphoneTextbox.Text == "")
            {
                var dialog = new MessageDialog("Customer Phone number can't be empty! ");
                await dialog.ShowAsync();
                customerphoneTextbox.Focus(FocusState.Programmatic);
            }

        }

        //Calculation button
        private async void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            decimal carPrice = 0, tradeinPrice = 0, warrentyCost, insuranceCost, feautresCost, subAmount, gstAmount, finalPrice;

            if (vehiclepriceTextbox.Text == "")
            {
                var dialog = new MessageDialog("Vehicle Price can't be empty!");
                await dialog.ShowAsync();
                vehiclepriceTextbox.Focus(FocusState.Programmatic);
                return;
            }
            else
            {
                carPrice = decimal.Parse(vehiclepriceTextbox.Text);
            }

            if (tradeinTextbox.Text == "")
            {
                tradeinTextbox.Text = "0";
            }
            else
            {
                tradeinPrice = decimal.Parse(tradeinTextbox.Text);
            }
            if (carPrice <= tradeinPrice)
            {
                var dialog = new MessageDialog(" Trade-In price can't be greater or equal to Vehicle Price");
                await dialog.ShowAsync();
                tradeinTextbox.Focus(FocusState.Programmatic);
                return;
            }


            warrentyCost = calcVehicleWarrenty(carPrice);                       //warrenty method calling.   
            feautresCost = calcOptionalExtras();                                //optional-extra method calling.
            insuranceCost = calcAccidentInsurance(carPrice, feautresCost);      //Insurance method calling

            //Subamount calculation
            subAmount = (carPrice + warrentyCost + feautresCost + insuranceCost) - tradeinPrice;
            subamount1Textblock.Text = subAmount.ToString();

            //GST calculation
            gstAmount = subAmount * GST;
            gstamount1Textblock.Text = gstAmount.ToString();

            //Final Price Calculation
            finalPrice = gstAmount + subAmount;
            finalamount1Textblock.Text = finalPrice.ToString();

            //dispalying all together.
            outputListbox.Items.Add("  Name: " + customernameTextbox.Text
                                    + "\n \n Phone: " + customerphoneTextbox.Text
                                    + "\n \n Vehicle: " + vehiclepriceTextbox.Text
                                    + "\n \n Trade-In: " + tradeinTextbox.Text
                                    + "\n \n Warrenty: " + warrentyCost
                                    + "\n \n Extras: " + feautresCost
                                    + "\n \n Insurance: " + insuranceCost
                                    + "\n \n Sub-Amount: " + subAmount
                                    + "\n \n GST: " + gstAmount
                                    + "\n \n Final-Price: "+ finalPrice);
        
        }

        private void insurance_Checked(object sender, RoutedEventArgs e)
        {
            age25Radiobutton.IsEnabled = true;
            ageAbove25Radiobutton.IsEnabled = true;
            age25Radiobutton.IsChecked = true;
        }

        private void insurance_Unchecked(object sender, RoutedEventArgs e)
        {
            age25Radiobutton.IsEnabled = false;
            ageAbove25Radiobutton.IsEnabled = false;
        }

       //Reset button 
             private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            customernameTextbox.Text = "";
            customernameTextbox.Focus(FocusState.Programmatic);
            customerphoneTextbox.Text = "";
            vehiclepriceTextbox.Text = "";
            tradeinTextbox.Text = "";
            subamount1Textblock.Text = "";
            gstamount1Textblock.Text = "";
            finalamount1Textblock.Text = "";
            insurance.IsChecked = false;

           outputListbox.Items.Clear();

            checkBox1.IsChecked = false;
            checkBox2.IsChecked = false;
            checkBox3.IsChecked = false;
            checkBox4.IsChecked = false;
            checkBox5.IsChecked = false;
            checkBox6.IsChecked = false;

            RadioButton1.IsChecked = false;
            RadioButton2.IsChecked = false;
            RadioButton3.IsChecked = false;
            RadioButton5.IsChecked = false;
            
        }
    
    }
}
