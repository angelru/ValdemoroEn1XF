namespace ValdemoroEn1.Validations.Interfaces
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }

        bool Check(T value);
    }
}
