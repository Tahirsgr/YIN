namespace YIN_Portal._keenthemes.libs;

class KTThemeSettings
{
    public static KTThemeBase config;

    public static void init(IConfiguration configuration)
    {
        config = configuration.GetSection("Theme").Get<KTThemeBase>();
    }
}
