using System.ComponentModel;

namespace XamarinFormsBox.Models.Gatt.Characteristics
{
    public enum BodySensorLocations
    {
        [Description("Other")]
        Other,
        [Description("Chest")]
        Chest,
        [Description("Wrist")]
        Wrist,
        [Description("Finger")]
        Finger,
        [Description("Hand")]
        Hand,
        [Description("Ear Lobe")]
        EarLobe,
        [Description("Foot")]
        Foot
    }
}
