//  Copyright (C) 2009-2016 Christopher Brochtrup
//
//  This file is part of subs2srs.
//
//  subs2srs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  subs2srs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with subs2srs.  If not, see <http://www.gnu.org/licenses/>.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// Represents a language code.
  /// </summary>
  public class LangCode
  {
    string twoLetter;
    string threeLetter;
    string full;

    /// <summary>
    /// The two letter language code. ISO 639-1.
    /// </summary>
    public string TwoLetter 
    {
      get { return twoLetter; } 
    }


    /// <summary>
    /// The three letter language code. ISO 639-3.
    /// </summary>
    public string ThreeLetter
    { 
      get { return threeLetter; } 
    }


    /// <summary>
    /// The full name of the language.
    /// </summary>
    public string Full
    { 
      get { return full; } 
    }

    public LangCode(string twoLetter, string threeLetter, string full)
    {
      this.twoLetter = twoLetter;
      this.threeLetter = threeLetter;
      this.full = full;
    }
  }


  /// <summary>
  /// Utilities related to languages.
  /// </summary>
  public class UtilsLang
  {
    // See: http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
    static private LangCode[] langCodes = 
    { 
      new LangCode("aa", "aar", "Afar"),
      new LangCode("ab", "abk", "Abkhazian"),
      new LangCode("ae", "ave", "Avestan"),
      new LangCode("af", "afr", "Afrikaans"),
      new LangCode("ak", "aka", "Akan"),
      new LangCode("am", "amh", "Amharic"),
      new LangCode("an", "arg", "Aragonese"),
      new LangCode("ar", "ara", "Arabic"),
      new LangCode("as", "asm", "Assamese"),
      new LangCode("av", "ava", "Avaric"),
      new LangCode("ay", "aym", "Aymara"),
      new LangCode("az", "aze", "Azerbaijani"),
      new LangCode("ba", "bak", "Bashkir"),
      new LangCode("be", "bel", "Belarusian"),
      new LangCode("bg", "bul", "Bulgarian"),
      new LangCode("bh", "—  ", "Bihari"),
      new LangCode("bi", "bis", "Bislama"),
      new LangCode("bm", "bam", "Bambara"),
      new LangCode("bn", "ben", "Bengali"),
      new LangCode("bo", "bod", "Tibetan"),
      new LangCode("br", "bre", "Breton"),
      new LangCode("bs", "bos", "Bosnian"),
      new LangCode("ca", "cat", "Catalan, Valencian"),
      new LangCode("ce", "che", "Chechen"),
      new LangCode("ch", "cha", "Chamorro"),
      new LangCode("co", "cos", "Corsican"),
      new LangCode("cr", "cre", "Cree"),
      new LangCode("cs", "ces", "Czech"),
      new LangCode("cu", "chu", "Church Slavic, Old Slavonic, Church Slavonic, Old Bulgarian, Old Church Slavonic"),
      new LangCode("cv", "chv", "Chuvash"),
      new LangCode("cy", "cym", "Welsh"),
      new LangCode("da", "dan", "Danish"),
      new LangCode("de", "deu", "German"),
      new LangCode("dv", "div", "Divehi, Dhivehi, Maldivian"),
      new LangCode("dz", "dzo", "Dzongkha"),
      new LangCode("ee", "ewe", "Ewe"),
      new LangCode("el", "ell", "Modern Greek"),
      new LangCode("en", "eng", "English"),
      new LangCode("eo", "epo", "Esperanto"),
      new LangCode("es", "spa", "Spanish, Castilian"),
      new LangCode("et", "est", "Estonian"),
      new LangCode("eu", "eus", "Basque"),
      new LangCode("fa", "fas", "Persian"),
      new LangCode("ff", "ful", "Fulah"),
      new LangCode("fi", "fin", "Finnish"),
      new LangCode("fj", "fij", "Fijian"),
      new LangCode("fo", "fao", "Faroese"),
      new LangCode("fr", "fra", "French"),
      new LangCode("fy", "fry", "Western Frisian"),
      new LangCode("ga", "gle", "Irish"),
      new LangCode("gd", "gla", "Gaelic, Scottish Gaelic"),
      new LangCode("gl", "glg", "Galician"),
      new LangCode("gn", "grn", "Guaraní"),
      new LangCode("gu", "guj", "Gujarati"),
      new LangCode("gv", "glv", "Manx"),
      new LangCode("ha", "hau", "Hausa"),
      new LangCode("he", "heb", "Modern Hebrew"),
      new LangCode("hi", "hin", "Hindi"),
      new LangCode("ho", "hmo", "Hiri Motu"),
      new LangCode("hr", "hrv", "Croatian"),
      new LangCode("ht", "hat", "Haitian, Haitian Creole"),
      new LangCode("hu", "hun", "Hungarian"),
      new LangCode("hy", "hye", "Armenian"),
      new LangCode("hz", "her", "Herero"),
      new LangCode("ia", "ina", "Interlingua (International Auxiliary Language Association)"),
      new LangCode("id", "ind", "Indonesian"),
      new LangCode("ie", "ile", "Interlingue, Occidental"),
      new LangCode("ig", "ibo", "Igbo"),
      new LangCode("ii", "iii", "Sichuan Yi, Nuosu"),
      new LangCode("ik", "ipk", "Inupiaq"),
      new LangCode("io", "ido", "Ido"),
      new LangCode("is", "isl", "Icelandic"),
      new LangCode("it", "ita", "Italian"),
      new LangCode("iu", "iku", "Inuktitut"),
      new LangCode("ja", "jpn", "Japanese"),
      new LangCode("jv", "jav", "Javanese"),
      new LangCode("ka", "kat", "Georgian"),
      new LangCode("kg", "kon", "Kongo"),
      new LangCode("ki", "kik", "Kikuyu, Gikuyu"),
      new LangCode("kj", "kua", "Kwanyama, Kuanyama"),
      new LangCode("kk", "kaz", "Kazakh"),
      new LangCode("kl", "kal", "Kalaallisut, Greenlandic"),
      new LangCode("km", "khm", "Central Khmer"),
      new LangCode("kn", "kan", "Kannada"),
      new LangCode("ko", "kor", "Korean"),
      new LangCode("kr", "kau", "Kanuri"),
      new LangCode("ks", "kas", "Kashmiri"),
      new LangCode("ku", "kur", "Kurdish"),
      new LangCode("kv", "kom", "Komi"),
      new LangCode("kw", "cor", "Cornish"),
      new LangCode("ky", "kir", "Kirghiz, Kyrgyz"),
      new LangCode("la", "lat", "Latin"),
      new LangCode("lb", "ltz", "Luxembourgish, Letzeburgesch"),
      new LangCode("lg", "lug", "Ganda"),
      new LangCode("li", "lim", "Limburgish, Limburgan, Limburger"),
      new LangCode("ln", "lin", "Lingala"),
      new LangCode("lo", "lao", "Lao"),
      new LangCode("lt", "lit", "Lithuanian"),
      new LangCode("lu", "lub", "Luba-Katanga"),
      new LangCode("lv", "lav", "Latvian"),
      new LangCode("mg", "mlg", "Malagasy"),
      new LangCode("mh", "mah", "Marshallese"),
      new LangCode("mi", "mri", "Ma-ori"),
      new LangCode("mk", "mkd", "Macedonian"),
      new LangCode("ml", "mal", "Malayalam"),
      new LangCode("mn", "mon", "Mongolian"),
      new LangCode("mr", "mar", "Marathi"),
      new LangCode("ms", "msa", "Malay"),
      new LangCode("mt", "mlt", "Maltese"),
      new LangCode("my", "mya", "Burmese"),
      new LangCode("na", "nau", "Nauru"),
      new LangCode("nb", "nob", "Norwegian Bokmål"),
      new LangCode("nd", "nde", "North Ndebele"),
      new LangCode("ne", "nep", "Nepali"),
      new LangCode("ng", "ndo", "Ndonga"),
      new LangCode("nl", "nld", "Dutch, Flemish"),
      new LangCode("nn", "nno", "Norwegian Nynorsk"),
      new LangCode("no", "nor", "Norwegian"),
      new LangCode("nr", "nbl", "South Ndebele"),
      new LangCode("nv", "nav", "Navajo, Navaho"),
      new LangCode("ny", "nya", "Chichewa, Chewa, Nyanja"),
      new LangCode("oc", "oci", "Occitan (after 1500)"),
      new LangCode("oj", "oji", "Ojibwa"),
      new LangCode("om", "orm", "Oromo"),
      new LangCode("or", "ori", "Oriya"),
      new LangCode("os", "oss", "Ossetian, Ossetic"),
      new LangCode("pa", "pan", "Panjabi, Punjabi"),
      new LangCode("pi", "pli", "Pa-li"),
      new LangCode("pl", "pol", "Polish"),
      new LangCode("ps", "pus", "Pashto, Pushto"),
      new LangCode("pt", "por", "Portuguese"),
      new LangCode("qu", "que", "Quechua"),
      new LangCode("rm", "roh", "Romansh"),
      new LangCode("rn", "run", "Rundi"),
      new LangCode("ro", "ron", "Romanian, Moldavian, Moldovan"),
      new LangCode("ru", "rus", "Russian"),
      new LangCode("rw", "kin", "Kinyarwanda"),
      new LangCode("sa", "san", "Sanskrit"),
      new LangCode("sc", "srd", "Sardinian"),
      new LangCode("sd", "snd", "Sindhi"),
      new LangCode("se", "sme", "Northern Sami"),
      new LangCode("sg", "sag", "Sango"),
      new LangCode("si", "sin", "Sinhala, Sinhalese"),
      new LangCode("sk", "slk", "Slovak"),
      new LangCode("sl", "slv", "Slovene"),
      new LangCode("sm", "smo", "Samoan"),
      new LangCode("sn", "sna", "Shona"),
      new LangCode("so", "som", "Somali"),
      new LangCode("sq", "sqi", "Albanian"),
      new LangCode("sr", "srp", "Serbian"),
      new LangCode("ss", "ssw", "Swati"),
      new LangCode("st", "sot", "Southern Sotho"),
      new LangCode("su", "sun", "Sundanese"),
      new LangCode("sv", "swe", "Swedish"),
      new LangCode("sw", "swa", "Swahili"),
      new LangCode("ta", "tam", "Tamil"),
      new LangCode("te", "tel", "Telugu"),
      new LangCode("tg", "tgk", "Tajik"),
      new LangCode("th", "tha", "Thai"),
      new LangCode("ti", "tir", "Tigrinya"),
      new LangCode("tk", "tuk", "Turkmen"),
      new LangCode("tl", "tgl", "Tagalog"),
      new LangCode("tn", "tsn", "Tswana"),
      new LangCode("to", "ton", "Tonga (Tonga Islands)"),
      new LangCode("tr", "tur", "Turkish"),
      new LangCode("ts", "tso", "Tsonga"),
      new LangCode("tt", "tat", "Tatar"),
      new LangCode("tw", "twi", "Twi"),
      new LangCode("ty", "tah", "Tahitian"),
      new LangCode("ug", "uig", "Uighur, Uyghur"),
      new LangCode("uk", "ukr", "Ukrainian"),
      new LangCode("ur", "urd", "Urdu"),
      new LangCode("uz", "uzb", "Uzbek"),
      new LangCode("ve", "ven", "Venda"),
      new LangCode("vi", "vie", "Vietnamese"),
      new LangCode("vo", "vol", "Volapük"),
      new LangCode("wa", "wln", "Walloon"),
      new LangCode("wo", "wol", "Wolof"),
      new LangCode("xh", "xho", "Xhosa"),
      new LangCode("yi", "yid", "Yiddish"),
      new LangCode("yo", "yor", "Yoruba"),
      new LangCode("za", "zha", "Zhuang, Chuang"),
      new LangCode("zh", "zho", "Chinese"),
      new LangCode("zu", "zul", "Zulu")
    };


    /// <summary>
    /// Convert a two letter language code to it's full name.
    /// </summary>
    public static string LangTwoLetter2Full(string lang)
    {
      string ret = "";
      lang = lang.ToLower();

      for (int i = 0; i < langCodes.Length; i++)
      {
        if (langCodes[i].TwoLetter == lang)
        {
          ret = langCodes[i].Full;
          break;
        }
      }

      return ret;
    }


    /// <summary>
    /// Convert a three letter language code to it's full name.
    /// </summary>
    public static string LangThreeLetter2Full(string lang)
    {
      string ret = "";
      lang = lang.ToLower();

      for (int i = 0; i < langCodes.Length; i++)
      {
        if (langCodes[i].ThreeLetter == lang)
        {
          ret = langCodes[i].Full;
          break;
        }
      }

      return ret;
    }

  
    /// <summary>
    /// Does provided string contain any hiragana?
    /// </summary>
    public static bool containsHiragana(string str)
    {
      return Regex.IsMatch(str, @"^.*\p{IsHiragana}+.*$");
    }
    

    /// <summary>
    /// Does provided string contain any katakana?
    /// </summary>
    public static bool containsKatakana(string str)
    {
      return Regex.IsMatch(str, @"^.*\p{IsKatakana}+.*$");
    }


    /// <summary>
    /// Does this string contain an ideagraph (like a kanji)?
    /// </summary>
    public static bool containsIdeograph(string str)
    {
      return Regex.IsMatch(str, @"^.*\p{IsCJKUnifiedIdeographs}+.*$");
    }


    /// <summary>
    /// Does this string contain any non-ASCII unicode characters?
    /// </summary>
    public static bool containsUnicode(string str)
    {
      // Note: 0x7F = 127, so anything ASCII is OK
      return Regex.IsMatch(str, @"^.*[\u007F-\uFFFF-[\s\p{P}\p{IsGreek}\x85]]+.*$");
    }


    /// <summary>
    /// Convert a double to US format.
    /// </summary>
    public static double toDouble(string str)
    {
     // NumberFormatInfo numformat = new NumberFormatInfo();
     // numformat.NumberDecimalSeparator = ".";
     // return Convert.ToDouble(str, numformat);

      return Double.Parse(str.Replace(",", ".").Trim(), new CultureInfo("")); // Invariant culture
    }



  }
}
