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
            Validator = (pwd) => !new Regex(@"/\d+/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must not end in '!'",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => !new Regex(@"/!$/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least 1 upper case character",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => !new Regex(@"/[A-Z]/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least 1 lower case character",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (pwd) => !new Regex(@"/[a-z]/").IsMatch(pwd)
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
            Validator = (pwd) => !new Regex(@"/1$/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must be less than 24 characters long.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (pwd) => pwd.Length < 24
        },
        new()
        {
            Message = "Password must end with dog.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (pwd) => !new Regex(@"/dog$/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must start with cat.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (pwd) => !new Regex(@"/^cat/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least 3 digits from the first 10 decimal places of pi",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => !new Regex(@"/(?:[^1234569]*[1234569]){3}[^1234569]*/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least 1 primary Simpsons family character (case sensitive).",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => !new Regex(@"/Homer|Marge|Bart|Lisa|Maggie/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least 1 primary Griffin family character (caps sensitive).",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => !new Regex(@"/Peter|Lois|Chris|Meg|Brian|Stewie/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least one emoticon.",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (pwd) => !new Regex(@"/:‑\)|:\)|:\-\]|:\]|:>|:\-\}|:\}|:o\)\)|:\^\)|=\]|=\)|:\]|:\->|:>|8\-\)|:\-\}|:\}|:o\)|:\^\)|=\]|=\)|:‑D|:D|B\^D|:‑\(|:\(|:‑<|:<|:‑\[|:\[|:\-\|\||>:\[|:\{|:\(|;\(|:\'‑\(|:\'\(|:=\(|:\'‑\)|:\'\)|:‑O|:O|:‑o|:o|:\-0|>:O|>:3|;‑\)|;\)|;‑\]|;\^\)|:‑P|:\-\/|:\/|:‑\.|>:|>:\/|:|:‑\||:\||>:‑\)|>:\)|\}:‑\)|>;‑\)|>;\)|>:3|\|;‑\)|:‑J|<:‑\||~:>/,").IsMatch(pwd)
        },
        new()
        {
            Message = "Password when stripped of non-numeric characters must be a number divisible by 3",
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
            Message = "Password must contain a bobcat.",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !new Regex(@"/bobcat|Lynx rufus|L. rufus/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least one upper case German Umlaut",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !new Regex(@"/[ÄÜÖ\u1e9e]/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least one named solarian planetary satellite (case sensitive).",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !new Regex(@"/Luna|Deimos|Phobos|Amalthea|Callisto|Europa|Ganymede|Io|Dione|Enceladus|Hyperion|Iapetus|Mimas|Phoebe|Rhea|Tethys|Titan|Ariel|Miranda|Oberon|Titania|Umbriel|Nereid|Triton|Charon|Himalia|Carme|Ananke|Adrastea|Elara|Adrastea|Elara|Epimetheus|Callirrhoe|Kalyke|Thebe|Methone|Kiviuq|Ijiraq|Paaliaq|Albiorix|Erriapus|Pallene|Polydeuces|Bestla|Daphnis|Despina|Puck|Carpo|Pasiphae|Themisto|Cyllene|Isonoe|Harpalyke|Hermippe|Iocaste|Chaldene|Euporie/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain only unique characters.",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => new Regex(@"/(?=^[A-Za-z0-9]+$)(.)+.*\1.*/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain 'Password must contain'",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !new Regex(@"/Password\smust\scontain/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least 1 Nordic character",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !new Regex(@"/[ÅåÄäÖöÆæØø]/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must contain at least 1 Greek character",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !new Regex(@"/[\u0370-\u03ff\u1f00-\u1fff]/").IsMatch(pwd)
        },
        new()
        {
            Message = "Password must be a palindrome.",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (pwd) => !new Regex(@"/(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])/").IsMatch(pwd)
        }
    };
}