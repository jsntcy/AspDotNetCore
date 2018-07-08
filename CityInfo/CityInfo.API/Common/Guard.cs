namespace CityInfo.API.Common
{
    using System;

    public static class Guard
    {
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotNullOrEmpty(string argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentException($"{argumentName} is not allowed to be empty.");
            }
        }

        public static void Argument(Func<bool> validationFunction, string errorMessage)
        {
            if (validationFunction == null)
            {
                return;
            }

            if (!validationFunction())
            {
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
