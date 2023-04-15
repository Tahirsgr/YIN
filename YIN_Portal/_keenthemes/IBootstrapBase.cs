using YIN_Portal._keenthemes.libs;

namespace YIN_Portal._keenthemes;

public interface IBootstrapBase
{
    void initThemeMode();
    
    void initThemeDirection();
    
    void initRtl();

    void initLayout();

    void init(IKTTheme theme);
}