// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.CheckCall.Domain.Models
{
    using System;
    using System.Text.RegularExpressions;

    public class PhoneNumberModel
    {
        private string _CountryCode;

        public string CountryCode
        {
            get
            {
                return _CountryCode;
            }
            set
            {
                if (String.IsNullOrEmpty(CountryCode))
                {
                    _CountryCode = "1";
                }
                else
                {
                    _CountryCode = Regex.Replace(value, "[^0-9]", "");
                }
            }
        }

        private string _Extension;

        public string Extension
        {
            get
            {
                return _Extension;
            }
            set
            {
                _Extension = Regex.Replace(value, "[^0-9]", "");
            }
        }

        private string _Number;

        public string Number
        {
            get
            {
                return _Number;
            }
            set
            {
                _Number = Regex.Replace(value, "[^0-9]", "");
            }

        }

        private string _Formatted;

        public string Formatted
        {
            get
            {
                return _Formatted;
            }
            set
            {
                _Formatted = value;
            }
        }

        public static implicit operator PhoneNumberModel(string value)
        {
            if (null == value)
            {
                return new PhoneNumberModel();
            }

            string[] number = value.Split('|');
            if (number.Length != 5)
            {
                return new PhoneNumberModel();
            }
            return new PhoneNumberModel()
            {
                CountryCode = number[1],
                Number = number[2],
                Extension = number[3],
                Formatted = PhoneNumberFormatter.ParsePhone(value)
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public static bool operator ==(PhoneNumberModel phoneOne, PhoneNumberModel phoneTwo)
        {
            return phoneOne.CountryCode.Equals(phoneTwo.CountryCode) &&
                phoneOne.Number.Equals(phoneTwo.Number) &&
                phoneOne.Extension.Equals(phoneTwo.Extension);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        public static bool operator !=(PhoneNumberModel phoneOne, PhoneNumberModel phoneTwo)
        {
            if (!(phoneOne.CountryCode.Equals(phoneTwo.CountryCode)))
            {
                return true;
            }
            if (!(phoneOne.Number.Equals(phoneTwo.Number)))
            {
                return true;
            }
            if (!(phoneOne.Extension.Equals(phoneTwo.Extension)))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return string.Format("|{0}|{1}|{2}|", this.CountryCode, this.Number, this.Extension);
        }

        public bool IsValid()
        {
            if (null == this.CountryCode || null == this.Number)
            {
                return false;
            }
            string countryCode = Regex.Replace(this.CountryCode, "[^0-9]", "");
            string number = Regex.Replace(this.Number, "[^0-9]", "");

            if (String.IsNullOrWhiteSpace(countryCode) || String.IsNullOrWhiteSpace(number))
            {
                return false;
            }

            return true;
        }
    }

    internal static class PhoneNumberFormatter
    {
        public static string ParsePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return string.Empty;
            }
            else
            {
                string[] arrays = phone.Split('|');
                if (arrays.Length == 5)
                {
                    string countryCode = arrays[1];
                    string wholeNumber = arrays[2];
                    string extension = arrays[3];
                    if (wholeNumber.Length == 0)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        string formattedNumber = "";
                        // being America
                        if (countryCode == "1" && wholeNumber.Length >= 10)
                        {
                            string areaCode = GetAmericanAreaCode(wholeNumber);
                            string number = GetAmericanPhoneNumber(wholeNumber);

                            formattedNumber = "+" + countryCode + " (" + areaCode + ") " + number;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(countryCode))
                            {
                                formattedNumber = "+" + countryCode + " " + wholeNumber;
                            }
                            else
                            {
                                formattedNumber = wholeNumber;
                            }
                        }
                        if (!string.IsNullOrEmpty(extension))
                        {
                            extension = " x" + extension;
                        }
                        return formattedNumber + extension;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private static string Left(string value, int length)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length >= length)
                {
                    return value.Substring(0, length);
                }
            }
            return value;
        }

        private static string Right(string value, int length)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length >= length)
                {
                    return value.Substring(value.Length - length, length);
                }
            }
            return value;
        }

        private static string GetAmericanAreaCode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Left(value, 3);
            }
            return value;
        }

        private static string GetAmericanPhoneNumber(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string sNumber = Right(value, 7);
                return Left(sNumber, 3) + " " + Right(sNumber, 4);
            }
            return value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static string ConstructPhone(string phone)
        {
            if (!string.IsNullOrWhiteSpace(phone))
            {
                string[] phoneArray = Regex.Split(Regex.Replace(phone.Trim(), @"[^\s0-9x().-]+", ""), @"[\s-()x.]+");
                int phoneIndex = 0;

                if (string.IsNullOrWhiteSpace(phoneArray[0]))
                {
                    phone = "||";
                    phoneIndex++;
                }
                else if (phoneArray[0][0] == '+' || phoneArray[0][0] == '1')
                {
                    phone = "|" + phoneArray[phoneIndex].Replace("+", "") + "|";
                    phoneIndex++;
                }
                else
                {
                    phone = "||";
                }

                if (phoneArray.Length >= phoneIndex + 3)
                {
                    phone += phoneArray[phoneIndex++] +
                             phoneArray[phoneIndex++] +
                             phoneArray[phoneIndex++] + "|" +
                             ((phoneArray.Length > phoneIndex) ? phoneArray[phoneIndex] : "") + "|";

                    return phone;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
