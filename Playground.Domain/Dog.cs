namespace Playground.Domain.Entities;

public sealed class Dog : Pet
{
    public override string Species => "dog";

    public Dog(
        string id,
        string nickname,
        int? age,
        string physicalDescription,
        string personalityDescription,
        decimal suggestedDonation)
        : base(id, nickname, age, physicalDescription, personalityDescription, suggestedDonation)
    {
    }
}