using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordPurgatory.ApiFunction.Models;

public class Check
{
    public string Password { get; set; }

    public string Message { get; set; }

    public InfuriationLevel InfuriationLevel { get; set; }

    public Func<string, bool> Validator { get; set; }

    public bool ValidatePassword()
    {
        return Validator.Invoke(Password);
    }

    public static readonly List<Check> Checks = new()
    {
        new()
        {
            Message = "Password must contain at least 1 number",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => !Regex.Match(pwd, @"/\d+/").Success
        },
        new()
        {
            Message = "Password must not end in '!'",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => !Regex.Match(pwd, @"/!$/").Success
        },
        new()
        {
            Message = "Password must contain at least 1 upper case character",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => Regex.Match(pwd, @"/[A-Z]/").Success
        },
        new()
        {
            Message = "Password must contain at least 1 lower case character",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => Regex.Match(pwd, @"/[a-z]/").Success
        },
        new()
        {
            Message = "Password must be at least 8 characters long",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => pwd.Length >= 8
        },
        new()
        {
            Message = "Password must not end in '1'",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (pwd) => !Regex.Match(pwd, @"/1$/").Success
        },
        new()
        {
            Message = "Password must be less than 40 characters long.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (pwd) => pwd.Length < 40
        },
        new()
        {
            Message = "Password must end with 'dog'.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (pwd) => !Regex.Match(pwd, @"/dog$/").Success
        },
        new()
        {
            Message = "Password must start with 'cat'.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (pwd) => !Regex.Match(pwd, @"/^cat/").Success
        },
        new()
        {
            Message = "Password must contain at least 3 digits from the first 10 decimal places of pi.",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => Regex.Match(pwd, @"/(?:[^1234569]*[1234569]){3}[^1234569]*/").Success
        },
        new()
        {
            Message = "Password must contain at least 1 primary Simpsons family character (case sensitive).",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => Regex.Match(pwd, @"/Homer|Marge|Bart|Lisa|Maggie/").Success
        },
        new()
        {
            Message = "Password must contain at least 1 primary Griffin family character (caps sensitive).",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => Regex.Match(pwd, @"/Peter|Lois|Chris|Meg|Brian|Stewie/").Success
        },
        new()
        {
            Message = @"Password must contain at least one emoticon (examples: =], :), :p)",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => Regex.Match(pwd, @"/:‑\)|:\)|:\-\]|:\]|:>|:\-\}|:\}|:o\)\)|:\^\)|=\]|=\)|:\]|:\->|:>|8\-\)|:\-\}|:\}|:o\)|:\^\)|=\]|=\)|:‑D|:D|B\^D|:‑\(|:\(|:‑<|:<|:‑\[|:\[|:\-\|\||>:\[|:\{|:\(|;\(|:\'‑\(|:\'\(|:=\(|:\'‑\)|:\'\)|:‑O|:O|:‑o|:o|:\-0|>:O|>:3|;‑\)|;\)|;‑\]|;\^\)|:‑P|:\-\/|:\/|:‑\.|>:|>:\/|:|:‑\||:\||>:‑\)|>:\)|\}:‑\)|>;‑\)|>;\)|>:3|\|;‑\)|:‑J|<:‑\||~:>/,").Success
        },
        new()
        {
            Message = "Password must be a number divisible by 3 (when stripped of non-numeric characters).",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) =>
            {
                try
                {
                    var numbersOnly = new string(pwd.Where(char.IsDigit).ToArray());

                    return numbersOnly.Sum(Convert.ToInt32) % 3 == 0;
                }
                catch (Exception)
                {
                    // Just in case there's a problem with my logic/math, I don't want to ruin the fun... so lets keep moving on.
                    return true;
                }
            }
        },
        new()
        {
            Message = "Password must contain a bobcat (use scientific name for stronger security).",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => Regex.Match(pwd, @"/bobcat|Lynx rufus|L. rufus/").Success
        },
        new()
        {
            Message = "Password must contain at least one name of a moon in the solar system (case sensitive).",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => Regex.Match(pwd, @"/Luna|Deimos|Phobos|Amalthea|Callisto|Europa|Ganymede|Io|Dione|Enceladus|Hyperion|Iapetus|Mimas|Phoebe|Rhea|Tethys|Titan|Ariel|Miranda|Oberon|Titania|Umbriel|Nereid|Triton|Charon|Himalia|Carme|Ananke|Adrastea|Elara|Adrastea|Elara|Epimetheus|Callirrhoe|Kalyke|Thebe|Methone|Kiviuq|Ijiraq|Paaliaq|Albiorix|Erriapus|Pallene|Polydeuces|Bestla|Daphnis|Despina|Puck|Carpo|Pasiphae|Themisto|Cyllene|Isonoe|Harpalyke|Hermippe|Iocaste|Chaldene|Euporie/").Success
        },
        new()
        {
            Message = "Password must contain at least one upper case German Umlaut (examples: Ä, Ü, Ö)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => Regex.Match(pwd, @"/[ÄÜÖ\u1e9e]/").Success
        },
        new()
        {
            Message = "Password must contain only unique characters.",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !Regex.Match(pwd, @"/(?=^[A-Za-z0-9]+$)(.)+.*\1.*/").Success //regex matches when there's a duplicate character
        },
        new()
        {
            Message = "Password must contain 'Password must contain' (case sensitive)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => Regex.Match(pwd, @"/Password\smust\scontain/").Success
        },
        new()
        {
            Message = "Password must contain at least 1 Nordic character (examples: Å, å, Ä, ä, Ö, ö, Æ, æ, Ø, ø)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => Regex.Match(pwd, @"/[ÅåÄäÖöÆæØø]/").Success
        },
        new()
        {
            Message = "Password must contain at least 1 Greek character (examples: α, θ, π, ψ, Ω)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => Regex.Match(pwd, @"/[\u0370-\u03ff\u1f00-\u1fff]/").Success
        },
        new()
        {
            Message = "Password must be a palindrome.",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) =>
            {
                // Credit: https://codereview.stackexchange.com/a/252128

                pwd = Regex.Replace(pwd, @"(?i)[^a-z0-9]+", "");

                if (pwd == "" || pwd.Length == 1 || pwd == " ") {
                    return true;
                }

                for (int i = 0, j = pwd.Length - 1; i <= pwd.Length / 2  && j >= pwd.Length / 2; i++, j--)
                {
                    if (char.ToLower(pwd[i]) == char.ToLower(pwd[j])) {
                        continue;
                    }

                    return false;
                }

                return true;

                // Doesn't seem to match, for example 'madam'
                //return !Regex.Match(pwd, @"/(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])/").Success;
            }
        }
    };
}