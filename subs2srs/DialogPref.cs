using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace subs2srs
{
  public partial class DialogPref : Form
  {
    private PropertyTable propTable;

    public DialogPref()
    {
      InitializeComponent();

      // Read the setting file
      PrefIO.read();

      propTable = new PropertyTable();

      // main_window_width
      propTable.Properties.Add(new PropertySpec("Main Window Width", typeof(int),
        "User Interface Defaults",
        "The width in pixels of the main interface.\n\n" 
        + "Range: 0-9999.",
        PrefDefaults.MainWindowWidth));
      propTable["Main Window Width"] = ConstantSettings.MainWindowWidth;

      // main_window_height
      propTable.Properties.Add(new PropertySpec("Main Window Height", typeof(int),
        "User Interface Defaults",
        "The height in pixels of the main interface.\n\n"
        + "Range: 0-9999.",
        PrefDefaults.MainWindowHeight));
      propTable["Main Window Height"] = ConstantSettings.MainWindowHeight;

      // default_enable_audio_clip_generation
      propTable.Properties.Add(new PropertySpec("Enable Audio Clip Generation", typeof(bool),
        "User Interface Defaults",
        "Enable the Generate Audio Clips option when subs2srs starts up.",
        PrefDefaults.DefaultEnableAudioClipGeneration));
      propTable["Enable Audio Clip Generation"] = ConstantSettings.DefaultEnableAudioClipGeneration;

      // default_enable_snapshots_generation
      propTable.Properties.Add(new PropertySpec("Enable Snapshots Generation", typeof(bool),
        "User Interface Defaults",
        "Enable the Generate Snapshots option when subs2srs starts up.", 
        PrefDefaults.DefaultEnableSnapshotsGeneration));
      propTable["Enable Snapshots Generation"] = ConstantSettings.DefaultEnableSnapshotsGeneration;

      // default_enable_video_clips_generation
      propTable.Properties.Add(new PropertySpec("Enable Video Clips Generation", typeof(bool),
        "User Interface Defaults",
        "Enable the Generate Video Clips option when subs2srs starts up.",
        PrefDefaults.DefaultEnableVideoClipsGeneration));
      propTable["Enable Video Clips Generation"] = ConstantSettings.DefaultEnableVideoClipsGeneration;

      // default_audio_clip_bitrate
      propTable.Properties.Add(new PropertySpec("Audio Clip Bitrate", typeof(int),
        "User Interface Defaults",
        "The default audio clip bitrate to use when subs2srs starts up.\n\n"
        + "You may use these values: 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 192, 224, 256, 320.", 
        PrefDefaults.DefaultAudioClipBitrate));
      propTable["Audio Clip Bitrate"] = ConstantSettings.DefaultAudioClipBitrate;

      // default_audio_normalize
      propTable.Properties.Add(new PropertySpec("Normalize Audio", typeof(bool),
        "User Interface Defaults",
        "Enable the 'Normalize Audio' option when subs2srs starts up.",
        PrefDefaults.DefaultAudioNormalize));
      propTable["Normalize Audio"] = ConstantSettings.DefaultAudioNormalize;

      // default_video_clip_video_bitrate
      propTable.Properties.Add(new PropertySpec("Video Clip Video Bitrate", typeof(int),
        "User Interface Defaults",
        "The default video clip video bitrate to use when subs2srs starts up.\n\n" 
        + "Range: 100-3000.",
        PrefDefaults.DefaultVideoClipVideoBitrate));
      propTable["Video Clip Video Bitrate"] = ConstantSettings.DefaultVideoClipVideoBitrate;

      // default_video_clip_audio_bitrate
      propTable.Properties.Add(new PropertySpec("Video Clip Audio Bitrate", typeof(int),
        "User Interface Defaults",
        "The default video clip audio bitrate to use when subs2srs starts up.\n\n" 
        + "You may use these values: 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 192, 224, 256, 320.",
        PrefDefaults.DefaultVideoClipAudioBitrate));
      propTable["Video Clip Audio Bitrate"] = ConstantSettings.DefaultVideoClipAudioBitrate;

      // default_ipod_support
      propTable.Properties.Add(new PropertySpec("iPhone Support", typeof(bool),
        "User Interface Defaults", 
        "Enable the iPhone Support option when subs2srs starts up.", 
        PrefDefaults.DefaultIphoneSupport));
      propTable["iPhone Support"] = ConstantSettings.DefaultIphoneSupport;

      // default_encoding_subs1
      propTable.Properties.Add(new PropertySpec("Encoding Subs1", typeof(string),
        "User Interface Defaults",
        "The default text encoding to use for subs1.\n\n" 
        + "You may use these values:\n"
        + "ASMO-708, big5, cp1025, cp866, cp875, csISO2022JP, DOS-720, DOS-862,"
        + "EUC-CN, euc-jp, EUC-JP, euc-kr, GB18030, gb2312, hz-gb-2312, IBM00858,"
        + "IBM00924, IBM01047, IBM01140, IBM01141, IBM01142, IBM01143, IBM01144,"
        + "IBM01145, IBM01146, IBM01147, IBM01148, IBM01149, IBM037, IBM1026, "
        + "IBM273, IBM277, IBM278, IBM280, IBM284, IBM285, IBM290, IBM297, IBM420,"
        + "IBM423, IBM424, IBM437, IBM500, ibm737, ibm775, ibm850, ibm852, IBM855,"
        + "ibm857, IBM860, ibm861, IBM863, IBM864, IBM865, ibm869, IBM870, IBM871,"
        + "IBM880, IBM905, IBM-Thai, iso-2022-jp, iso-2022-jp, iso-2022-kr, iso-8859-1,"
        + "iso-8859-13, iso-8859-15, iso-8859-2, iso-8859-3, iso-8859-4, iso-8859-5, "
        + "iso-8859-6, iso-8859-7, iso-8859-8, iso-8859-8-i, iso-8859-9, Johab, koi8-r,"
        + "koi8-u, ks_c_5601-1987, macintosh, shift_jis, unicodeFFFE, us-ascii, utf-16,"
        + "utf-32, utf-32BE, utf-7, utf-8, windows-1250, windows-1251, Windows-1252,"
        + "windows-1253, windows-1254, windows-1255, windows-1256, windows-1257,"
        + "windows-1258, windows-874, x-Chinese-CNS, x-Chinese-Eten, x-cp20001,"
        + "x-cp20003, x-cp20004, x-cp20005, x-cp20261, x-cp20269, x-cp20936,"
        + "x-cp20949, x-cp50227, x-EBCDIC-KoreanExtended, x-Europa, x-IA5,"
        + "x-IA5-German, x-IA5-Norwegian, x-IA5-Swedish, x-iscii-as,"
        + "x-iscii-be, x-iscii-de, x-iscii-gu, x-iscii-ka, x-iscii-ma, x-iscii-or,"
        + "x-iscii-pa, x-iscii-ta, x-iscii-te, x-mac-arabic, x-mac-ce,"
        + "x-mac-chinesesimp, x-mac-chinesetrad, x-mac-croatian, x-mac-cyrillic,"
        + "x-mac-greek, x-mac-hebrew, x-mac-icelandic, x-mac-japanese, x-mac-korean,"
        + "x-mac-romanian, x-mac-thai, x-mac-turkish, x-mac-ukrainian.",
        PrefDefaults.DefaultEncodingSubs1));
      propTable["Encoding Subs1"] = ConstantSettings.DefaultEncodingSubs1;

      // default_encoding_subs2
      propTable.Properties.Add(new PropertySpec("Encoding Subs2", typeof(string),
        "User Interface Defaults",
        "The default text encoding to use for subs2.\n\n"
        + "You may use these values:\n"
        + "ASMO-708, big5, cp1025, cp866, cp875, csISO2022JP, DOS-720, DOS-862,"
        + "EUC-CN, euc-jp, EUC-JP, euc-kr, GB18030, gb2312, hz-gb-2312, IBM00858,"
        + "IBM00924, IBM01047, IBM01140, IBM01141, IBM01142, IBM01143, IBM01144,"
        + "IBM01145, IBM01146, IBM01147, IBM01148, IBM01149, IBM037, IBM1026, "
        + "IBM273, IBM277, IBM278, IBM280, IBM284, IBM285, IBM290, IBM297, IBM420,"
        + "IBM423, IBM424, IBM437, IBM500, ibm737, ibm775, ibm850, ibm852, IBM855,"
        + "ibm857, IBM860, ibm861, IBM863, IBM864, IBM865, ibm869, IBM870, IBM871,"
        + "IBM880, IBM905, IBM-Thai, iso-2022-jp, iso-2022-jp, iso-2022-kr, iso-8859-1,"
        + "iso-8859-13, iso-8859-15, iso-8859-2, iso-8859-3, iso-8859-4, iso-8859-5, "
        + "iso-8859-6, iso-8859-7, iso-8859-8, iso-8859-8-i, iso-8859-9, Johab, koi8-r,"
        + "koi8-u, ks_c_5601-1987, macintosh, shift_jis, unicodeFFFE, us-ascii, utf-16,"
        + "utf-32, utf-32BE, utf-7, utf-8, windows-1250, windows-1251, Windows-1252,"
        + "windows-1253, windows-1254, windows-1255, windows-1256, windows-1257,"
        + "windows-1258, windows-874, x-Chinese-CNS, x-Chinese-Eten, x-cp20001,"
        + "x-cp20003, x-cp20004, x-cp20005, x-cp20261, x-cp20269, x-cp20936,"
        + "x-cp20949, x-cp50227, x-EBCDIC-KoreanExtended, x-Europa, x-IA5,"
        + "x-IA5-German, x-IA5-Norwegian, x-IA5-Swedish, x-iscii-as,"
        + "x-iscii-be, x-iscii-de, x-iscii-gu, x-iscii-ka, x-iscii-ma, x-iscii-or,"
        + "x-iscii-pa, x-iscii-ta, x-iscii-te, x-mac-arabic, x-mac-ce,"
        + "x-mac-chinesesimp, x-mac-chinesetrad, x-mac-croatian, x-mac-cyrillic,"
        + "x-mac-greek, x-mac-hebrew, x-mac-icelandic, x-mac-japanese, x-mac-korean,"
        + "x-mac-romanian, x-mac-thai, x-mac-turkish, x-mac-ukrainian.",
        PrefDefaults.DefaultEncodingSubs2));
      propTable["Encoding Subs2"] = ConstantSettings.DefaultEncodingSubs2;

      // default_context_num_leading
      propTable.Properties.Add(new PropertySpec("Context Number Leading", typeof(int),
        "User Interface Defaults",
        "The default number of leading context lines to use when subs2srs starts up.\n\n"
        + "Range: 0-9.",
        PrefDefaults.DefaultContextNumLeading));
      propTable["Context Number Leading"] = ConstantSettings.DefaultContextNumLeading;

      // default_context_num_trailing
      propTable.Properties.Add(new PropertySpec("Context Number Trailing", typeof(int),
        "User Interface Defaults",
        "The default number of trailing context lines to use when subs2srs starts up.\n\n"
        + "Range: 0-9.",
        PrefDefaults.DefaultContextNumTrailing));
      propTable["Context Number Trailing"] = ConstantSettings.DefaultContextNumTrailing;

      // default_context_leading_range
      propTable.Properties.Add(new PropertySpec("Context Nearby Line Range Leading", typeof(int),
        "User Interface Defaults",
        "The default leading nearby line range to use when subs2srs starts up.\n\n"
        + "Range: 0-99999. To disable this feature, set to 0.",
        PrefDefaults.DefaultContextLeadingRange));
      propTable["Context Nearby Line Range Leading"] = ConstantSettings.DefaultContextLeadingRange;

      // default_context_trailing_range
      propTable.Properties.Add(new PropertySpec("Context Nearby Line Range Trailing", typeof(int),
        "User Interface Defaults",
        "The default trailing nearby line range to use when subs2srs starts up.\n\n"
        + "Range: 0-99999. To disable this feature, set to 0.",
        PrefDefaults.DefaultContextTrailingRange));
      propTable["Context Nearby Line Range Trailing"] = ConstantSettings.DefaultContextTrailingRange;

      // default_file_browser_start_dir
      propTable.Properties.Add(new PropertySpec("File Browser Start Folder", typeof(string),
        "User Interface Defaults",
        "The directory that the file/directory browser will start in by default.",
        PrefDefaults.DefaultFileBrowserStartDir));
      propTable["File Browser Start Folder"] = ConstantSettings.DefaultFileBrowserStartDir;

      // default_remove_styled_lines_subs1
      propTable.Properties.Add(new PropertySpec("Remove Subs1 Styled Lines", typeof(bool),
        "User Interface Defaults",
        "Remove styled lines when parsing .ass subtitles for Subs1. A styled line "
        + "is one that starts with a '{' character. It is often something unwanted "
        + "such as a karaoke effect.",
        PrefDefaults.DefaultRemoveStyledLinesSubs1));
      propTable["Remove Subs1 Styled Lines"] = ConstantSettings.DefaultRemoveStyledLinesSubs1;

      // default_remove_styled_lines_subs2
      propTable.Properties.Add(new PropertySpec("Remove Subs2 Styled Lines", typeof(bool),
        "User Interface Defaults",
        "Remove styled lines when parsing .ass subtitles for Subs2. A styled line "
        + "is one that starts with a '{' character. It is often something unwanted "
        + "such as a karaoke effect.",
        PrefDefaults.DefaultRemoveStyledLinesSubs1));
      propTable["Remove Subs2 Styled Lines"] = ConstantSettings.DefaultRemoveStyledLinesSubs2;

      // default_remove_no_counterpart_subs1
      propTable.Properties.Add(new PropertySpec("Remove Subs1 Lines With No Obvious Counterpart", typeof(bool),
        "User Interface Defaults",
        "Remove a line from Subs1 if there exists no obvious Subs1 counterpart.",
        PrefDefaults.DefaultRemoveNoCounterpartSubs1));
      propTable["Remove Subs1 Lines With No Obvious Counterpart"] = ConstantSettings.DefaultRemoveNoCounterpartSubs1;

      // default_remove_no_counterpart_subs2
      propTable.Properties.Add(new PropertySpec("Remove Subs2 Lines With No Obvious Counterpart", typeof(bool),
        "User Interface Defaults",
        "Remove a line from Subs2 if there exists no obvious Subs1 counterpart.",
        PrefDefaults.DefaultRemoveNoCounterpartSubs2));
      propTable["Remove Subs2 Lines With No Obvious Counterpart"] = ConstantSettings.DefaultRemoveNoCounterpartSubs2;

      // default_included_text_subs1
      propTable.Properties.Add(new PropertySpec("Included Text Subs1", typeof(string),
        "User Interface Defaults",
        "The list of semicolon-seperated word/phrases to use for the Subs1 'Included Text' option.",
        PrefDefaults.DefaultIncludeTextSubs1));
      propTable["Included Text Subs1"] = ConstantSettings.DefaultIncludeTextSubs1;

      // default_included_text_subs2
      propTable.Properties.Add(new PropertySpec("Included Text Subs2", typeof(string),
        "User Interface Defaults",
        "The list of semicolon-seperated word/phrases to use for the Subs2 'Included Text' option.",
        PrefDefaults.DefaultIncludeTextSubs1));
      propTable["Included Text Subs2"] = ConstantSettings.DefaultIncludeTextSubs2;

      // default_excluded_text_subs1
      propTable.Properties.Add(new PropertySpec("Excluded Text Subs1", typeof(string),
        "User Interface Defaults",
        "The list of semicolon-seperated word/phrases to use for the Subs1 'Excluded Text' option.",
        PrefDefaults.DefaultExcludeTextSubs1));
      propTable["Excluded Text Subs1"] = ConstantSettings.DefaultExcludeTextSubs1;

      // default_excluded_text_subs2
      propTable.Properties.Add(new PropertySpec("Excluded Text Subs2", typeof(string),
        "User Interface Defaults",
        "The list of semicolon-seperated word/phrases to use for the Subs2 'Excluded Text' option.",
        PrefDefaults.DefaultExcludeTextSubs2));
      propTable["Excluded Text Subs2"] = ConstantSettings.DefaultExcludeTextSubs2;

      // default_exclude_duplicate_lines_subs1
      propTable.Properties.Add(new PropertySpec("Exclude Duplicate Lines Subs1", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Duplicate Lines' Subs1 option.",
        PrefDefaults.DefaultExcludeDuplicateLinesSubs1));
      propTable["Exclude Duplicate Lines Subs1"] = ConstantSettings.DefaultExcludeDuplicateLinesSubs1;

      // default_exclude_duplicate_lines_subs2
      propTable.Properties.Add(new PropertySpec("Exclude Duplicate Lines Subs2", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Duplicate Lines' Subs2 option.",
        PrefDefaults.DefaultExcludeDuplicateLinesSubs2));
      propTable["Exclude Duplicate Lines Subs2"] = ConstantSettings.DefaultExcludeDuplicateLinesSubs2;

      // default_exclude_lines_with_fewer_than_n_chars_subs1
      propTable.Properties.Add(new PropertySpec("Exclude Lines With Fewer Than n Characters Enable Subs1", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Lines With Fewer Than n Characters' Subs1 option.",
        PrefDefaults.DefaultExcludeLinesFewerThanCharsSubs1));
      propTable["Exclude Lines With Fewer Than n Characters Enable Subs1"] = ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs1;

      // default_exclude_lines_with_fewer_than_n_chars_subs2
      propTable.Properties.Add(new PropertySpec("Exclude Lines With Fewer Than n Characters Enable Subs2", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Lines With Fewer Than n Characters' Subs2 option.",
        PrefDefaults.DefaultExcludeLinesFewerThanCharsSubs2));
      propTable["Exclude Lines With Fewer Than n Characters Enable Subs2"] = ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs2;

      // default_exclude_lines_with_fewer_than_n_chars_num_subs1
      propTable.Properties.Add(new PropertySpec("Exclude Lines With Fewer Than n Characters Number Subs1", typeof(int),
        "User Interface Defaults",
        "Specify the 'n' in the 'Exclude Lines With Fewer Than n Characters' Subs1 option\n\n"
        + "Range: 2-99999.",
        PrefDefaults.DefaultExcludeLinesFewerThanCharsNumSubs1));
      propTable["Exclude Lines With Fewer Than n Characters Number Subs1"] = ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs1;

      // default_exclude_lines_with_fewer_than_n_chars_num_subs2
      propTable.Properties.Add(new PropertySpec("Exclude Lines With Fewer Than n Characters Number Subs2", typeof(int),
        "User Interface Defaults",
        "Specify the 'n' in the 'Exclude Lines With Fewer Than n Characters' Subs2 option\n\n"
        + "Range: 2-99999.",
        PrefDefaults.DefaultExcludeLinesFewerThanCharsNumSubs2));
      propTable["Exclude Lines With Fewer Than n Characters Number Subs2"] = ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs2;

      // default_exclude_lines_shorter_than_n_ms_subs1
      propTable.Properties.Add(new PropertySpec("Exclude Lines Shorter Than n Milliseconds Enable Subs1", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Lines Shorter Than n Milliseconds' Subs1 option.",
        PrefDefaults.DefaultExcludeLinesShorterThanMsSubs1));
      propTable["Exclude Lines Shorter Than n Milliseconds Enable Subs1"] = ConstantSettings.DefaultExcludeLinesShorterThanMsSubs1;

      // default_exclude_lines_shorter_than_n_ms_subs2
      propTable.Properties.Add(new PropertySpec("Exclude Lines Shorter Than n Milliseconds Enable Subs2", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Lines Shorter Than n Milliseconds' Subs2 option.",
        PrefDefaults.DefaultExcludeLinesShorterThanMsSubs2));
      propTable["Exclude Lines Shorter Than n Milliseconds Enable Subs2"] = ConstantSettings.DefaultExcludeLinesShorterThanMsSubs2;

      // default_exclude_lines_shorter_than_n_ms_num_subs1
      propTable.Properties.Add(new PropertySpec("Exclude Lines Shorter Than n Milliseconds Number Subs1", typeof(int),
        "User Interface Defaults",
        "Specify the 'n' in the 'Exclude Lines Shorter Than n Milliseconds' Subs1 option\n\n"
        + "Range: 100-99999.",
        PrefDefaults.DefaultExcludeLinesShorterThanMsNumSubs1));
      propTable["Exclude Lines Shorter Than n Milliseconds Number Subs1"] = ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs1;

      // default_exclude_lines_shorter_than_n_ms_num_subs2
      propTable.Properties.Add(new PropertySpec("Exclude Lines Shorter Than n Milliseconds Number Subs2", typeof(int),
        "User Interface Defaults",
        "Specify the 'n' in the 'Exclude Lines Shorter Than n Milliseconds' Subs2 option\n\n"
        + "Range: 100-99999.",
        PrefDefaults.DefaultExcludeLinesShorterThanMsNumSubs2));
      propTable["Exclude Lines Shorter Than n Milliseconds Number Subs2"] = ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs2;

      // default_exclude_lines_longer_than_n_ms_subs1
      propTable.Properties.Add(new PropertySpec("Exclude Lines Longer Than n Milliseconds Enable Subs1", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Lines Longer Than n Milliseconds' Subs1 option.",
        PrefDefaults.DefaultExcludeLinesLongerThanMsSubs1));
      propTable["Exclude Lines Longer Than n Milliseconds Enable Subs1"] = ConstantSettings.DefaultExcludeLinesLongerThanMsSubs1;

      // default_exclude_lines_longer_than_n_ms_subs2
      propTable.Properties.Add(new PropertySpec("Exclude Lines Longer Than n Milliseconds Enable Subs2", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Exclude Lines Longer Than n Milliseconds' Subs2 option.",
        PrefDefaults.DefaultExcludeLinesLongerThanMsSubs2));
      propTable["Exclude Lines Longer Than n Milliseconds Enable Subs2"] = ConstantSettings.DefaultExcludeLinesLongerThanMsSubs2;

      // default_exclude_lines_longer_than_n_ms_num_subs1
      propTable.Properties.Add(new PropertySpec("Exclude Lines Longer Than n Milliseconds Number Subs1", typeof(int),
        "User Interface Defaults",
        "Specify the 'n' in the 'Exclude Lines Longer Than n Milliseconds' Subs1 option\n\n"
        + "Range: 100-99999.",
        PrefDefaults.DefaultExcludeLinesLongerThanMsNumSubs1));
      propTable["Exclude Lines Longer Than n Milliseconds Number Subs1"] = ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs1;

      // default_exclude_lines_longer_than_n_ms_num_subs2
      propTable.Properties.Add(new PropertySpec("Exclude Lines Longer Than n Milliseconds Number Subs2", typeof(int),
        "User Interface Defaults",
        "Specify the 'n' in the 'Exclude Lines Longer Than n Milliseconds' Subs2 option\n\n"
        + "Range: 100-99999.",
        PrefDefaults.DefaultExcludeLinesLongerThanMsNumSubs2));
      propTable["Exclude Lines Longer Than n Milliseconds Number Subs2"] = ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs2;

      // default_join_sentences_subs1
      propTable.Properties.Add(new PropertySpec("Join Lines That End With One of the Following Characters Enable Subs1", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Join Lines That End With One of the Following Characters' Subs1 option.",
        PrefDefaults.DefaultJoinSentencesSubs1));
      propTable["Join Lines That End With One of the Following Characters Enable Subs1"] = ConstantSettings.DefaultJoinSentencesSubs1;

      // default_join_sentences_subs2
      propTable.Properties.Add(new PropertySpec("Join Lines That End With One of the Following Characters Enable Subs2", typeof(bool),
        "User Interface Defaults",
        "Enable/Disable the 'Join Lines That End With One of the Following Characters' Subs2 option.",
        PrefDefaults.DefaultJoinSentencesSubs2));
      propTable["Join Lines That End With One of the Following Characters Enable Subs2"] = ConstantSettings.DefaultJoinSentencesSubs2;

      // default_join_sentences_char_list_subs1
      propTable.Properties.Add(new PropertySpec("Join Lines That End With One of the Following Characters Subs1", typeof(string),
        "User Interface Defaults",
        "Specify the list of characters in the 'Join Lines That End With One of the Following Characters' Subs1 option.",
        PrefDefaults.DefaultJoinSentencesCharListSubs1));
      propTable["Join Lines That End With One of the Following Characters Subs1"] = convertToTokens(ConstantSettings.DefaultJoinSentencesCharListSubs1);

      // default_join_sentences_char_list_subs2
      propTable.Properties.Add(new PropertySpec("Join Lines That End With One of the Following Characters Subs2", typeof(string),
        "User Interface Defaults",
        "Specify the list of characters in the 'Join Lines That End With One of the Following Characters' Subs2 option.",
        PrefDefaults.DefaultJoinSentencesCharListSubs2));
      propTable["Join Lines That End With One of the Following Characters Subs2"] = convertToTokens(ConstantSettings.DefaultJoinSentencesCharListSubs2);

      // srs_filename_format
      propTable.Properties.Add(new PropertySpec("SRS Filename Format", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for SRS (ex. Anki) import filename.\n\n"
        + "Supported Tokens: All except ${episode_num}, ${sequence_num}, ${subs1_line}, ${subs2_line}, or any of the time tokens.",
        convertToTokens(PrefDefaults.SrsFilenameFormat)));
      propTable["SRS Filename Format"] = convertToTokens(ConstantSettings.SrsFilenameFormat);

      // srs_delimiter
      propTable.Properties.Add(new PropertySpec("Delimiter", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The delimeter to use for the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: Only ${tab}.",
        convertToTokens(PrefDefaults.SrsDelimiter)));
      propTable["Delimiter"] = convertToTokens(ConstantSettings.SrsDelimiter);

      // srs_tag_format
      propTable.Properties.Add(new PropertySpec("Tag Format", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the tag in the SRS (ex. Anki) import file. Leave blank if you do not want to include it.\n"
        + "Note: The tag is normally the first column of the import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsTagFormat)));
      propTable["Tag Format"] = convertToTokens(ConstantSettings.SrsTagFormat);

      // srs_sequence_marker_format
      propTable.Properties.Add(new PropertySpec("Sequence Marker Format", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the sequence marker in the SRS (ex. Anki) import file. Leave blank if you do not want to include it.\n"
        + "Note: The sequence marker is normally the second column of the import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsSequenceMarkerFormat)));
      propTable["Sequence Marker Format"] = convertToTokens(ConstantSettings.SrsSequenceMarkerFormat);

      // srs_audio_filename_prefix
      propTable.Properties.Add(new PropertySpec("Audio Clip Prefix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the prefix of the audio entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsAudioFilenamePrefix)));
      propTable["Audio Clip Prefix"] = convertToTokens(ConstantSettings.SrsAudioFilenamePrefix);

      // audio_filename_format
      propTable.Properties.Add(new PropertySpec("Audio Clip Filename Format", typeof(string),
        "Audio Clip Formatting (Uses Tokens)",
        "The format to use for audio clip filenames. You must ensure that each filename will be unique.\n\n"
        + "Supported Tokens: All. But don't use ${subs1_line} or ${subs2_line} if they will contain non-ASCII (ex. Japanese) characters.",
        convertToTokens(PrefDefaults.AudioFilenameFormat)));
      propTable["Audio Clip Filename Format"] = convertToTokens(ConstantSettings.AudioFilenameFormat);

      // audio_id3_artist
      propTable.Properties.Add(new PropertySpec("ID3 Tag Artist", typeof(string),
        "Audio Clip Formatting (Uses Tokens)",
        "The format to use for the audio file's ID3 Artist tag.\n\n"
        + "Supported Tokens: All. However, in the Extract Audio from Media dialog, the following have no affect: ${subs1_line}, ${subs2_line}, ${width}, ${height}.",
        convertToTokens(PrefDefaults.AudioId3Artist)));
      propTable["ID3 Tag Artist"] = convertToTokens(ConstantSettings.AudioId3Artist);

      // audio_id3_album
      propTable.Properties.Add(new PropertySpec("ID3 Tag Album", typeof(string),
        "Audio Clip Formatting (Uses Tokens)",
        "The format to use for the audio file's ID3 Album tag.\n\n"
        + "Supported Tokens: All. However, in the Extract Audio from Media dialog, the following have no affect: ${subs1_line}, ${subs2_line}, ${width}, ${height}.",
        convertToTokens(PrefDefaults.AudioId3Album)));
      propTable["ID3 Tag Album"] = convertToTokens(ConstantSettings.AudioId3Album);

      // audio_id3_title
      propTable.Properties.Add(new PropertySpec("ID3 Tag Title", typeof(string),
        "Audio Clip Formatting (Uses Tokens)",
        "The format to use for the audio file's ID3 Title tag.\n\n"
        + "Supported Tokens: All. However, in the Extract Audio from Media dialog, the following have no affect: ${subs1_line}, ${subs2_line}, ${width}, ${height}.",
        convertToTokens(PrefDefaults.AudioId3Title)));
      propTable["ID3 Tag Title"] = convertToTokens(ConstantSettings.AudioId3Title);

      // audio_id3_genre
      propTable.Properties.Add(new PropertySpec("ID3 Tag Genre", typeof(string),
        "Audio Clip Formatting (Uses Tokens)",
        "The format to use for the audio file's ID3 Genre tag.\n\n"
        + "Supported Tokens: All. However, in the Extract Audio from Media dialog, the following have no affect: ${subs1_line}, ${subs2_line}, ${width}, ${height}.",
        convertToTokens(PrefDefaults.AudioId3Genre)));
      propTable["ID3 Tag Genre"] = convertToTokens(ConstantSettings.AudioId3Genre);

      // audio_id3_lyrics
      propTable.Properties.Add(new PropertySpec("ID3 Tag Lyrics", typeof(string),
        "Audio Clip Formatting (Uses Tokens)",
        "The format to use for the audio file's ID3 Lyrics tag. "
        + "If you want to change lyrics tags that the Extract Audio from Media tool generates, than you need to "
        + "change the lyrics setting in the Extract Audio from Media Formats section.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.AudioId3Lyrics)));
      propTable["ID3 Tag Lyrics"] = convertToTokens(ConstantSettings.AudioId3Lyrics);

      // srs_audio_filename_suffix
      propTable.Properties.Add(new PropertySpec("Audio Clip Suffix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the suffix of the audio entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsAudioFilenameSuffix)));
      propTable["Audio Clip Suffix"] = convertToTokens(ConstantSettings.SrsAudioFilenameSuffix);

      // srs_snapshot_filename_prefix
      propTable.Properties.Add(new PropertySpec("Snapshot Prefix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the prefix of the snapshot entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsSnapshotFilenamePrefix)));
      propTable["Snapshot Prefix"] = convertToTokens(ConstantSettings.SrsSnapshotFilenamePrefix);

      // snapshot_filename_format
      propTable.Properties.Add(new PropertySpec("Snapshot Filename Format", typeof(string),
        "Snapshot Formatting (Uses Tokens)",
        "The format to use for snapshot filenames. You must ensure that each filename will be unique.\n\n"
        + "Supported Tokens: All. But don't use ${subs1_line} or ${subs2_line} if they will contain non-ASCII (ex. Japanese) characters.",
        convertToTokens(PrefDefaults.SnapshotFilenameFormat)));
      propTable["Snapshot Filename Format"] = convertToTokens(ConstantSettings.SnapshotFilenameFormat);

      // srs_snapshot_filename_suffix
      propTable.Properties.Add(new PropertySpec("Snapshot Suffix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the suffix of the snapshot entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsSnapshotFilenameSuffix)));
      propTable["Snapshot Suffix"] = convertToTokens(ConstantSettings.SrsSnapshotFilenameSuffix);

      // srs_video_filename_prefix
      propTable.Properties.Add(new PropertySpec("Video Clip Prefix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the prefix of the video entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsVideoFilenamePrefix)));
      propTable["Video Clip Prefix"] = convertToTokens(ConstantSettings.SrsVideoFilenamePrefix);

      // video_filename_format
      propTable.Properties.Add(new PropertySpec("Video Clip Filename Format", typeof(string),
        "Video Formatting (Uses Tokens)",
        "The format to use for video clip filenames. You must ensure that each filename will be unique.\n\n"
        + "Supported Tokens: All. But don't use ${subs1_line} or ${subs2_line} if they will contain non-ASCII (ex. Japanese) characters.",
        convertToTokens(PrefDefaults.VideoFilenameFormat)));
      propTable["Video Clip Filename Format"] = convertToTokens(ConstantSettings.VideoFilenameFormat);

      // srs_video_filename_suffix
      propTable.Properties.Add(new PropertySpec("Video Clip Suffix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the suffix of the video entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsVideoFilenameSuffix)));
      propTable["Video Clip Suffix"] = convertToTokens(ConstantSettings.SrsVideoFilenameSuffix);

      // srs_vobsub_filename_prefix
      propTable.Properties.Add(new PropertySpec("Vobsub Prefix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the prefix of the vobsub entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: Only ${deck_name}.",
        convertToTokens(PrefDefaults.SrsVobsubFilenamePrefix)));
      propTable["Vobsub Prefix"] = convertToTokens(ConstantSettings.SrsVobsubFilenamePrefix);

      // srs_vobsub_filename_suffix
      propTable.Properties.Add(new PropertySpec("Vobsub Suffix", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use for the suffix of the vobsub entry in the SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: Only ${deck_name}.",
        convertToTokens(PrefDefaults.SrsVobsubFilenameSuffix)));
      propTable["Vobsub Suffix"] = convertToTokens(ConstantSettings.SrsVobsubFilenameSuffix);

      // srs_subs1_format
      propTable.Properties.Add(new PropertySpec("Subs1 Format", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use when adding Subs1 to SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsSubs1Format)));
      propTable["Subs1 Format"] = convertToTokens(ConstantSettings.SrsSubs1Format);

      // srs_subs2_format
      propTable.Properties.Add(new PropertySpec("Subs2 Format", typeof(string),
        "SRS File Formatting (Uses Tokens)",
        "The format to use when adding Subs2 to SRS (ex. Anki) import file.\n\n"
        + "Supported Tokens: All.",
        convertToTokens(PrefDefaults.SrsSubs2Format)));
      propTable["Subs2 Format"] = convertToTokens(ConstantSettings.SrsSubs2Format);

      // extract_media_audio_filename_format
      propTable.Properties.Add(new PropertySpec("Extract Audio From Media Filename Format", typeof(string),
        "Extract Audio from Media Formats (Uses Tokens)",
        "The format to use for audio filenames in the Extract Audio from Media dialog. "
        + "You must ensure that each filename will be unique.\n\n"
        + "Supported Tokens: All except ${subs1_line}, ${subs2_line}, ${width} and ${height}.",
        convertToTokens(PrefDefaults.ExtractMediaAudioFilenameFormat)));
      propTable["Extract Audio From Media Filename Format"] = convertToTokens(ConstantSettings.ExtractMediaAudioFilenameFormat);

      // extract_media_lyrics_subs1_format
      propTable.Properties.Add(new PropertySpec("Lyrics Subs1 Format", typeof(string),
        "Extract Audio from Media Formats (Uses Tokens)",
        "The format to use when adding Subs1 to the audio file's ID3 Lyrics tag in the Extract Audio from Media dialog.\n\n"
        + "Supported Tokens: All except ${width} and ${height}.",
        convertToTokens(PrefDefaults.ExtractMediaLyricsSubs1Format)));
      propTable["Lyrics Subs1 Format"] = convertToTokens(ConstantSettings.ExtractMediaLyricsSubs1Format);

      // extract_media_lyrics_subs2_format
      propTable.Properties.Add(new PropertySpec("Lyrics Subs2 Format", typeof(string),
        "Extract Audio from Media Formats (Uses Tokens)",
        "The format to use when adding Subs2 to the audio file's ID3 Lyrics tag in the Extract Audio from Media dialog\n\n"
        + "Supported Tokens: All except ${width} and ${height}",
        convertToTokens(PrefDefaults.ExtractMediaLyricsSubs2Format)));
      propTable["Lyrics Subs2 Format"] = convertToTokens(ConstantSettings.ExtractMediaLyricsSubs2Format);

      // dueling_subtitle_filename_format
      propTable.Properties.Add(new PropertySpec("Dueling Subtitles Filename Format", typeof(string),
        "Dueling Subtitles Formats (Uses Tokens)",
        "The format to use for dueling subtitle filenames in the Dueling Subtitles dialog. "
        + "You must ensure that each filename will be unique.\n\n"
        + "Supported Tokens: All except ${sequence_num}, ${subs1_line}, ${subs2_line}, ${width}, and ${height}.",
        convertToTokens(PrefDefaults.DuelingSubtitleFilenameFormat)));
      propTable["Dueling Subtitles Filename Format"] = convertToTokens(ConstantSettings.DuelingSubtitleFilenameFormat);

      // dueling_quick_ref_filename_format
      propTable.Properties.Add(new PropertySpec("Quick Reference Filename Format", typeof(string),
        "Dueling Subtitles Formats (Uses Tokens)",
        "The format to use for dueling subtitle quick reference filenames in the Dueling Subtitles dialog. "
        + "You must ensure that each filename will be unique.\n\n"
        + "Supported Tokens: All except ${sequence_num}, ${subs1_line}, ${subs2_line}, ${width}, and ${height}.",
        convertToTokens(PrefDefaults.DuelingQuickRefFilenameFormat)));
      propTable["Quick Reference Filename Format"] = convertToTokens(ConstantSettings.DuelingQuickRefFilenameFormat);

      // dueling_quick_ref_subs1_format
      propTable.Properties.Add(new PropertySpec("Quick Reference Subs1 Format", typeof(string),
        "Dueling Subtitles Formats (Uses Tokens)",
        "The format to use when adding Subs1 to the quick reference file in the Dueling Subtitles dialog.\n\n"
        + "Supported Tokens: All except ${width}, and ${height}.",
        convertToTokens(PrefDefaults.DuelingQuickRefSubs1Format)));
      propTable["Quick Reference Subs1 Format"] = convertToTokens(ConstantSettings.DuelingQuickRefSubs1Format);

      // dueling_quick_ref_subs2_format
      propTable.Properties.Add(new PropertySpec("Quick Reference Subs2 Format", typeof(string),
        "Dueling Subtitles Formats (Uses Tokens)",
        "The format to use when adding Subs2 to the quick reference file in the Dueling Subtitles dialog. " 
        + "Leave blank if you don't want to use this setting (for example if you combined both "
        + "${subs1_line} ${subs2_line} into Quick Reference Subs1 Format.\n\n"
        + "Supported Tokens: All except ${width}, and ${height}.",
        convertToTokens(PrefDefaults.DuelingQuickRefSubs2Format)));
      propTable["Quick Reference Subs2 Format"] = convertToTokens(ConstantSettings.DuelingQuickRefSubs2Format);

      // video_player
      propTable.Properties.Add(new PropertySpec("Video Player Path", typeof(string),
        "Video Player (Uses Tokens)",
        "The video player to use in the Preview dialog. When this setting is blank, the Preview Video button" +
        " will be removed the the Preview dialog. Be sure to set Video Player Arguments.\n\n"
        + "Supported Tokens: None.",
        PrefDefaults.VideoPlayer));
      propTable["Video Player Path"] = convertToTokens(ConstantSettings.VideoPlayer);

      // video_player_args
      propTable.Properties.Add(new PropertySpec("Video Player Arguments", typeof(string),
        "Video Player (Uses Tokens)",
        "The video player arguments to pass to the video player in the Preview dialog.\n\n"
        + "Supported Tokens: All except ${subs1_line}, ${subs2_line}, ${total_line_num} and ${sequence_num}.",
        convertToTokens(PrefDefaults.VideoPlayerArgs)));
      propTable["Video Player Arguments"] = convertToTokens(ConstantSettings.VideoPlayerArgs);

      // reencode_before_splitting_audio
      propTable.Properties.Add(new PropertySpec("Re-encode Before Splitting Audio", typeof(bool),
        "Misc",
        "This setting affects the processing of the .mp3 files that you provide in "
        + "the Audio textbox of the Generate Audio Clips section of the main interface. "
        + "When set, subs2srs will re-encode the mp3 before splitting it. This is useful "
        + "for certain (possibly malformed) .mp3 files that subs2srs has trouble "
        + "splitting at the correct times. Setting this option will increase "
        + "processing time by a small amount.",
        PrefDefaults.ReencodeBeforeSplittingAudio));
      propTable["Re-encode Before Splitting Audio"] = ConstantSettings.ReencodeBeforeSplittingAudio;

      // enable_logging
      propTable.Properties.Add(new PropertySpec("Enable Logging", typeof(bool),
        "Misc",
        "Enable logging. Logs will be placed in the Logs directory. Up to " +  ConstantSettings.MaxLogFiles
        + " logs will be stored. This preference will take effect when subs2srs is restarted.",
        PrefDefaults.EnableLogging));
      propTable["Enable Logging"] = ConstantSettings.EnableLogging;

      // audio_normalize_args
      propTable.Properties.Add(new PropertySpec("Normalize Audio Arguments", typeof(string),
        "Misc",
        "The arguments to pass to mp3gain (the tool used to normalize the audio).\n"
        + " \"{Media_Directory}\\*.mp3\" will be appended to this.\n\n"
        + "Options for mp3gain:\n"
        + "\n"
        + "  /g <i>  - apply gain i without doing any analysis\n"
        + "  /l 0 <i> - apply gain i to channel 0 (left channel)\n"
        + "            without doing any analysis (ONLY works for STEREO files,\n"
        + "            not Joint Stereo)\n"
        + "  /l 1 <i> - apply gain i to channel 1 (right channel)\n"
        + "  /e - skip Album analysis, even if multiple files listed\n"
        + "  /r - apply Track gain automatically (all files set to equal loudness)\n"
        + "  /k - automatically lower Track/Album gain to not clip audio\n"
        + "  /a - apply Album gain automatically (files are all from the same\n"
        + "                album: a single gain change is applied to all files, so\n"
        + "                their loudness relative to each other remains unchanged,\n"
        + "                but the average album loudness is normalized)\n"
        + "  /m <i> - modify suggested MP3 gain by integer i\n"
        + "  /d <n> - modify suggested dB gain by floating-point n\n"
        + "  /c - ignore clipping warning when applying gain\n"
        + "  /o - output is a database-friendly tab-delimited list\n"
        + "  /t - writes modified data to temp file, then deletes original\n"
        + "       instead of modifying bytes in original file\n"
        + "  /q - Quiet mode: no status messages\n"
        + "  /p - Preserve original file timestamp\n"
        + "  /x - Only find max. amplitude of file\n"
        + "  /f - Assume input file is an MPEG 2 Layer III file\n"
        + "       (i.e. don't check for mis-named Layer I or Layer II files)\n"
        + "  /s c - only check stored tag info (no other processing)\n"
        + "  /s d - delete stored tag info (no other processing)\n"
        + "  /s s - skip (ignore) stored tag info (do not read or write tags)\n"
        + "  /s r - force re-calculation (do not read tag info)\n"
        + "  /s i - use ID3v2 tag for MP3 gain info\n"
        + "  /s a - use APE tag for MP3 gain info (default)\n"
        + "  /u - undo changes made (based on stored tag info)\n"
        + "  /w - \"wrap\" gain change if gain+change > 255 or gain+change < 0\n"
        + "        (use \"/? wrap\" switch for a complete explanation)\n"
        + "If you specify /r and /a, only the second one will work\n"
        + "If you do not specify /c, the program will stop and ask before\n"
        + "     applying gain change to a file that might clip\n",
        PrefDefaults.AudioNormalizeArgs));
      propTable["Normalize Audio Arguments"] = ConstantSettings.AudioNormalizeArgs;

      // long_clip_warning_seconds
      propTable.Properties.Add(new PropertySpec("Long Clip Warning", typeof(int),
        "Misc",
        "If a line of dialog's duration exceeds the specified number of seconds, display"
        + " a warning when the \"Go!\" button is pressed in the preview dialog.\n\n"
        + "Range: 0-99999. To disable this feature, set to 0.",
        PrefDefaults.LongClipWarningSeconds));
      propTable["Long Clip Warning"] = ConstantSettings.LongClipWarningSeconds;

      propertyGridPref.SelectedObject = propTable;
    }


    private void DialogPref_Load(object sender, EventArgs e)
    {
      this.resizeDescriptionArea(ref propertyGridPref, 6);
    }


    /// <summary>
    /// Resize the description portion of the provided property grid. See:
    /// http://www.codeproject.com/Articles/28193/Change-the-height-of-a-PropertyGrid-s-description
    /// </summary>
    private void resizeDescriptionArea(ref PropertyGrid grid, int numLines)
    {
      try
      {
        System.Reflection.PropertyInfo pi = grid.GetType().GetProperty("Controls");
        System.Windows.Forms.Control.ControlCollection cc = (System.Windows.Forms.Control.ControlCollection)pi.GetValue(grid, null);
        foreach (Control c in cc)
        {
          Type ct = c.GetType();
          string sName = ct.Name;

          if (sName == "DocComment")
          {
            pi = ct.GetProperty("Lines");
            pi.SetValue(c, numLines, null);

            System.Reflection.FieldInfo fi = ct.BaseType.GetField("userSized",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic);

            fi.SetValue(c, true);
          }
        }
      }
      catch
      {
        // Don't care
      }
    }


    /// <summary>
    /// Replace tabs and newlines with their token versions.
    /// </summary>
    private string convertToTokens(string format)
    {
      string newFomat = format;

      newFomat = newFomat.Replace("\t", "${tab}");
      newFomat = newFomat.Replace("\r", "${cr}");
      newFomat = newFomat.Replace("\n", "${lf}");

      return newFomat;
    }

  
    /// <summary>
    /// Convert blank preference value to "none"
    /// </summary>
    private string convertOut(string pref)
    {
      string value = (string)propTable[pref];

      if (value.Trim() == "")
      {
        value = "none";
      }

      return value;
    }


    /// <summary>
    /// Is the provided encoding preference valid?
    /// </summary>
    private string checkValidEncoding(string encoding, string def)
    {
      string newEncoding = def;

      if (InfoEncoding.isValidShortEncoding(encoding))
      {
        newEncoding = encoding;
      }

      return newEncoding;
    }


    /// <summary>
    /// Use default if preference's value is blank.
    /// </summary>
    private string preventBlank(string pref)
    {
      string value = (string)propTable[pref];

      if (value.Trim() == "")
      {
        value = getPrefDefault(pref);
      }

      return value;
    }


    /// <summary>
    /// Get the default value of a preference.
    /// </summary>
    private string getPrefDefault(string pref)
    {
      string def = "";

      for (int i = 0; i < propTable.Properties.Count; i++)
      {
        if (propTable.Properties[i].Name == pref)
        {
          def = propTable.Properties[i].DefaultValue.ToString();
          break;
        }
      }

      return def;
    }


    /// <summary>
    /// Write the preferences to file.
    /// </summary>
    private void buttonOK_Click(object sender, EventArgs e)
    {
      List<PrefIO.SettingsPair> pairList = new List<PrefIO.SettingsPair>();

      pairList.Add(new PrefIO.SettingsPair("main_window_width", 
        UtilsCommon.checkRange((int)propTable["Main Window Width"], 0, 9999, PrefDefaults.MainWindowWidth).ToString()));
      pairList.Add(new PrefIO.SettingsPair("main_window_height", 
        UtilsCommon.checkRange((int)propTable["Main Window Height"], 0, 9999, PrefDefaults.MainWindowHeight).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_enable_audio_clip_generation", propTable["Enable Audio Clip Generation"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_enable_snapshots_generation", propTable["Enable Snapshots Generation"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_enable_video_clips_generation", propTable["Enable Video Clips Generation"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_audio_clip_bitrate", 
        UtilsCommon.checkRangeInSet<int>((int)propTable["Audio Clip Bitrate"], 
        new List<int>(new int[] { 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 192, 224, 256, 320 }), PrefDefaults.DefaultAudioClipBitrate).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_audio_normalize", propTable["Normalize Audio"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_video_clip_video_bitrate", 
        UtilsCommon.checkRange((int)propTable["Video Clip Video Bitrate"], 100, 3000, PrefDefaults.DefaultVideoClipVideoBitrate).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_video_clip_audio_bitrate", 
        UtilsCommon.checkRangeInSet<int>((int)propTable["Video Clip Audio Bitrate"],
        new List<int>(new int[] { 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 192, 224, 256, 320 }), PrefDefaults.DefaultVideoClipAudioBitrate).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_ipod_support", propTable["iPhone Support"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_encoding_subs1", 
        checkValidEncoding((string)propTable["Encoding Subs1"], PrefDefaults.DefaultEncodingSubs1)));
      pairList.Add(new PrefIO.SettingsPair("default_encoding_subs2", 
        checkValidEncoding((string)propTable["Encoding Subs2"], PrefDefaults.DefaultEncodingSubs2)));
      pairList.Add(new PrefIO.SettingsPair("default_context_num_leading", 
        UtilsCommon.checkRange((int)propTable["Context Number Leading"], 0, 9, PrefDefaults.DefaultContextNumLeading).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_context_num_trailing", 
        UtilsCommon.checkRange((int)propTable["Context Number Trailing"], 0, 9, PrefDefaults.DefaultContextNumTrailing).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_context_leading_range",
        UtilsCommon.checkRange((int)propTable["Context Nearby Line Range Leading"], 0, 99999, PrefDefaults.DefaultContextLeadingRange).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_context_trailing_range",
        UtilsCommon.checkRange((int)propTable["Context Nearby Line Range Trailing"], 0, 99999, PrefDefaults.DefaultContextTrailingRange).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_file_browser_start_dir", convertOut("File Browser Start Folder")));
      pairList.Add(new PrefIO.SettingsPair("default_remove_styled_lines_subs1", propTable["Remove Subs1 Styled Lines"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_remove_styled_lines_subs2", propTable["Remove Subs2 Styled Lines"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_remove_no_counterpart_subs1", propTable["Remove Subs1 Lines With No Obvious Counterpart"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_remove_no_counterpart_subs2", propTable["Remove Subs2 Lines With No Obvious Counterpart"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_included_text_subs1", convertOut("Included Text Subs1")));
      pairList.Add(new PrefIO.SettingsPair("default_included_text_subs2", convertOut("Included Text Subs2")));
      pairList.Add(new PrefIO.SettingsPair("default_excluded_text_subs1", convertOut("Excluded Text Subs1")));
      pairList.Add(new PrefIO.SettingsPair("default_excluded_text_subs2", convertOut("Excluded Text Subs2")));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_duplicate_lines_subs1", propTable["Exclude Duplicate Lines Subs1"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_duplicate_lines_subs2", propTable["Exclude Duplicate Lines Subs2"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_with_fewer_than_n_chars_subs1", propTable["Exclude Lines With Fewer Than n Characters Enable Subs1"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_with_fewer_than_n_chars_subs2", propTable["Exclude Lines With Fewer Than n Characters Enable Subs2"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_with_fewer_than_n_chars_num_subs1",
              UtilsCommon.checkRange((int)propTable["Exclude Lines With Fewer Than n Characters Number Subs1"], 2, 99999, PrefDefaults.DefaultExcludeLinesFewerThanCharsNumSubs1).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_with_fewer_than_n_chars_num_subs2",
              UtilsCommon.checkRange((int)propTable["Exclude Lines With Fewer Than n Characters Number Subs2"], 2, 99999, PrefDefaults.DefaultExcludeLinesFewerThanCharsNumSubs2).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_shorter_than_n_ms_subs1", propTable["Exclude Lines Shorter Than n Milliseconds Enable Subs1"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_shorter_than_n_ms_subs2", propTable["Exclude Lines Shorter Than n Milliseconds Enable Subs2"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_shorter_than_n_ms_num_subs1",
              UtilsCommon.checkRange((int)propTable["Exclude Lines Shorter Than n Milliseconds Number Subs1"], 100, 99999, PrefDefaults.DefaultExcludeLinesShorterThanMsNumSubs1).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_shorter_than_n_ms_num_subs2",
              UtilsCommon.checkRange((int)propTable["Exclude Lines Shorter Than n Milliseconds Number Subs2"], 100, 99999, PrefDefaults.DefaultExcludeLinesShorterThanMsNumSubs2).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_longer_than_n_ms_subs1", propTable["Exclude Lines Longer Than n Milliseconds Enable Subs1"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_longer_than_n_ms_subs2", propTable["Exclude Lines Longer Than n Milliseconds Enable Subs2"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_longer_than_n_ms_num_subs1",
              UtilsCommon.checkRange((int)propTable["Exclude Lines Longer Than n Milliseconds Number Subs1"], 100, 99999, PrefDefaults.DefaultExcludeLinesLongerThanMsNumSubs1).ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_exclude_lines_longer_than_n_ms_num_subs2",
              UtilsCommon.checkRange((int)propTable["Exclude Lines Longer Than n Milliseconds Number Subs2"], 100, 99999, PrefDefaults.DefaultExcludeLinesLongerThanMsNumSubs2).ToString()));
      pairList.Add(new PrefIO.SettingsPair("srs_filename_format", preventBlank("SRS Filename Format")));
      pairList.Add(new PrefIO.SettingsPair("default_join_sentences_subs1", propTable["Join Lines That End With One of the Following Characters Enable Subs1"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_join_sentences_subs2", propTable["Join Lines That End With One of the Following Characters Enable Subs2"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("default_join_sentences_char_list_subs1", preventBlank("Join Lines That End With One of the Following Characters Subs1")));
      pairList.Add(new PrefIO.SettingsPair("default_join_sentences_char_list_subs2", preventBlank("Join Lines That End With One of the Following Characters Subs2")));
      pairList.Add(new PrefIO.SettingsPair("srs_delimiter", preventBlank("Delimiter")));
      pairList.Add(new PrefIO.SettingsPair("srs_tag_format", convertOut("Tag Format")));
      pairList.Add(new PrefIO.SettingsPair("srs_sequence_marker_format", convertOut("Sequence Marker Format")));
      pairList.Add(new PrefIO.SettingsPair("srs_audio_filename_prefix", convertOut("Audio Clip Prefix")));
      pairList.Add(new PrefIO.SettingsPair("audio_filename_format", preventBlank("Audio Clip Filename Format")));
      pairList.Add(new PrefIO.SettingsPair("audio_id3_artist", convertOut("ID3 Tag Artist")));
      pairList.Add(new PrefIO.SettingsPair("audio_id3_album", convertOut("ID3 Tag Album")));
      pairList.Add(new PrefIO.SettingsPair("audio_id3_title", convertOut("ID3 Tag Title")));
      pairList.Add(new PrefIO.SettingsPair("audio_id3_genre", convertOut("ID3 Tag Genre")));
      pairList.Add(new PrefIO.SettingsPair("audio_id3_lyrics", convertOut("ID3 Tag Lyrics")));
      pairList.Add(new PrefIO.SettingsPair("srs_audio_filename_suffix", convertOut("Audio Clip Suffix")));
      pairList.Add(new PrefIO.SettingsPair("srs_snapshot_filename_prefix", convertOut("Snapshot Prefix")));
      pairList.Add(new PrefIO.SettingsPair("snapshot_filename_format", preventBlank("Snapshot Filename Format")));
      pairList.Add(new PrefIO.SettingsPair("srs_snapshot_filename_suffix", convertOut("Snapshot Suffix")));
      pairList.Add(new PrefIO.SettingsPair("srs_video_filename_prefix", convertOut("Video Clip Prefix")));
      pairList.Add(new PrefIO.SettingsPair("video_filename_format", preventBlank("Video Clip Filename Format")));
      pairList.Add(new PrefIO.SettingsPair("srs_video_filename_suffix", convertOut("Video Clip Suffix")));
      pairList.Add(new PrefIO.SettingsPair("srs_vobsub_filename_prefix", convertOut("Vobsub Prefix")));
      pairList.Add(new PrefIO.SettingsPair("srs_vobsub_filename_suffix", convertOut("Vobsub Suffix")));
      pairList.Add(new PrefIO.SettingsPair("srs_subs1_format", preventBlank("Subs1 Format")));
      pairList.Add(new PrefIO.SettingsPair("srs_subs2_format", preventBlank("Subs2 Format")));
      pairList.Add(new PrefIO.SettingsPair("extract_media_audio_filename_format", preventBlank("Extract Audio From Media Filename Format")));
      pairList.Add(new PrefIO.SettingsPair("extract_media_lyrics_subs1_format", preventBlank("Lyrics Subs1 Format")));
      pairList.Add(new PrefIO.SettingsPair("extract_media_lyrics_subs2_format", convertOut("Lyrics Subs2 Format")));
      pairList.Add(new PrefIO.SettingsPair("dueling_subtitle_filename_format", preventBlank("Dueling Subtitles Filename Format")));
      pairList.Add(new PrefIO.SettingsPair("dueling_quick_ref_filename_format", preventBlank("Quick Reference Filename Format")));
      pairList.Add(new PrefIO.SettingsPair("dueling_quick_ref_subs1_format", preventBlank("Quick Reference Subs1 Format")));
      pairList.Add(new PrefIO.SettingsPair("dueling_quick_ref_subs2_format", convertOut("Quick Reference Subs2 Format")));
      pairList.Add(new PrefIO.SettingsPair("video_player", convertOut("Video Player Path")));
      pairList.Add(new PrefIO.SettingsPair("video_player_args", convertOut("Video Player Arguments")));
      pairList.Add(new PrefIO.SettingsPair("reencode_before_splitting_audio", propTable["Re-encode Before Splitting Audio"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("enable_logging", propTable["Enable Logging"].ToString()));
      pairList.Add(new PrefIO.SettingsPair("audio_normalize_args", convertOut("Normalize Audio Arguments")));
      pairList.Add(new PrefIO.SettingsPair("long_clip_warning_seconds",
              UtilsCommon.checkRange((int)propTable["Long Clip Warning"], 0, 99999, PrefDefaults.LongClipWarningSeconds).ToString()));

      // Write the settings to file
      PrefIO.writeString(pairList);

      // Read the setting file to update the settings
      PrefIO.read();
      
      this.Close();
    }
 
    /// <summary>
    /// Reset all preferences to default values.
    /// </summary>
    private void buttonResetAllDefaults_Click(object sender, EventArgs e)
    {
      bool confirmed = UtilsMsg.showConfirm("Are you sure that you want to reset all preferences to default values?");

      if (!confirmed)
      {
        return;
      }

      for (int i = 0; i < propTable.Properties.Count; i++)
      {
        propTable[propTable.Properties[i].Name] = propTable.Properties[i].DefaultValue;
      }

      propertyGridPref.SelectedObject = propTable;
    }


    /// <summary>
    /// Reset the currently selected preference to it's value.
    /// </summary>
    private void buttonResultItem_Click(object sender, EventArgs e)
    {
      GridItem item = this.propertyGridPref.SelectedGridItem;

      for (int i = 0; i < propTable.Properties.Count; i++)
      {
        if (propTable.Properties[i].Name == item.Label)
        {
          propTable[item.Label] = propTable.Properties[i].DefaultValue;
          break;
        }
      }

      propertyGridPref.SelectedObject = propTable;
    }


    /// <summary>
    /// Get the system's default browser. See:
    /// http://stackoverflow.com/questions/2646825/launching-default-browser-with-html-from-file-then-jump-to-specific-anchor
    /// </summary>
    private string getDefaultBrowserPath()
    {
      string key = @"HTTP\shell\open\command";

      using (RegistryKey registrykey = Registry.ClassesRoot.OpenSubKey(key, false))
      {
        return ((string)registrykey.GetValue(null, null)).Split('"')[1];
      }
    }

    /// <summary>
    /// Goto URL that contains anchor. Example: www.example.com#help.
    /// </summary>
    private void goToAnchor(string url)
    {
      System.Diagnostics.Process p = new System.Diagnostics.Process();
      p.StartInfo.FileName = getDefaultBrowserPath();
      p.StartInfo.Arguments = url;
      p.Start();
    }


    /// <summary>
    /// Show the tokens section of the help file.
    /// </summary>
    /// <param name="sender"></param>
    private void buttonTokenList_Click(object sender, EventArgs e)
    {
      try
      {
        string target = String.Format("{0}#prefs_tokens", ConstantSettings.HelpPage);
        goToAnchor(target);
      }
      catch
      {
        UtilsMsg.showErrMsg("Help page not found.");
      }
    }




 
  }
}
