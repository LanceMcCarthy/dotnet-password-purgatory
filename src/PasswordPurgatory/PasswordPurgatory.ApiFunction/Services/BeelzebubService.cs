using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PasswordPurgatory.ApiFunction.Models;

namespace PasswordPurgatory.ApiFunction.Services;

internal class BeelzebubService
{
    private static string pwd;

    public Check Infuriate(string password)
    {
        foreach (var check in checks)
        {
            if (check.PasswordIsValid)
                continue;

            return check;
        }

        return null;
    }

    private readonly List<Check> checks = new()
    {
        new Check
        {
            PasswordIsValid = !new Regex(@"/\d+/").IsMatch(pwd),
            Message = "Password must contain at least 1 number",
            InfuriationLevel = InfuriationLevel.Low
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/!$/").IsMatch(pwd),
            Message = "Password must not end in '!'",
            InfuriationLevel = InfuriationLevel.Low
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/[A-Z]/").IsMatch(pwd),
            Message = "Password must contain at least 1 lowercase character",
            InfuriationLevel = InfuriationLevel.Low
        },
        new Check
        {
            PasswordIsValid = pwd.Length == 0,
            Message = "Password cannot be empty",
            InfuriationLevel = InfuriationLevel.Low
        },
        new Check
        {
            PasswordIsValid = pwd.Length < 8,
            Message = "Password must be at least 8 characters long",
            InfuriationLevel = InfuriationLevel.Low
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/1$/").IsMatch(pwd),
            Message = "Password must not end in '1'",
            InfuriationLevel = InfuriationLevel.Moderate
        },
        new Check
        {
            PasswordIsValid = pwd.Length > 20,
            Message = $"Password must not be {pwd.Length} characters long'",
            InfuriationLevel = InfuriationLevel.Moderate
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/dog$/").IsMatch(pwd),
            Message = "Password must end with dog",
            InfuriationLevel = InfuriationLevel.Moderate
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/^cat/").IsMatch(pwd),
            Message = "Password must start with cat",
            InfuriationLevel = InfuriationLevel.Moderate
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/Homer|Marge|Bart|Lisa|Maggie/").IsMatch(pwd),
            Message = "Password must contain at least 1 primary Simpsons family character",
            InfuriationLevel = InfuriationLevel.High
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/Peter|Lois|Chris|Meg|Brian|Stewie/").IsMatch(pwd),
            Message = "Password must contain at least 1 primary Griffin family character",
            InfuriationLevel = InfuriationLevel.High
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/:‑\)|:\)|:\-\]|:\]|:>|:\-\}|:\}|:o\)\)|:\^\)|=\]|=\)|:\]|:\->|:>|8\-\)|:\-\}|:\}|:o\)|:\^\)|=\]|=\)|:‑D|:D|B\^D|:‑\(|:\(|:‑<|:<|:‑\[|:\[|:\-\|\||>:\[|:\{|:\(|;\(|:\'‑\(|:\'\(|:=\(|:\'‑\)|:\'\)|:‑O|:O|:‑o|:o|:\-0|>:O|>:3|;‑\)|;\)|;‑\]|;\^\)|:‑P|:\-\/|:\/|:‑\.|>:|>:\/|:|:‑\||:\||>:‑\)|>:\)|\}:‑\)|>;‑\)|>;\)|>:3|\|;‑\)|:‑J|<:‑\||~:>/,").IsMatch(pwd),
            Message = "Password must contain at least one emoticon",
            InfuriationLevel = InfuriationLevel.High
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/bobcat|Lynx rufus|L. rufus/").IsMatch(pwd),
            Message = "Password must contain a bobcat",
            InfuriationLevel = InfuriationLevel.High
        },
        new Check
        {
            PasswordIsValid = !IsDivisibleByThree(),
            Message = "Password when stripped of non-numeric characters must be a number divisible by 3",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/[ÄÜÖ\u1e9e]/").IsMatch(pwd),
            Message = "Password must contain at least one upper case German Umlaut",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/Luna|Deimos|Phobos|Amalthea|Callisto|Europa|Ganymede|Io|Dione|Enceladus|Hyperion|Iapetus|Mimas|Phoebe|Rhea|Tethys|Titan|Ariel|Miranda|Oberon|Titania|Umbriel|Nereid|Triton|Charon|Himalia|Carme|Ananke|Adrastea|Elara|Adrastea|Elara|Epimetheus|Callirrhoe|Kalyke|Thebe|Methone|Kiviuq|Ijiraq|Paaliaq|Albiorix|Erriapus|Pallene|Polydeuces|Bestla|Daphnis|Despina|Puck|Carpo|Pasiphae|Themisto|Cyllene|Isonoe|Harpalyke|Hermippe|Iocaste|Chaldene|Euporie/").IsMatch(pwd),
            Message = "Password must contain at least one named solarian planetary satellite",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/(?:[^1234569]*[1234569]){3}[^1234569]*/").IsMatch(pwd),
            Message = "Password must contain at least 3 digits from the first 10 decimal places of pi",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/(?=^[A-Za-z0-9]+$)(.)+.*\1.*/").IsMatch(pwd),
            Message = "Password must contain only unique characters.",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/Password\smust\scontain/").IsMatch(pwd),
            Message = "Password must contain 'Password must contain'",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/[ÅåÄäÖöÆæØø]/").IsMatch(pwd),
            Message = "Password must contain at least 1 Nordic character",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/[\u0370-\u03ff\u1f00-\u1fff]/").IsMatch(pwd),
            Message = "Password must contain at least 1 Greek character",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
        new Check
        {
            PasswordIsValid = !new Regex(@"/(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])/").IsMatch(pwd),
            Message = "Password must be a palindrome.",
            InfuriationLevel = InfuriationLevel.Ridiculous
        },
    };

    private static bool IsDivisibleByThree()
    {
        int total = 0;

        foreach (Match match in Regex.Matches(pwd, "/[0-9]/g"))
        {
            string result = match.Result("");

            total += Convert.ToInt32(result);


            Console.WriteLine(result);
        }

        return total % 3 == 0;
    }
}