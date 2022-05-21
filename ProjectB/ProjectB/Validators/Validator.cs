namespace ProjectB.Validators
{
    public class Validator : IValidator
    {
        public bool Validate(string message)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            return true;
        }

        public bool ValidateDestination(int destinationId)
        {
            if (destinationId != int.MinValue)
            {
                return true;
            }

            return false;
        }

        public bool ValidateResponse(string result)
        {
            if (result != "ERROR")
            {
                return true;
            }

            return false;
        }
    }
}
