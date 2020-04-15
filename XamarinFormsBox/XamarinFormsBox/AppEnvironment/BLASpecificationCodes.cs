using Plugin.BLE.Abstractions.Extensions;
using System;

namespace XamarinFormsBox.AppEnvironment
{
    public static class BLASpecificationCodes
    {
        public static readonly Guid BATTERY_SERVICE;
        public static readonly Guid HEART_RATE_SERVICE;
        public static readonly Guid HEALTH_THERMOMETER_SERVICE;
        public static readonly Guid UART_SERVICE;

        public static readonly Guid MEASUREMENT_INTERVAL_CHARACTERISTIC;
        public static readonly Guid TEMPERATURE_MEASUREMENT_CHARACTERISTIC;

        public static readonly Guid BATTERY_LEVEL_CHARACTERISTIC;

        public static readonly Guid BODY_SENSOR_LOCATION_CHARACTERISTIC;
        public static readonly Guid HEART_RATE_CONTROL_POINT_CHARACTERISTIC;
        public static readonly Guid HEART_RATE_MEASUREMENT_CHARACTERISTIC;

        public static readonly Guid UART_TX_CHARACTERISTIC;
        public static readonly Guid UART_RX_CHARACTERISTIC;

        static BLASpecificationCodes()
        {
            BATTERY_SERVICE = 0x180F.UuidFromPartial();
            HEART_RATE_SERVICE = 0x180D.UuidFromPartial();
            HEALTH_THERMOMETER_SERVICE = 0x1809.UuidFromPartial();
            UART_SERVICE = Guid.ParseExact("6e400001b5a3f393e0a9e50e24dcca9e", "N");

            MEASUREMENT_INTERVAL_CHARACTERISTIC = 0x2A21.UuidFromPartial();
            TEMPERATURE_MEASUREMENT_CHARACTERISTIC = 0x2A1C.UuidFromPartial();

            BATTERY_LEVEL_CHARACTERISTIC = 0x2A19.UuidFromPartial();

            BODY_SENSOR_LOCATION_CHARACTERISTIC = 0x2A38.UuidFromPartial();
            HEART_RATE_CONTROL_POINT_CHARACTERISTIC = 0x2A39.UuidFromPartial();
            HEART_RATE_MEASUREMENT_CHARACTERISTIC = 0x2A37.UuidFromPartial();

            UART_TX_CHARACTERISTIC = Guid.ParseExact("6e400003b5a3f393e0a9e50e24dcca9e", "N");
            UART_RX_CHARACTERISTIC = Guid.ParseExact("6e400002b5a3f393e0a9e50e24dcca9e", "N");
        }
    }
}
