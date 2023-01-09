using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace procesor.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, TextBox> registerMap;


        public MainWindow()
        {
            InitializeComponent();
            registerMap = new Dictionary<string, TextBox>
    {
        {"AH", AH},
        {"BH", BH},
        {"CH", CH},
        {"DH", DH},
        {"AL", AL},
        {"BL", BL},
        {"CL", CL},
        {"DL", DL}
    };
        }

        private void MOV_Click(object sender, RoutedEventArgs e)
        {
            // Get the names of the source and destination registers
            var sourceName = MOVSource.SelectionBoxItem.ToString();
            var destName = MOVDestination.SelectionBoxItem.ToString();

            // Get the TextBox elements for the source and destination registers
            TextBox sourceBox;
            TextBox destBox;
            if (registerMap.TryGetValue(sourceName, out sourceBox) && registerMap.TryGetValue(destName, out destBox))
            {
                // Try to parse the value in the source TextBox as an integer
                int value;
                if (int.TryParse(sourceBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                {
                    // Set the value of the destination TextBox to the parsed value
                    destBox.Text = value.ToString("X2");
                }
                else
                {
                    // Display an error message if the value is not a valid integer
                    MessageBox.Show("Invalid input");
                }
            }
        }

        private void XCHG_Click(object sender, EventArgs e)
        {
            // Get the names of the registers to exchange
            var sourceName = XCHGSource.SelectionBoxItem.ToString();
            var destinationName = XCHGDestination.SelectionBoxItem.ToString();

            // Get the TextBox elements for the registers
            TextBox sourceBox;
            TextBox destinationBox;
            if (registerMap.TryGetValue(sourceName, out sourceBox) && registerMap.TryGetValue(destinationName, out destinationBox))
            {
                // Exchange the values of the registers
                var temp = sourceBox.Text.ToUpper();
                sourceBox.Text = destinationBox.Text.ToUpper();
                destinationBox.Text = temp;
            }
        }

        private void HexValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9a-fA-F]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void INC_Click(object sender, EventArgs e)
        {
            ModifyRegister(1, "Overflow occured");
        }

        private void DEC_Click(object sender, EventArgs e)
        {
            ModifyRegister(-1, "Underflow occured");
        }
        private void NOT_Click(object sender, EventArgs e)
        {
            // Get the selected register name
            var registerName = NOT.SelectionBoxItem.ToString();

            // Get the TextBox element for the selected register
            TextBox textBox;
            if (registerMap.TryGetValue(registerName, out textBox))
            {
                // Try to parse the value in the TextBox as an integer
                int value;
                if (int.TryParse(textBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                {
                    // Perform a bitwise NOT operation on the value
                    value = ~value;

                    // Update the TextBox with the modified value
                    textBox.Text = value.ToString("X2");
                }
                else
                {
                    // Display an error message if the value is not a valid integer
                    MessageBox.Show("Invalid input");
                }
            }
        }
        private void ADD_Click(object sender, EventArgs e)
        {
            // Get the selected register names
            var destinationName = ADDDestination.SelectionBoxItem.ToString();
            var sourceName = ADDSource.SelectionBoxItem.ToString();

            // Get the TextBox elements for the selected registers
            TextBox destinationTextBox;
            TextBox sourceTextBox;
            if (registerMap.TryGetValue(destinationName, out destinationTextBox) && registerMap.TryGetValue(sourceName, out sourceTextBox))
            {
                // Try to parse the values in the TextBoxes as integers
                int destinationValue;
                int sourceValue;
                if (int.TryParse(destinationTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out destinationValue) && int.TryParse(sourceTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out sourceValue))
                {
                    // Add the values and check for overflow/underflow
                    int result = destinationValue + sourceValue;
                    if (result > 255)
                    {
                        MessageBox.Show("Overflow occured");
                    }
                    else
                    {
                        destinationTextBox.Text = result.ToString("X2");
                    }
                }
                else
                {
                    // Display an error message if the values are not valid integers
                    MessageBox.Show("Invalid input");
                }
            }
        }

        private void SUB_Click(object sender, EventArgs e)
        {
            // Get the selected register names
            var destinationName = ADDDestination.SelectionBoxItem.ToString();
            var sourceName = ADDSource.SelectionBoxItem.ToString();

            // Get the TextBox elements for the selected registers
            TextBox destinationTextBox;
            TextBox sourceTextBox;
            if (registerMap.TryGetValue(destinationName, out destinationTextBox) && registerMap.TryGetValue(sourceName, out sourceTextBox))
            {
                // Try to parse the values in the TextBoxes as integers
                int destinationValue;
                int sourceValue;
                if (int.TryParse(destinationTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out destinationValue) && int.TryParse(sourceTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out sourceValue))
                {
                    // Sub the values and check for overflow/underflow
                    int result = destinationValue - sourceValue;
                    if (result < 0)
                    {
                        MessageBox.Show("Underflow occured");
                    }
                    else
                    {
                        destinationTextBox.Text = result.ToString("X2");
                    }
                }
                else
                {
                    // Display an error message if the values are not valid integers
                    MessageBox.Show("Invalid input");
                }
            }
        }
        private void AND_Click(object sender, EventArgs e)
        {
            // Get the names of the destination and source registers
            var destName = ANDDestination.SelectionBoxItem.ToString();
            var srcName = ANDSource.SelectionBoxItem.ToString();

            // Get the TextBox elements for the registers
            TextBox destBox;
            TextBox srcBox;
            if (registerMap.TryGetValue(destName, out destBox) && registerMap.TryGetValue(srcName, out srcBox))
            {
                // Try to parse the values in the TextBox elements as integers
                int destValue;
                int srcValue;
                if (int.TryParse(destBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out destValue) &&
                    int.TryParse(srcBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out srcValue))
                {
                    // Perform the AND operation and store the result in the destination register
                    destValue &= srcValue;
                    destBox.Text = destValue.ToString("X2");
                }
                else
                {
                    // Display an error message if the values are not valid integers
                    MessageBox.Show("Invalid input");
                }
            }
        }

        private void OR_Click(object sender, EventArgs e)
        {
            // Get the destination and source register names
            var destinationName = ORDestination.SelectionBoxItem.ToString();
            var sourceName = ORSource.SelectionBoxItem.ToString();
            // Get the TextBox elements for the registers
            TextBox destinationBox;
            TextBox sourceBox;
            if (registerMap.TryGetValue(destinationName, out destinationBox) && registerMap.TryGetValue(sourceName, out sourceBox))
            {
                // Try to parse the values in the TextBox elements as integers
                int destinationValue;
                int sourceValue;
                if (int.TryParse(destinationBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out destinationValue) &&
                    int.TryParse(sourceBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out sourceValue))
                {
                    // Perform the OR operation and update the destination register
                    destinationValue |= sourceValue;
                    destinationBox.Text = destinationValue.ToString("X2");
                }
                else
                {
                    // Display an error message if the values are not valid integers
                    MessageBox.Show("Invalid input");
                }

            }
        }
        private void XOR_Click(object sender, EventArgs e)
        {
            // Get the selected register names
            var destRegisterName = XORDestination.SelectionBoxItem.ToString();
            var srcRegisterName = XORSource.SelectionBoxItem.ToString();
            // Get the TextBox elements for the selected registers
            TextBox destTextBox;
            TextBox srcTextBox;
            if (registerMap.TryGetValue(destRegisterName, out destTextBox) && registerMap.TryGetValue(srcRegisterName, out srcTextBox))
            {
                // Try to parse the values in the TextBoxes as integers
                int destValue;
                int srcValue;
                if (int.TryParse(destTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out destValue) &&
                    int.TryParse(srcTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out srcValue))
                {
                    // Perform the XOR operation and update the destination register
                    destValue = destValue ^ srcValue;
                    destTextBox.Text = destValue.ToString("X2");
                }
                else
                {
                    // Display an error message if the values are not valid integers
                    MessageBox.Show("Invalid input");
                }
            }

        }

        private void ModifyRegister(int delta, string errorMessage)
        {

            // Get the selected register name
            var registerName = DEC.SelectionBoxItem.ToString();

            // Get the TextBox element for the selected register
            TextBox textBox;
            if (registerMap.TryGetValue(registerName, out textBox))
            {
                // Try to parse the value in the TextBox as an integer
                int value;
                if (int.TryParse(textBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                {
                    // Modify the value if it is within the valid range
                    if (value + delta >= 0 && value + delta <= 255)
                    {
                        value += delta;
                        textBox.Text = value.ToString("X2");
                    }
                    else
                    {
                        // Display an error message if the value is outside the valid range
                        MessageBox.Show(errorMessage);
                    }
                }
                else
                {
                    // Display an error message if the value is not a valid integer
                    MessageBox.Show("Invalid input");
                }
            }
        }

    }
}