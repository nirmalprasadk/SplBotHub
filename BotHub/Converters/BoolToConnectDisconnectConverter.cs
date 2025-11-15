using System.Globalization;

namespace BotHub.Converters;

public class BoolToConnectDisconnectConverter : ConverterBase
{
    public override object Convert(object value, Type type, object parameter, CultureInfo culture) => (bool)value ? "Disconnect" : "Connect";
}