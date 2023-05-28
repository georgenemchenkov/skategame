using UnityEngine;

[System.Serializable]
public struct PlayerAbilities {
    [Header("Навык, отвечающий за доступность исполнения определённого трюка")]
    public Ability Strength;
    [Header("Навык, отвечающий за шанс успешного выполнения трюка")]
    public Ability Agility;
//    [Header("Является бонусной составляющей при выполнении трюка (бонус к ловкости) для получения дополнительных очков необходимых для завершения турнира")]
//    public Ability Improvisation;
    [Header("Характеристика, значение которого увеличивается по итогам прохождения турнира и отвечает за доступ к последующим заданиям и получению внутриигровых предметов.")]
    public Ability Reputation;

    /*
     * Очистка модификаторов хар-к персонажа (Чтобы не хранить лишнюю информацию в сохранениях) 
     */
    public void ClearModifiers()
    {
        Strength.ClearModifiers();
        Agility.ClearModifiers();
        //Improvisation.ClearModifiers();
        Reputation.ClearModifiers();
    }

    public void ApplyModifiers(PlayerAbilities modifiers)
    {
        Strength.AddModifier(modifiers.Strength.baseValue);
        Agility.AddModifier(modifiers.Agility.baseValue);
        Reputation.AddModifier(modifiers.Reputation.baseValue);
    }

    public void RemoveModifiers(PlayerAbilities modifiers)
    {
        Strength.RemoveModifier(modifiers.Strength.baseValue);
        Agility.RemoveModifier(modifiers.Agility.baseValue);
        Reputation.RemoveModifier(modifiers.Reputation.baseValue);
    }

    /**
     * Хватает ли хар-к
     */
    public bool IsSufficient(PlayerAbilities compareWith)
    {
        bool hasStrength = Strength.GetValue() >= compareWith.Strength.baseValue;
        bool hasAgility = Agility.GetValue() >= compareWith.Agility.baseValue;
        //bool hasImprovisation = Improvisation.GetValue() >= compareWith.Improvisation.baseValue;
        bool hasReputation = Reputation.GetValue() >= compareWith.Reputation.baseValue;

        return hasStrength && hasAgility /*&& hasImprovisation*/ && hasReputation;
    }

    public void UpdateBaseValues(PlayerAbilities playerAbilities)
    {
        if (playerAbilities.Strength != null) Strength.baseValue += playerAbilities.Strength.baseValue;
        if (playerAbilities.Agility != null) Agility.baseValue += playerAbilities.Agility.baseValue;
        //if (playerAbilities.Improvisation != null) Improvisation.baseValue += playerAbilities.Improvisation.baseValue;
        if (playerAbilities.Reputation != null) Reputation.baseValue += playerAbilities.Reputation.baseValue;
    }
}