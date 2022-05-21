namespace ProjectB.Validators
{
    public interface IValidator
    {
        bool Validate(string message);

        bool ValidateDestination(int destinationId);

        bool ValidateResponse(string result);
    }
}
