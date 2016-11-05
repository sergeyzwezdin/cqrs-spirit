using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CqrsSpirit.Exceptions
{
    /// <summary>
    /// Base domain exception
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Exception reason
        /// </summary>
        public abstract string ReasonName { get; }
    }

    /// <summary>
    /// Base domain exception
    /// </summary>
    /// <typeparam name="TReason">Reason type</typeparam>
    public class DomainException<TReason> : DomainException
            where TReason : struct, IConvertible
    {
        private readonly TReason _reason;

        /// <summary>
        /// Constructs domain exception with explicit reason
        /// </summary>
        /// <param name="reason"></param>
        public DomainException(TReason reason)
        {
            if (typeof(TReason).GetTypeInfo()
                               .IsEnum == false)
                throw new ArgumentException("The reason's type must be an enum.", nameof(reason));

            _reason = reason;
        }

        /// <summary>
        /// Exception reason
        /// </summary>
        public TReason Reason => _reason;

        /// <summary>
        /// Exception reason
        /// </summary>
        public override string ReasonName => _reason.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Exception message
        /// </summary>
        public override string Message
                =>
                ExtractDecriptionAttribute(_reason, _reason.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Text representation of exception
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (String.IsNullOrWhiteSpace(_reason.ToString(CultureInfo.InvariantCulture)) == false)
                return Reason.ToString(CultureInfo.InvariantCulture) + ": " + Message;
            else
                return Reason.ToString(CultureInfo.InvariantCulture);
        }

        private static string ExtractDecriptionAttribute(TReason reason, string defaultValue)
        {
            MemberInfo member = typeof(TReason).GetTypeInfo()
                                               .GetMember(reason.ToString(CultureInfo.InvariantCulture))
                                               .FirstOrDefault();

            return member?.GetCustomAttribute<DescriptionAttribute>()
                         ?.Description ?? defaultValue;
        }
    }
}