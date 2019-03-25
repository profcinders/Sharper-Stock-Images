using System;

namespace Sharper.StockImages.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DisableServiceAttribute : Attribute
    {
        public string DisabledId { get; }

        public Type DisabledType { get; }

        public DisableServiceAttribute(string disabledId)
        {
            DisabledId = disabledId;
        }

        public DisableServiceAttribute(Type disabledType)
        {
            DisabledType = disabledType;
        }
    }
}