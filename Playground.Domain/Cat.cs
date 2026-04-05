namespace Playground.Domain.Entities;

public sealed class Cat : Pet
{
    public override string Species => "cat";

    public Cat(
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