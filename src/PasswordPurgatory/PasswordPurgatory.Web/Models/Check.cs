using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordPurgatory.Web.Models;

public class Check
{
    public string Username { get; set; }

    public string Password { get; set; }

    public string Message { get; set; }

    public InfuriationLevel InfuriationLevel { get; set; }

    /// <summary>
    /// Returns true to pass, false to fails
    /// </summary>
    public Func<string, string, bool> Validator { get; set; }

    public bool ValidateCredentials()
    {
        return Validator.Invoke(this.Username, this.Password);
    }

    public static readonly List<Check> Checks = new()
    {
        new()
        {
            Message = "Username cannot just be empty spaces.",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) => !string.IsNullOrWhiteSpace(un) || !string.IsNullOrEmpty(un)
        },
        new()
        {
            Message = "Password must not match the username",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) => un.ToLowerInvariant() != pwd.ToLowerInvariant()
        },
        new()
        {
            Message = "Your username cannot be 'admin', 'user', 'anon/anonymous' or 'null'.",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) =>
            {
                var lowered = un.ToLowerInvariant();

                if(lowered is "admin" or "anonymous" or "anon" or "user" or "null" or "test")
                {
                    return false;
                }
                
                return true;
            }
        },
        new()
        {
            Message = "This is not a test or a joke, your password is too weak. A lot of money is on the line, please create a strong password to protect your account.",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) =>
            {
                var lowered = pwd.ToLowerInvariant();

                if(lowered is "test" or "joke" or "anonymous" or "anon" or "user" or "null")
                {
                    return false;
                }

                return true;
            }
        },
        new()
        {
            Message = "Password must contain at least 1 number",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) => pwd.Any(char.IsDigit)
        },
        new()
        {
            Message = "Password must not end in '!'",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) => pwd[^1..] != "!"
        },
        new()
        {
            Message = "Password must not end in '1'",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (un, pwd) => pwd[^1..] != "1"
        },
        new()
        {
            Message = "Password must contain at least 1 upper case character",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) => pwd.Any(char.IsUpper)
        },
        new()
        {
            Message = "Password must contain at least 1 lower case character",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) => pwd.Any(char.IsLower)
        },
        new()
        {
            Message = "Password must be at least 8 characters long",
            InfuriationLevel = InfuriationLevel.Low,
            Validator = (un, pwd) => pwd.Length >= 8
        },
        new()
        {
            Message = "Password must be less than 40 characters long.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (un, pwd) => pwd.Length < 40
        },
        new()
        {
            Message = "Password must end with 'dog'.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (un, pwd) => pwd[^3..].ToLower() == "dog"
        },
        new()
        {
            Message = "Password must start with 'cat'.",
            InfuriationLevel = InfuriationLevel.Moderate,
            Validator = (un, pwd) => pwd[..3].ToLower() == "cat"
        },
        new()
        {
            Message = "Password must contain at least 3 digits from the first 10 decimal places of pi.",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (un, pwd) =>
            {
                var numbersOnly = pwd.Where(char.IsDigit).ToArray();
                var piNumbers = new [] { "1", "2", "3", "4", "5", "6", "9" };
                var totalMatches = numbersOnly.Where(num => piNumbers.Contains(num.ToString()));
                return totalMatches.Count() >= 3;
            }
        },
        new()
        {
            Message = "Password must contain at least 1 primary Simpsons family character (case sensitive).",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (un, pwd) => "Homer|Marge|Bart|Lisa|Maggie".Split("|").Any(pwd.Contains)
},
        new()
        {
            Message = "Password must contain at least 1 primary Griffin family character (caps sensitive).",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (un, pwd) => "Peter|Lois|Chris|Meg|Brian|Stewie".Split('|').Any(pwd.Contains)
        },
        new()
        {
            Message = @"Password must contain at least one common emoticon (examples: =], :), :p)",
            InfuriationLevel = InfuriationLevel.High,
            Validator = (un, pwd) => ";p|:-p|:P|:‑)|:)|:-]|:]|:>|:-}|:}|:o))|:^)|=]|=)|:]|:->|:>|8-)|:-}|:}|:o)|:^)|=]|=)|:‑D|:D|B^D|:‑(|:(|:‑<|:<|:‑[|:[|:{|:(|;(|:'‑(|:'(|:=(|:'‑)|:')|:‑O|:O|:‑o|:o|:-0|>:O|>:3|;‑)|;)|;‑]|;^)|:‑P|:-/|:/|:‑.|>:|>:/|:|:‑||>:‑)|>:)|}:‑)|>;‑)|>;)|>:3|;‑)|:‑J|<:‑||~:>/".Split('|').Any(pwd.Contains)
        },
        new()
        {
            Message = "Password must be a number divisible by 3 (when stripped of non-numeric characters).",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) =>
            {
                try
                {
                    return new string(pwd.Where(char.IsDigit).ToArray()).Sum(Convert.ToInt32) % 3 == 0;
                }
                catch (Exception)
                {
                    // Just in case there's a problem with my logic, I don't want to ruin the fun... so lets keep moving on.
                    return true;
                }
            }
        },
        new()
        {
            Message = "Password must contain a bobcat (use scientific name for stronger security).",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) => @"bobcat|Lynx rufus|L. rufus".Split('|').Any(pwd.Contains)
        },
        new()
        {
            Message = "Password must contain at least 1 Nordic character (examples: Å, å, Ä, ä, Ö, ö, Æ, æ, Ø, ø)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) => @"Å|å|Ä|ä|Ö|ö|Æ|æ|Ø|ø".Split("|").Any(pwd.Contains)
        },
        new()
        {
            Message = "Password must contain at least one name of a moon in the solar system (case sensitive).",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) => @"Luna|Deimos|Phobos|Amalthea|Callisto|Europa|Ganymede|Io|Dione|Enceladus|Hyperion|Iapetus|Mimas|Phoebe|Rhea|Tethys|Titan|Ariel|Miranda|Oberon|Titania|Umbriel|Nereid|Triton|Charon|Himalia|Carme|Ananke|Adrastea|Elara|Adrastea|Elara|Epimetheus|Callirrhoe|Kalyke|Thebe|Methone|Kiviuq|Ijiraq|Paaliaq|Albiorix|Erriapus|Pallene|Polydeuces|Bestla|Daphnis|Despina|Puck|Carpo|Pasiphae|Themisto|Cyllene|Isonoe|Harpalyke|Hermippe|Iocaste|Chaldene|Euporie".Split('|').Any(pwd.Contains)
        },
        new()
        {
            Message = "Password must contain at least one upper case German Umlaut (examples: Ä, Ü, Ö)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) => Regex.Match(pwd, @"/[ÄÜÖ\u1e9e]/").Success
        },
        //new()
        //{
        //    Message = "Password must contain only unique characters.",
        //    InfuriationLevel = InfuriationLevel.Ridiculous,
        //    Validator = (un, pwd) => !Regex.Match(pwd, @"/(?=^[A-Za-z0-9]+$)(.)+.*\1.*/").Success //regex matches when there's a duplicate character
        //},
        new()
        {
            Message = "Password must contain 'Password must contain' (case sensitive)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) => pwd.Contains("/Password must contain/")
        },
        new()
        {
            Message = "Password must contain at least 1 Greek character (examples: α, θ, π, ψ, Ω)",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) => Regex.Match(pwd, @"/[\u0370-\u03ff\u1f00-\u1fff]/").Success
        },
        new()
        {
            Message = "Password must be a palindrome.",
            InfuriationLevel = InfuriationLevel.Ridiculous,
            Validator = (un, pwd) =>
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
            }
        }
    };
}
