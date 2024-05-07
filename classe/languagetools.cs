using System;
using classe;

using static classe.JsonTemplate;

public class LanguageTools
{
    public static string lang_str { get; set; }
    public static bool temp_lang_cc { get; set; }
    public static bool sauvegardetype { get; set; }

    public static string SelectLanguage(string default_lang)
    {
        while (true)
        {
            Console.WriteLine(JsonTemplate.GetJson(default_lang, "defaultlang"));

            string inputLanguage = Console.ReadLine().ToUpper();

            if (inputLanguage == "FR" || inputLanguage == "ENG" || inputLanguage == "DE")
            {
                Console.WriteLine(JsonTemplate.GetJson(inputLanguage, "setlang"));
                lang_str = inputLanguage;
                return inputLanguage;
            }
            else
            {
                Console.WriteLine(JsonTemplate.GetJson(default_lang, "error_setlang"));
            }
        }
    }
    public static int GetNumberOfFolders()
    {
        int nbr_save;
        while (true)
        {
            Console.WriteLine(JsonTemplate.GetJson(lang_str, "get_number"));

            if (int.TryParse(Console.ReadLine(), out nbr_save) && nbr_save > 0)
            {
                break;
            }
            else
            {
                Console.WriteLine(JsonTemplate.GetJson(lang_str, "invalid_enter"));
            }
        }
        return nbr_save;
    }
    public static void ChooseCutCopy(LanguageBase lang)
    {
        while (true)
        {
            Console.WriteLine(JsonTemplate.GetJson(lang_str, "choicecutcopy"));

            string inputstart = Console.ReadLine();
            inputstart = inputstart.ToUpper();

            if (inputstart == "COPY" || inputstart == "COPIER" || inputstart == "KOPIEREN" || inputstart == "KOPIE")
            {
                temp_lang_cc = false;
                Console.WriteLine(JsonTemplate.GetJson(lang_str, "validationchoicecopy"));

            }
            else if (inputstart == "CUT" || inputstart == "COUPER" || inputstart == "SCHNEIDEN")
            {
                temp_lang_cc = true;
                Console.WriteLine(JsonTemplate.GetJson(lang_str, "validationchoicecut"));

            }
            else
            {
                Console.WriteLine(JsonTemplate.GetJson(lang_str, "invalid_input"));
                continue; // Retourne au début de la boucle
            }
            break;
        }
    }
    // type off save
    public static void t_save()
    {
        while (true)
        {
            Console.WriteLine(JsonTemplate.GetJson(lang_str, "savetype"));

            string save_type = Console.ReadLine();
            save_type = save_type.ToUpper();

            if (save_type == "COMPLETE" || save_type == "COMP" || save_type == "C" || save_type == "VOLLSTÄNDIGES" || save_type == "V")
            {
                sauvegardetype = false;
            }
            else if (save_type == "DIFFERENTIEL" || save_type == "DIF" || save_type == "D" || save_type == "DIFFERENTIAL" || save_type == "DIFFERENTIELLES")
            {
                sauvegardetype = true;
            }
            else
            {
                continue; // Retourne au début de la boucle
            }
            break;
        }
    }
}