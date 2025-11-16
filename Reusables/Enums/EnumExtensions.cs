namespace Reusables.Enums;

public static class EnumExtensions
{
    public static string ToModelString(this OpenAIModel model)
    {
        return model switch
        {
            OpenAIModel.GPT5_Nano => "gpt-5-nano",
            OpenAIModel.GPT5_Mini => "gpt-5-mini",
            OpenAIModel.GPT4_1Mini => "gpt-4.1-mini",
            OpenAIModel.GPT4o => "gpt-4o",
            OpenAIModel.GPT4_1 => "gpt-4.1",
            _ => throw new ArgumentOutOfRangeException(nameof(model))
        };
    }
}
